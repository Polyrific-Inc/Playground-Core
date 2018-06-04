// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using PG.Api.Domains.Base;

namespace PG.Api.Domains.UserProfile
{
    public class EditUserProfileDto : BaseDto<Model.UserProfile>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}