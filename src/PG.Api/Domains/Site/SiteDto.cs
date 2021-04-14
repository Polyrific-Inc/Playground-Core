// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;

namespace PG.Api.Domains.Site
{
    public class SiteDto : EditSiteDto
    {
        public override void LoadFromEntity(Model.Site entity, IMapper mapper)
        {
            base.LoadFromEntity(entity, mapper);

            Name = entity.Name;
        }
    }
}