using AutoMapper;
using PG.Model;

namespace PG.Api.Domains.Base
{
    public abstract class BaseDto<TEntity> : BaseNewDto<TEntity>
        where TEntity : BaseModel, new()
    {
        public int Id { get; set; }

        public virtual TEntity ToEntity(TEntity originalEntity)
        {
            var updatedEntity = originalEntity;

            Mapper.Map(this, updatedEntity);

            return updatedEntity;
        }

        public virtual void LoadFromEntity(TEntity entity)
        {
            Id = entity.Id;
        }
    }
}
