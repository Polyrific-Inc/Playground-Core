// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using PG.Model;

namespace PG.Repository
{
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        UserProfile GetByUserName(string userName);
    }
}