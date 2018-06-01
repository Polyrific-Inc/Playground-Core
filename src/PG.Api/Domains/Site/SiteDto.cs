// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace PG.Api.Domains.Site
{
    public class SiteDto : EditSiteDto
    {
        public override void LoadFromEntity(Model.Site entity)
        {
            base.LoadFromEntity(entity);

            Name = entity.Name;
        }
    }
}