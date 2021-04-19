// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PG.Api.Domains.Base;
using PG.BLL;
using Swashbuckle.AspNetCore.Annotations;

namespace PG.Api.Domains.Facility
{
    [Route("Facility")]
    public class FacilityController : BaseController<NewFacilityDto, EditFacilityDto, FacilityDto, Model.Facility, IFacilityService>
    {
        public FacilityController(IFacilityService service, ILogger<FacilityController> logger, IMapper mapper) 
            : base(service, logger, mapper)
        {
        }
        
        [HttpGet("{id}", Name = "GetFacilityById")]
        [SwaggerOperation(Summary = "Get Facility By ID")]
        [SwaggerResponse(200, "Success: Facility found")]
        [SwaggerResponse(404, "Error: Facility not found")]
        public override ActionResult<FacilityDto> Get(int id)
        {
            return base.Get(id);
        }

        [Authorize]
        [HttpPost("")]
        [SwaggerOperation(Summary = "Create New Facility")]
        [SwaggerResponse(201, "Success: Facility created")]
        public IActionResult Post([FromBody] NewFacilityDto value)
        {
            return base.Post(value, "GetFacilityById");
        }

        [Authorize]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update Existing Facility")]
        [SwaggerResponse(200, "Success: Facilty updated")]
        [SwaggerResponse(400, "Error: Facility not found")]
        public override ActionResult<FacilityDto> Put(int id, [FromBody] EditFacilityDto value)
        {
            return base.Put(id, value);
        }

        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete Facility")]
        [SwaggerResponse(204, "Success: Facility deleted")]
        public override IActionResult Delete(int id)
        {
            return base.Delete(id);
        }
    }
}