// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using PG.Api.Domains.Site;
using PG.Model;

namespace PG.Api.AutoMapperProfiles
{
    public class SiteProfile : Profile
    {
        public SiteProfile()
        {
            CreateMap<NewSiteDto, Site>();
            CreateMap<EditSiteDto, Site>();
            CreateMap<SiteDto, Site>().ReverseMap();
        }
    }
}