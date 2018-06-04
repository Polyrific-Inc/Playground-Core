// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PG.Common.Extensions;
using PG.DataAccess;
using PG.Model;

namespace PG.Repository
{
    public class UserProfileRepository : BaseRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(PlaygroundDbContext dbContext, IDistributedCache cache, ILogger<UserProfileRepository> logger) : base(dbContext, cache, logger)
        {
        }

        public UserProfile GetByUserName(string userName)
        {
            UserProfile entity;

            string cachedEntity = null;
            try
            {
                cachedEntity = Cache?.GetString($"{SingleCacheKeyPrefix}:{userName}");
            }
            catch (System.Exception ex)
            {
                Logger.LogWarning(ex.GetLastInnerException(), "Failed to get entity from cache");
            }

            if (string.IsNullOrEmpty(cachedEntity))
            {
                entity = Db.Set<UserProfile>().Include(e => e.AppUser)
                    .FirstOrDefault(e => e.AppUser.UserName == userName);

                try
                {
                    Cache?.SetString($"{SingleCacheKeyPrefix}:{userName}", JsonConvert.SerializeObject(entity));
                }
                catch (System.Exception ex)
                {
                    Logger.LogWarning(ex.GetLastInnerException(), "Failed to store existing entity to cache");
                }
            }
            else
            {
                entity = JsonConvert.DeserializeObject<UserProfile>(cachedEntity);
            }

            return entity;
        }
    }
}