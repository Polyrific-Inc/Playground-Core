// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;

namespace PG.Api.Domains.UserProfile
{
    public class UserProfileAutoMapperProfile : Profile
    {
        public UserProfileAutoMapperProfile()
        {
            CreateMap<EditUserProfileDto, Model.UserProfile>();
            CreateMap<UserProfileDto, Model.UserProfile>().ReverseMap();
        }
    }
}