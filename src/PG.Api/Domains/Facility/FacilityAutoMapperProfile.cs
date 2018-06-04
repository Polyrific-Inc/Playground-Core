// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;

namespace PG.Api.Domains.Facility
{
    public class FacilityAutoMapperProfile : Profile
    {
        public FacilityAutoMapperProfile()
        {
            CreateMap<NewFacilityDto, Model.Facility>();
            CreateMap<EditFacilityDto, Model.Facility>();
            CreateMap<FacilityDto, Model.Facility>().ReverseMap();
        }
    }
}