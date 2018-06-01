using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PG.Api.Domains.Base;
using PG.Api.Domains.Facility;
using PG.BLL;
using PG.Common;
using System.Linq;

namespace PG.Api.Domains.Site
{
    [Route("Site")]
    public class SiteController : BaseController<NewSiteDto, EditSiteDto, SiteDto, Model.Site, ISiteService>
    {
        public SiteController(ISiteService siteService)
            : base(siteService)
        {
            
        }

        [Route("{id}", Name = "GetSiteById")]
        public override ActionResult<SiteDto> Get(int id)
        {
            return base.Get(id);
        }

        [Authorize]
        [Route("")]
        public IActionResult Post(NewSiteDto value)
        {
            return base.Post(value, "GetSiteById");
        }

        [Authorize]
        [Route("{id}")]
        public override ActionResult<SiteDto> Put(int id, [FromBody] EditSiteDto value)
        {
            return base.Put(id, value);
        }

        [Authorize]
        [Route("{id}")]
        public override IActionResult Delete(int id)
        {
            return base.Delete(id);
        }

        [Route("n/{name}", Name = "GetSiteByName")]
        public ActionResult<SiteDto> Get(string name)
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

        [Authorize]
        [HttpPost("{id}/AddFacility")]
        public IActionResult AddFacility(int id, NewFacilityDto value)
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

        [Authorize]
        [HttpDelete("{id}/RemoveFacility/{facilityId}")]
        public IActionResult RemoveFacility(int id, int facilityId)
        {
            Svc.RemoveFacility(id, facilityId);

            return NoContent();
        }
    }
}
