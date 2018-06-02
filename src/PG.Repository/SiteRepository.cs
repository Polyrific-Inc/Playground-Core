// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PG.DataAccess;
using PG.Model;

namespace PG.Repository
{
    public class SiteRepository : BaseRepository<Site>, ISiteRepository
    {
        public SiteRepository(PlaygroundDbContext dbContext, IDistributedCache cache, ILogger<SiteRepository> logger)
            : base(dbContext, cache, logger)
        {

        }
    }
}