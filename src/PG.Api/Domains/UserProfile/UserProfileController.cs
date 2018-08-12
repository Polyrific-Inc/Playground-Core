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

        /// <summary>
        /// Get User Profile by it's ID
        /// </summary>        
        /// <param name="id">User Profile ID to get</param>
        /// <returns>
        /// The User Profile data if found
        /// </returns>
        /// <response code="200">Returns the User Profile data</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        //[Authorize]
        [HttpGet("{id}", Name = "GetUserProfileById")]
        public ActionResult<UserProfileDto> Get(int id)
        {
            return base.Get(id);
        }

        /// <summary>
        /// Update User Profile data
        /// </summary>
        /// <param name="id">User Profile ID to update</param>
        /// <param name="value">New value for update the User Profile</param>
        /// <returns>
        /// Returns the updated User Profile
        /// </returns>
        /// <response code="200">Returns the updated User Profile data</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        //[Authorize]
        [HttpPut("{id}")]
        public ActionResult<UserProfileDto> Put(int id, [FromBody] EditUserProfileDto value)
        {
            return base.Put(id, value);
        }

        /// <summary>
        /// Get User Profile by username
        /// </summary>        
        /// <param name="username">Username for search query</param>
        /// <returns>
        /// User Profilefound according to query
        /// </returns>
        /// <response code="200">Returns the User Profile</response>
        /// <response code="404">If User Profile not found</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        //[Authorize]
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