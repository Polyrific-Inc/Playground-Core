// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PG.Api.Domains.Base;
using PG.BLL;

namespace PG.Api.Domains.Facility
{
    [Route("Facility")]
    public class FacilityController : BaseController<NewFacilityDto, EditFacilityDto, FacilityDto, Model.Facility, IFacilityService>
    {
        public FacilityController(IFacilityService service, ILogger<FacilityController> logger) 
            : base(service, logger)
        {
        }

        /// <summary>
        /// Get a Facility by it's ID
        /// </summary>        
        /// <param name="id">Facility ID to get</param>
        /// <returns>
        /// The Facility data if found
        /// </returns>
        /// <response code="200">Returns the Facility data</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [HttpGet("{id}", Name = "GetFacilityById")]
        public ActionResult<FacilityDto> Get(int id)
        {
            return base.Get(id);
        }

        /// <summary>
        /// Create new Facility data
        /// </summary>
        /// <param name="value">Values to create the new Facility</param>
        /// <returns>
        /// The newly created Facility data
        /// </returns>
        /// <response code="201">Returns the newly created Facility</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        //[Authorize]
        [HttpPost("")]
        public IActionResult Post([FromBody] NewFacilityDto value)
        {
            return base.Post(value, "GetFacilityById");
        }

        /// <summary>
        /// Update Facility data
        /// </summary>
        /// <param name="id">Facility id to update</param>
        /// <param name="value">New value for update the Facility</param>
        /// <returns>
        /// Returns the updated Facility data
        /// </returns>
        /// <response code="200">Returns the updated Facility data</response>
        /// <response code="400">If there is malsyntax</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        //[Authorize]
        [HttpPut("{id}")]
        public ActionResult<FacilityDto> Put(int id, [FromBody] EditFacilityDto value)
        {
            return base.Put(id, value);
        }

        /// <summary>
        /// Delete a Facility
        /// </summary>
        /// <param name="id">Facility id to delete</param>
        /// <response code="204">If the delete successfull</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        //[Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return base.Delete(id);
        }
    }
}