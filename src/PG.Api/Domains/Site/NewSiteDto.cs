// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using PG.Api.Domains.Base;

namespace PG.Api.Domains.Site
{
    public class NewSiteDto : BaseNewDto<Model.Site>
    {
        public string Name { get; set; }
    }
}