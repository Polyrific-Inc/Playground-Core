// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PG.Api.Domains.Base;
using PG.BLL;

namespace PG.Api.Domains.Facility
{
    [Route("Facility")]
    public class FacilityController : BaseController<NewFacilityDto, EditFacilityDto, FacilityDto, Model.Facility, IFacilityService>
    {
        public FacilityController(IFacilityService service) : base(service)
        {
        }

        [Route("{id}", Name = "GetFacilityById")]
        public override ActionResult<FacilityDto> Get(int id)
        {
            return base.Get(id);
        }

        [Authorize]
        [Route("")]
        public IActionResult Post(NewFacilityDto value)
        {
            return base.Post(value, "GetFacilityById");
        }

        [Authorize]
        [Route("{id}")]
        public override ActionResult<FacilityDto> Put(int id, EditFacilityDto value)
        {
            return base.Put(id, value);
        }

        [Authorize]
        [Route("{id}")]
        public override IActionResult Delete(int id)
        {
            return base.Delete(id);
        }
    }
}