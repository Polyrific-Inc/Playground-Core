// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Linq.Expressions;
using PG.Common;
using PG.Model;

namespace PG.Repository
{
    public interface IRepository<TEntity> where TEntity: BaseModel
    {
        TEntity Get(int id);
        TEntity Get(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        PagedList<TEntity> Filter<TKey>(int pageIndex, int pageSize, OrderBySelector<TEntity, TKey> orderBySelector, Expression<Func<TEntity, bool>> whereFilter, params Expression<Func<TEntity, object>>[] includeProperties);
        int Create(TEntity newEntity);
        void Update(TEntity updatedEntity);
        void Delete(int id);
    }
}