// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PG.Api.Domains.Base;
using PG.BLL;

namespace PG.Api.Domains.UserProfile
{
    [Route("UserProfile")]
    public class UserProfileController : BaseController<UserProfileDto, EditUserProfileDto, UserProfileDto, Model.UserProfile, IUserProfileService>
    {
        public UserProfileController(IUserProfileService service, ILogger<UserProfileController> logger) : base(service, logger)
        {
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetUserProfileById")]
        public new ActionResult<UserProfileDto> Get(int id)
        {
            return base.Get(id);
        }

        [Authorize]
        [HttpPut("{id}")]
        public new ActionResult<UserProfileDto> Put(int id, [FromBody] EditUserProfileDto value)
        {
            return base.Put(id, value);
        }

        [Authorize]
        [HttpGet("u/{username}", Name = "GetUserProfileByUserName")]
        public ActionResult<UserProfileDto> Get(string username)
        {
            var entity = Svc.GetByUserName(username);
            if (entity == null)
                return NotFound();

            var item = new UserProfileDto();
            item.LoadFromEntity(entity);
            
            return Ok(item);
        }
    }
}