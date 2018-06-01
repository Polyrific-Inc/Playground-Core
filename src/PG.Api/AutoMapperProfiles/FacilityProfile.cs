// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using PG.Api.Domains.Facility;
using PG.Model;

namespace PG.Api.AutoMapperProfiles
{
    public class FacilityProfile : Profile
    {
        public FacilityProfile()
        {
            CreateMap<NewFacilityDto, Facility>();
            CreateMap<EditFacilityDto, Facility>();
            CreateMap<FacilityDto, Facility>().ReverseMap();
        }
    }
}