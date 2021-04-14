// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using PG.Model;

namespace PG.Api.Domains.Base
{
    public abstract class BaseDto<TEntity> : BaseNewDto<TEntity>
        where TEntity : BaseModel, new()
    {
        public int Id { get; set; }

        public virtual void LoadFromEntity(TEntity entity, IMapper mapper)
        {
            Id = entity.Id;
        }
    }
}
