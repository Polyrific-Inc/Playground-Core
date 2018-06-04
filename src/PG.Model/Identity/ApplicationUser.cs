// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Identity;

namespace PG.Model.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public bool IsActive { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}