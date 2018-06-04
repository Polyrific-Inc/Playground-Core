// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace PG.Api.Domains.UserProfile
{
    public class UserProfileDto : EditUserProfileDto
    {
        public string Email { get; set; }

        public override void LoadFromEntity(Model.UserProfile entity)
        {
            base.LoadFromEntity(entity);

            FirstName = entity.FirstName;
            LastName = entity.LastName;

            if (entity.AppUser != null)
            {
                Email = entity.AppUser.Email;
            }
        }
    }
}