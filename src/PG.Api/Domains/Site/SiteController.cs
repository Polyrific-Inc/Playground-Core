// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PG.Api.Domains.Base;
using PG.Api.Domains.Facility;
using PG.BLL;
using PG.Common;
using System.Linq;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Swashbuckle.AspNetCore.Annotations;

namespace PG.Api.Domains.Site
{
    [Route("Site")]
    public class SiteController : BaseController<NewSiteDto, EditSiteDto, SiteDto, Model.Site, ISiteService>
    {
        public SiteController(ISiteService siteService, ILogger<SiteController> logger, IMapper mapper)
            : base(siteService, logger, mapper)
        {
        }

        [HttpGet("{id}", Name = "GetSiteById")]
        [SwaggerOperation(Summary = "Get Site By ID")]
        [SwaggerResponse(200, "Success: Site found")]
        [SwaggerResponse(404, "Error: Site not found")]
        public override ActionResult<SiteDto> Get(int id)
        {
            return base.Get(id);
        }

        //[Authorize]
        [HttpPost("")]
        [SwaggerOperation(Summary = "Create New Site")]
        [SwaggerResponse(201, "Success: Site created")]
        public IActionResult Post([FromBody] NewSiteDto value)
        {
            return base.Post(value, "GetSiteById");
        }

        [Authorize]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update Existing Site")]
        [SwaggerResponse(200, "Success: Site updated")]
        [SwaggerResponse(400, "Error: Site not found")]
        public override ActionResult<SiteDto> Put(int id, [FromBody] EditSiteDto value)
        {
            return base.Put(id, value);
        }

        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete Site")]
        [SwaggerResponse(204, "Success: Site deleted")]
        public override IActionResult Delete(int id)
        {
            return base.Delete(id);
        }

        [HttpGet("n/{name}")]
        [SwaggerOperation(Summary = "Get Site By Name")]
        [SwaggerResponse(200, "Success: Site found")]
        public ActionResult<SiteDto> GetSiteByName(string name)
        {
            var list = Svc.GetByName(name);

            var source = list.Items.Select(i =>
            {
                var item = new SiteDto();
                item.LoadFromEntity(i, Mapper);

                return item;
            });

            return Json(new PagedList<SiteDto>(source, list.PageIndex, list.PageSize, list.TotalCount));
        }

        [HttpGet("{id}/Facilities")]
        [SwaggerOperation(Summary = "Get Site Facility")]
        [SwaggerResponse(200, "Success: Site facility found")]
        public ActionResult<FacilityDto> GetFacilities(int id)
        {
            var list = Svc.GetFacilities(id);

            var source = list.Items.Select(i =>
            {
                var item = new FacilityDto();
                item.LoadFromEntity(i, Mapper);

                return item;
            });

            return Json(new PagedList<FacilityDto>(source, list.PageIndex, list.PageSize, list.TotalCount));
        }

        [Authorize]
        [HttpPost("{id}/AddFacility")]
        [SwaggerOperation(Summary = "Add Site Facility")]
        [SwaggerResponse(201, "Success: Site facility created")]
        [SwaggerResponse(404, "Error: Site not found")]
        public IActionResult AddFacility(int id, [FromBody] NewFacilityDto value)
        {
            var entity = Svc.GetById(id);
            if (entity == null)
                return NotFound();

            var facility = Mapper.Map<Model.Facility>(value);
            int facilityId = Svc.AddFacility(id, facility);

            var createdDto = new FacilityDto();
            createdDto.LoadFromEntity(facility, Mapper);

            return CreatedAtRoute("GetFacilityById", new {id = facilityId}, createdDto);
        }

        [Authorize]
        [HttpDelete("{id}/RemoveFacility/{facilityId}")]
        [SwaggerOperation(Summary = "Delete Site Facility")]
        [SwaggerResponse(204, "Success: Site facility deleted")]
        public IActionResult RemoveFacility(int id, int facilityId)
        {
            Svc.RemoveFacility(id, facilityId);

            return NoContent();
        }
    }
}
