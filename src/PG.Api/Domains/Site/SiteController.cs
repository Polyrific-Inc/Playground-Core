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

namespace PG.Api.Domains.Site
{
    [Route("Site")]
    public class SiteController : BaseController<NewSiteDto, EditSiteDto, SiteDto, Model.Site, ISiteService>
    {
        private readonly IMapper _mapper;
        public SiteController(ISiteService siteService, ILogger<SiteController> logger, IMapper mapper)
            : base(siteService, logger, mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetSiteById")]
        public override ActionResult<SiteDto> Get(int id)
        {
            return base.Get(id);
        }

        //[Authorize]
        [HttpPost("")]
        public IActionResult Post([FromBody] NewSiteDto value)
        {
            return base.Post(value, "GetSiteById");
        }

        [Authorize]
        [HttpPut("{id}")]
        public override ActionResult<SiteDto> Put(int id, [FromBody] EditSiteDto value)
        {
            return base.Put(id, value);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public override IActionResult Delete(int id)
        {
            return base.Delete(id);
        }

        [HttpGet("n/{name}")]
        public ActionResult<SiteDto> GetSiteByName(string name)
        {
            var list = Svc.GetByName(name);

            var source = list.Items.Select(i =>
            {
                var item = new SiteDto();
                item.LoadFromEntity(i, _mapper);

                return item;
            });

            return Json(new PagedList<SiteDto>(source, list.PageIndex, list.PageSize, list.TotalCount));
        }

        [HttpGet("{id}/Facilities")]
        public ActionResult<FacilityDto> GetFacilities(int id)
        {
            var list = Svc.GetFacilities(id);

            var source = list.Items.Select(i =>
            {
                var item = new FacilityDto();
                item.LoadFromEntity(i, _mapper);

                return item;
            });

            return Json(new PagedList<FacilityDto>(source, list.PageIndex, list.PageSize, list.TotalCount));
        }

        [Authorize]
        [HttpPost("{id}/AddFacility")]
        public IActionResult AddFacility(int id, [FromBody] NewFacilityDto value)
        {
            var entity = Svc.GetById(id);
            if (entity == null)
                return NotFound();

            var facility = _mapper.Map<Model.Facility>(value);
            int facilityId = Svc.AddFacility(id, facility);

            var createdDto = new FacilityDto();
            createdDto.LoadFromEntity(facility, _mapper);

            return CreatedAtRoute("GetFacilityById", new {id = facilityId}, createdDto);
        }

        [Authorize]
        [HttpDelete("{id}/RemoveFacility/{facilityId}")]
        public IActionResult RemoveFacility(int id, int facilityId)
        {
            Svc.RemoveFacility(id, facilityId);

            return NoContent();
        }
    }
}
