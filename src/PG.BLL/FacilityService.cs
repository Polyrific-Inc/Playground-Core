// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using PG.Model;
using PG.Repository;

namespace PG.BLL
{
    public class FacilityService : BaseService<Facility, IFacilityRepository>, IFacilityService
    {
        public FacilityService(IFacilityRepository repository, ILogger<FacilityService> logger) 
            : base(repository, logger)
        {
        }

        public override Facility GetById(int id)
        {
            return Repo.Get(id, facility => facility.Site);
        }
    }
}