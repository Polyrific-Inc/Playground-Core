// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using PG.Model;

namespace PG.BLL
{
    public interface IService<TEntity> where TEntity : BaseModel
    {
        TEntity GetById(int id);
        int Create(TEntity newEntity);
        void Update(TEntity entity);
        void Delete(int id);
    }
}