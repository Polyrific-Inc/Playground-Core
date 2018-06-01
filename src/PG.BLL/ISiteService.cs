// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using PG.Common;
using PG.Model;

namespace PG.BLL
{
    public interface ISiteService : IService<Site>
    {
        PagedList<Site> GetByName(string name, int pageIndex = 1, int pageSize = 20);
        PagedList<Facility> GetFacilities(int siteId, int pageIndex = 1, int pageSize = 20);
        int AddFacility(int siteId, Facility newFacility);
        void RemoveFacility(int siteId, int facilityId);
    }
}