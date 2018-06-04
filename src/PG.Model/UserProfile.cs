// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using PG.Model.Identity;

namespace PG.Model
{
    public class UserProfile : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int? AppUserId { get; set; }
        public ApplicationUser AppUser { get; set; }
    }
}