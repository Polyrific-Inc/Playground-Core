// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;

namespace PG.Api.Domains.UserProfile
{
    public class UserProfileDto : EditUserProfileDto
    {
        public string Email { get; set; }

        public override void LoadFromEntity(Model.UserProfile entity, IMapper mapper)
        {
            base.LoadFromEntity(entity, mapper);

            FirstName = entity.FirstName;
            LastName = entity.LastName;

            if (entity.AppUser != null)
            {
                Email = entity.AppUser.Email;
            }
        }
    }
}