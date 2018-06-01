// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using PG.Api.Domains.Base;

namespace PG.Api.Domains.Site
{
    public class EditSiteDto : BaseDto<Model.Site>
    {
        public string Name { get; set; }
    }
}