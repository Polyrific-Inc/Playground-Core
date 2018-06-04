// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using PG.Model;
using PG.Repository;

namespace PG.BLL
{
    public class UserProfileService : BaseService<UserProfile, IUserProfileRepository>, IUserProfileService
    {
        protected UserProfileService(IUserProfileRepository repository, ILogger logger) : base(repository, logger)
        {
        }

        public override UserProfile GetById(int id)
        {
            return Repo.Get(id, u => u.AppUser);
        }

        public UserProfile GetByUserName(string userName)
        {
            return Repo.GetByUserName(userName);
        }
    }
}