// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.Extensions.Logging;
using PG.Model;
using PG.Repository;

namespace PG.BLL
{
    public abstract class BaseService<TEntity, TRepository>
        where TEntity : BaseModel
        where TRepository : IRepository<TEntity>
    {
        protected TRepository Repo;
        protected ILogger Logger;
        
        protected BaseService(TRepository repository, ILogger logger)
        {
            Repo = repository;
            Logger = logger;
        }

        public virtual int Create(TEntity newEntity)
        {
            newEntity.Created = DateTime.UtcNow;
            return Repo.Create(newEntity);
        }

        public virtual void Delete(int id)
        {
            Repo.Delete(id);
        }

        public virtual TEntity GetById(int id)
        {
            return Repo.Get(id);
        }

        public virtual void Update(TEntity entity)
        {
            entity.Updated = DateTime.UtcNow;
            Repo.Update(entity);
        }
    }

}