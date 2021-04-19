// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PG.Api.Domains.Base;
using PG.BLL;
using Swashbuckle.AspNetCore.Annotations;

namespace PG.Api.Domains.UserProfile
{
    [Route("UserProfile")]
    public class UserProfileController : BaseController<UserProfileDto, EditUserProfileDto, UserProfileDto, Model.UserProfile, IUserProfileService>
    {
        public UserProfileController(IUserProfileService service, ILogger<UserProfileController> logger, IMapper mapper) : base(service, logger, mapper)
        {
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetUserProfileById")]
        [SwaggerOperation(Summary = "Get User By ID")]
        [SwaggerResponse(200, "Success: User found")]
        [SwaggerResponse(404, "Error: User not found")]
        public override ActionResult<UserProfileDto> Get(int id)
        {
            return base.Get(id);
        }

        [Authorize]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update Existing User")]
        [SwaggerResponse(200, "Success: User updated")]
        [SwaggerResponse(400, "Error: User not found")]
        public override ActionResult<UserProfileDto> Put(int id, [FromBody] EditUserProfileDto value)
        {
            return base.Put(id, value);
        }

        [NonAction]
        public override IActionResult Delete(int id)
        {
            return base.Delete(id);
        }

        [Authorize]
        [HttpGet("u/{username}", Name = "GetUserProfileByUserName")]
        [SwaggerOperation(Summary = "Get User By Name")]
        public ActionResult<UserProfileDto> Get(string username)
        {
            var entity = Svc.GetByUserName(username);
            if (entity == null)
                return NotFound();

            var item = new UserProfileDto();
            item.LoadFromEntity(entity, Mapper);
            
            return Ok(item);
        }
    }
}