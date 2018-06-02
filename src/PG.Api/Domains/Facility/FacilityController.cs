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

        [HttpGet("{id}", Name = "GetFacilityById")]
        public override ActionResult<FacilityDto> Get(int id)
        {
            return base.Get(id);
        }

        [Authorize]
        [HttpPost("")]
        public IActionResult Post([FromBody] NewFacilityDto value)
        {
            return base.Post(value, "GetFacilityById");
        }

        [Authorize]
        [HttpPut("{id}")]
        public override ActionResult<FacilityDto> Put(int id, [FromBody] EditFacilityDto value)
        {
            return base.Put(id, value);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public override IActionResult Delete(int id)
        {
            return base.Delete(id);
        }
    }
}