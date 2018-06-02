// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using PG.Common;
using PG.Common.Extensions;
using PG.DataAccess;
using PG.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace PG.Repository
{
    public abstract class BaseRepository<TEntity> where TEntity: BaseModel
    {
        protected PlaygroundDbContext Db;
        protected IDistributedCache Cache;
        protected ILogger Logger;

        protected virtual string SingleCacheKeyPrefix => typeof(TEntity).FullName?.Replace(".", ":");
        
        protected BaseRepository(PlaygroundDbContext dbContext, IDistributedCache cache, ILogger logger)
        {
            Db = dbContext;
            Cache = cache;
            Logger = logger;
        }

        public virtual int Create(TEntity newEntity)
        {
            var dbEntity = GetEntity(newEntity);
            dbEntity.State = EntityState.Added;
            
            Db.SaveChanges();

            var id = newEntity.Id;

            try
            {
                Cache?.SetString($"{SingleCacheKeyPrefix}:{id}", JsonConvert.SerializeObject(newEntity));
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex.GetLastInnerException(), "Failed to store new entity to cache");
            }

            return id;
        }

        public virtual void Delete(int id)
        {
            TEntity entity = Get(id);
            if (entity != null)
            {
                Db.Entry(entity).State = EntityState.Deleted;
                Db.SaveChanges();

                try
                {
                    Cache?.Remove($"{SingleCacheKeyPrefix}:{id}");
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex.GetLastInnerException(), "Failed to remove entity from cache");
                }
            }
        }

        public virtual PagedList<TEntity> Filter<TKey>(int pageIndex, int pageSize, OrderBySelector<TEntity, TKey> orderBySelector, Expression<Func<TEntity, bool>> whereFilter, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> entities = Db.Set<TEntity>();
            foreach (var prop in includeProperties)
            {
                entities = entities.Include(prop);
            }

            entities = orderBySelector.Type == OrderByType.Ascending
                ? entities.OrderBy(orderBySelector.Selector)
                : entities.OrderByDescending(orderBySelector.Selector);

            var query = whereFilter != null ? entities.Where(whereFilter) : entities;

            return query.ToPagedList(pageIndex, pageSize);
        }

        public virtual TEntity Get(int id)
        {
            TEntity entity;

            string cachedEntity = null;

            try
            {
                cachedEntity = Cache?.GetString($"{SingleCacheKeyPrefix}:{id}");
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex.GetLastInnerException(), "Failed to get entity from cache");
            }

            if (string.IsNullOrEmpty(cachedEntity))
            {
                entity = Db.Set<TEntity>().Find(id);

                try
                {
                    Cache?.SetString($"{SingleCacheKeyPrefix}:{id}", JsonConvert.SerializeObject(entity));
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex.GetLastInnerException(), "Failed to store existing entity to cache");
                }
            }
            else
            {
                entity = JsonConvert.DeserializeObject<TEntity>(cachedEntity);
            }
            
            return entity;
        }

        public virtual TEntity Get(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var pagedList = Filter(1, 1, 
                new OrderBySelector<TEntity, int>(OrderByType.Ascending, entity => entity.Id),
                entity => entity.Id == id, includeProperties);

            return pagedList.TotalCount > 0 ? pagedList.Items.FirstOrDefault() : null;
        }

        public virtual void Update(TEntity updatedEntity)
        {
            var dbEntity = GetEntity(updatedEntity);
            dbEntity.State = EntityState.Modified;

            Db.SaveChanges();

            try
            {
                Cache?.Remove($"{SingleCacheKeyPrefix}:{updatedEntity.Id}");
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex.GetLastInnerException(), "Failed to remove updated entity from cache");
            }
        }

        private EntityEntry<TEntity> GetEntity(TEntity entity)
        {
            var dbEntity = Db.Entry(entity);
            if (dbEntity.State == EntityState.Detached)
                Db.Set<TEntity>().Attach(entity);
            return dbEntity;
        }
    }
}