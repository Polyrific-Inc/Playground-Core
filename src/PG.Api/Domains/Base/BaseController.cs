// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

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
        
        protected BaseController(TService service, ILogger logger)
        {
            Svc = service;
            Logger = logger;
        }

        protected ActionResult<TDto> Get(int id)
        {
            var entity = Svc.GetById(id);
            if (entity == null)
                return NotFound();

            var item = new TDto();
            item.LoadFromEntity(entity);
            
            return Ok(item);
        }
        
        protected IActionResult Post([FromBody] TNewDto value, string createdAtRouteName)
        {
            var newEntity = value.ToEntity();
            var id = Svc.Create(newEntity);

            var item = new TDto();
            item.LoadFromEntity(newEntity);
            
            return CreatedAtRoute(createdAtRouteName, new {id}, item);
        }

        protected ActionResult<TDto> Put(int id, TEditDto value)
        {
            if (id != value.Id)
                return BadRequest();

            var originalEntity = Svc.GetById(id);
            if (originalEntity == null)
                return NotFound();

            var updatedEntity = value.ToEntity(originalEntity);

            Svc.Update(updatedEntity);

            var item = new TDto();
            item.LoadFromEntity(updatedEntity);

            return Ok(item);
        }

        protected IActionResult Delete(int id)
        {
            Svc.Delete(id);

            return NoContent();
        }
    }
}
