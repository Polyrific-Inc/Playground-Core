// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;

namespace PG.Api.Domains.Site
{
    public class SiteAutoMapperProfile : Profile
    {
        public SiteAutoMapperProfile()
        {
            CreateMap<NewSiteDto, Model.Site>();
            CreateMap<EditSiteDto, Model.Site>();
            CreateMap<SiteDto, Model.Site>().ReverseMap();
        }
    }
}