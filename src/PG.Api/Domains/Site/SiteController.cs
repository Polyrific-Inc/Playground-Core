// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PG.Api.Domains.Base;
using PG.Api.Domains.Facility;
using PG.BLL;
using PG.Common;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace PG.Api.Domains.Site
{
    [Route("Site")]
    public class SiteController : BaseController<NewSiteDto, EditSiteDto, SiteDto, Model.Site, ISiteService>
    {
        public SiteController(ISiteService siteService, ILogger<SiteController> logger)
            : base(siteService, logger)
        {
            
        }

        /// <summary>
        /// Get a site by it's ID
        /// </summary>        
        /// <param name="id">Site ID to get</param>
        /// <returns>
        /// The site data if found
        /// </returns>
        /// <response code="200">Returns the site data</response>
        /// <response code="404">If the site is not found</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("{id}", Name = "GetSiteById")]
        public ActionResult<SiteDto> Get(int id)
        {
            return base.Get(id);
        }

        /// <summary>
        /// Create new Site data
        /// </summary>
        /// <param name="value">Values to create the new Site</param>
        /// <returns>
        /// The newly created Site data
        /// </returns>
        /// <response code="201">Returns the newly created Site</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        //[Authorize]
        [HttpPost("")]
        public IActionResult Post([FromBody] NewSiteDto value)
        {
            return base.Post(value, "GetSiteById");
        }

        /// <summary>
        /// Update site data
        /// </summary>
        /// <param name="id">Site id to update</param>
        /// <param name="value">New value for update the site</param>
        /// <returns>
        /// Returns the updated Site data
        /// </returns>
        /// <response code="200">Returns the updated Site data</response>
        /// <response code="404">If the site is not found</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        //[Authorize]
        [HttpPut("{id}")]
        public ActionResult<SiteDto> Put(int id, [FromBody] EditSiteDto value)
        {
            return base.Put(id, value);
        }

        /// <summary>
        /// Delete a site
        /// </summary>
        /// <param name="id">User id to delete</param>
        /// <response code="204">If the delete successfull</response>
        /// <response code="404">If the Site is not found</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        //[Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return base.Delete(id);
        }

        /// <summary>
        /// Get sites by name
        /// </summary>        
        /// <param name="name">Site name for search query</param>
        /// <returns>
        /// Sites found according to query
        /// </returns>
        /// <response code="200">Returns the sites</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [HttpGet("n/{name}")]
        public ActionResult<SiteDto> GetSiteByName(string name)
        {
            var list = Svc.GetByName(name);

            var source = list.Items.Select(i =>
            {
                var item = new SiteDto();
                item.LoadFromEntity(i);

                return item;
            });

            return Json(new PagedList<SiteDto>(source, list.PageIndex, list.PageSize, list.TotalCount));
        }

        /// <summary>
        /// Get a site's facilities
        /// </summary>        
        /// <param name="id">Site ID to get the facilities</param>
        /// <returns>
        /// The facilities data if found
        /// </returns>
        /// <response code="200">Returns the facilities data</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [HttpGet("{id}/Facilities")]
        public ActionResult<FacilityDto> GetFacilities(int id)
        {
            var list = Svc.GetFacilities(id);

            var source = list.Items.Select(i =>
            {
                var item = new FacilityDto();
                item.LoadFromEntity(i);

                return item;
            });

            return Json(new PagedList<FacilityDto>(source, list.PageIndex, list.PageSize, list.TotalCount));
        }

        /// <summary>
        /// Create a facility for the site
        /// </summary>
        /// <param name="value">Values to facility</param>
        /// <returns>
        /// The newly created Facility data
        /// </returns>
        /// <response code="201">Returns the newly created Facility</response>
        /// <response code="404">If the site is not found</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        //[Authorize]
        [HttpPost("{id}/AddFacility")]
        public IActionResult AddFacility(int id, [FromBody] NewFacilityDto value)
        {
            var entity = Svc.GetById(id);
            if (entity == null)
                return NotFound();

            var facility = value.ToEntity();
            int facilityId = Svc.AddFacility(id, facility);

            var createdDto = new FacilityDto();
            createdDto.LoadFromEntity(facility);

            return CreatedAtRoute("GetFacilityById", new {id = facilityId}, createdDto);
        }

        /// <summary>
        /// Delete a site's facility
        /// </summary>
        /// <param name="id">Site ID</param>
        /// <param name="facilityId">Facility ID to delete</param>
        /// <response code="204">If the delete successfull</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        //[Authorize]
        [HttpDelete("{id}/RemoveFacility/{facilityId}")]
        public IActionResult RemoveFacility(int id, int facilityId)
        {
            Svc.RemoveFacility(id, facilityId);

            return NoContent();
        }
    }
}
