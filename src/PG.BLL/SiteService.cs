using System;
using Microsoft.Extensions.Logging;
using PG.Common;
using PG.Model;
using PG.Repository;

namespace PG.BLL
{
    public class SiteService : BaseService<Site, ISiteRepository>, ISiteService
    {
        private readonly IFacilityRepository _facilityRepository;

        public SiteService(ISiteRepository siteRepository, IFacilityRepository facilityRepository, ILogger<SiteService> logger)
            : base(siteRepository, logger)
        {
            _facilityRepository = facilityRepository;
        }
        
        public PagedList<Site> GetByName(string name, int pageIndex = 1, int pageSize = 20)
        {
            return Repo.Filter(pageIndex, pageSize,
                new OrderBySelector<Site, string>(OrderByType.Ascending, site => site.Name),
                site => site.Name.Contains(name));
        }

        public PagedList<Facility> GetFacilities(int siteId, int pageIndex = 1, int pageSize = 20)
        {
            return _facilityRepository.Filter(pageIndex, pageSize,
                new OrderBySelector<Facility, string>(OrderByType.Ascending, facility => facility.Name),
                facility => facility.SiteId == siteId,
                facility => facility.Site);
        }

        public int AddFacility(int siteId, Facility newFacility)
        {
            newFacility.SiteId = siteId;
            newFacility.Created = DateTime.UtcNow;

            return _facilityRepository.Create(newFacility);
        }

        public void RemoveFacility(int siteId, int facilityId)
        {
            var facility = _facilityRepository.Get(facilityId);
            if (facility != null && facility.SiteId == siteId)
            {
                _facilityRepository.Delete(facilityId);
            }
        }
    }
}
