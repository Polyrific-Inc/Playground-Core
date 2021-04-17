// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PG.BLL;
using PG.Model;

namespace PG.Api.Domains.Base
{
    public abstract class BaseController<TNewDto, TEditDto, TDto, TEntity, TService> : Controller
        where TEntity : BaseModel, new()
        where TNewDto : BaseNewDto<TEntity> 
        where TEditDto : BaseDto<TEntity> 
        where TDto : BaseDto<TEntity>, new()
        where TService : IService<TEntity>
    {
        protected TService Svc;
        protected ILogger Logger;
        protected IMapper Mapper;
        
        protected BaseController(TService service, ILogger logger, IMapper mapper)
        {
            Svc = service;
            Logger = logger;
            Mapper = mapper;
        }
        
        public virtual ActionResult<TDto> Get(int id)
        {
            var entity = Svc.GetById(id);
            if (entity == null)
                return NotFound();

            var item = new TDto();
            item.LoadFromEntity(entity, Mapper);
            
            return Ok(item);
        }
        
        [NonAction]
        public virtual IActionResult Post([FromBody] TNewDto value, string createdAtRouteName)
        {
            var newEntity = Mapper.Map<TEntity>(value);
            var id = Svc.Create(newEntity);

            var item = new TDto();
            item.LoadFromEntity(newEntity, Mapper);
            
            return CreatedAtRoute(createdAtRouteName, new {id}, item);
        }
        
        public virtual ActionResult<TDto> Put(int id, TEditDto value)
        {
            if (id != value.Id)
                return BadRequest();

            var originalEntity = Svc.GetById(id);
            var updatedEntity = Mapper.Map<TEntity>(originalEntity);

            Svc.Update(updatedEntity);

            var item = new TDto();
            item.LoadFromEntity(updatedEntity, Mapper);

            return Ok(item);
        }
        
        public virtual IActionResult Delete(int id)
        {
            Svc.Delete(id);

            return NoContent();
        }
    }
}
