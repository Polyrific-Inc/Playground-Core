// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using PG.Api.Domains.UserProfile;
using PG.Model;

namespace PG.Api.AutoMapperProfiles
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<EditUserProfileDto, UserProfile>();
            CreateMap<UserProfileDto, UserProfile>().ReverseMap();
        }
    }
}