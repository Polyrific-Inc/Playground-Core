// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using PG.Api.Domains.Site;

namespace PG.Api.Domains.Facility
{
    public class FacilityDto : EditFacilityDto
    {
        public SiteDto Site { get; set; }

        public override void LoadFromEntity(Model.Facility entity, IMapper mapper)
        {
            base.LoadFromEntity(entity, mapper);

            Name = entity.Name;
            Description = entity.Description;
            
            if (entity.Site != null)
                Site = mapper.Map<Model.Site, SiteDto>(entity.Site);

            //if (entity.Location != null)
            //    Location = DbGeography.FromText(entity.Location.AsText());
        }
    }
}