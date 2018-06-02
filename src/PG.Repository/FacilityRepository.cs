// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PG.DataAccess;
using PG.Model;

namespace PG.Repository
{
    public class FacilityRepository : BaseRepository<Facility>, IFacilityRepository
    {
        public FacilityRepository(PlaygroundDbContext dbContext, IDistributedCache cache, ILogger<FacilityRepository> logger) 
            : base(dbContext, cache, logger)
        {
        }
    }
}