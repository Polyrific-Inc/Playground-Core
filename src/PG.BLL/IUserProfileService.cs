// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using PG.Model;

namespace PG.BLL
{
    public interface IUserProfileService : IService<UserProfile>
    {
        UserProfile GetByUserName(string userName);
    }
}