// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using Moq;
using PG.BLL;
using PG.Model;
using PG.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PG.Common;
using Xunit;

namespace PG.BLLTest
{
    public class SiteServiceTest
    {
        private readonly List<Site> _siteData;
        private readonly List<Facility> _facilityData;
        private readonly Mock<ISiteRepository> _siteRepository;
        private readonly Mock<IFacilityRepository> _facilityRepository;
        private readonly Mock<ILogger<SiteService>> _logger;
        
        public SiteServiceTest()
        {
            _siteData = new List<Site>
            {
                new Site
                {
                    Id = 1,
                    Name = "Site 1"
                }
            };

            _facilityData = new List<Facility>
            {
                new Facility
                {
                    Id = 1,
                    Name = "Facility 1",
                    SiteId = 1
                }
            };

            _siteRepository = new Mock<ISiteRepository>();
            _facilityRepository = new Mock<IFacilityRepository>();
            _logger = new Mock<ILogger<SiteService>>();

            SetupRepository();
        }

        private void SetupRepository()
        {
            _siteRepository.Setup(r => r.Get(It.IsAny<int>())).Returns((int id) =>
            {
                return _siteData.FirstOrDefault(d => d.Id == id);
            });

            _siteRepository.Setup(r => r.Create(It.IsAny<Site>())).Returns(2).Callback((Site item) =>
            {
                item.Id = 2;
                _siteData.Add(item);
            });

            _siteRepository.Setup(r => r.Update(It.IsAny<Site>())).Callback((Site item) =>
            {
                var oldItem = _siteData.FirstOrDefault(d => d.Id == item.Id);
                if (oldItem != null)
                {
                    _siteData.Remove(oldItem);
                    _siteData.Add(item);
                }
            });

            _siteRepository.Setup(d => d.Delete(It.IsAny<int>())).Callback((int id) =>
            {
                var deleteddata = _siteData.FirstOrDefault(a => a.Id == id);
                if (deleteddata != null)
                    _siteData.Remove(deleteddata);
            });

            _siteRepository
                .Setup(r => r.Filter(It.IsAny<int>(), It.IsAny<int>(),
                    It.IsAny<OrderBySelector<Site, string>>(), It.IsAny<Expression<Func<Site, bool>>>(),
                    It.IsAny<Expression<Func<Site, object>>[]>()))
                .Returns((int pageIndex, int pageSize, 
                    OrderBySelector<Site, string> orderBySelector, Expression<Func<Site, bool>> whereFilter, 
                    Expression<Func<Site, object>>[] includeProperties) =>
                {
                    var items = _siteData.Where(whereFilter.Compile()).ToList();
                    var totalCount = items.Count;

                    var source = totalCount > pageSize
                        ? items.OrderBy(orderBySelector.Selector.Compile()).Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                        : items;
                    
                    return new PagedList<Site>(source, pageIndex, pageSize, totalCount);
                });

            _facilityRepository.Setup(r => r.Get(It.IsAny<int>(), f => f.Site)).Returns((int id, Expression<Func<Facility, object>>[] includeProperties) =>
            {
                var item = _facilityData.FirstOrDefault(d => d.Id == id);
                if (item != null)
                    item.Site = new Site();

                return item;
            });

            _facilityRepository
                .Setup(r => r.Filter(It.IsAny<int>(), It.IsAny<int>(),
                    It.IsAny<OrderBySelector<Facility, string>>(), It.IsAny<Expression<Func<Facility, bool>>>(),
                    It.IsAny<Expression<Func<Facility, object>>[]>()))
                .Returns((int pageIndex, int pageSize, 
                    OrderBySelector<Facility, string> orderBySelector, Expression<Func<Facility, bool>> whereFilter, 
                    Expression<Func<Facility, object>>[] includeProperties) =>
                {
                    var items = _facilityData.Where(whereFilter.Compile()).ToList();
                    var totalCount = items.Count;

                    var source = totalCount > pageSize
                        ? items.OrderBy(orderBySelector.Selector.Compile()).Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                        : items;
                    
                    return new PagedList<Facility>(source, pageIndex, pageSize, totalCount);
                });

            _facilityRepository.Setup(r => r.Create(It.IsAny<Facility>())).Returns(2).Callback((Facility item) =>
            {
                item.Id = 2;
                _facilityData.Add(item);
            });

            _facilityRepository.Setup(d => d.Delete(It.IsAny<int>())).Callback((int id) =>
            {
                var deleteddata = _facilityData.FirstOrDefault(a => a.Id == id);
                if (deleteddata != null)
                    _facilityData.Remove(deleteddata);
            });
        }

        [Theory]
        [InlineData(1)]
        public void GetById_ReturnItem(int id)
        {
            //act
            var service = new SiteService(_siteRepository.Object, _facilityRepository.Object, _logger.Object);
            var item = service.GetById(id);

            //assert
            Assert.NotNull(item);
            Assert.Equal(id, item.Id);
        }

        [Theory]
        [InlineData(2)]
        public void GetById_ReturnNull(int id)
        {
            //act
            var service = new SiteService(_siteRepository.Object, _facilityRepository.Object, _logger.Object);
            var item = service.GetById(id);

            //assert
            Assert.Null(item);
        }

        [Theory]
        [InlineData(2)]
        public void Create_ValidModel_ReturnId(int newId)
        {
            //arrange
            var newItem = new Site();

            //act
            var service = new SiteService(_siteRepository.Object, _facilityRepository.Object, _logger.Object);
            var returnId = service.Create(newItem);

            //assert
            Assert.Equal(newId, returnId);
            Assert.NotEqual(0, newItem.Id);
            Assert.NotEqual(DateTime.MinValue, newItem.Created);
            Assert.True(_siteData.Count == 2);
        }

        [Theory]
        [InlineData(1)]
        public void Update_ValidModel(int id)
        {
            //arrange
            var updatedItem = new Site()
            {
                Id = id,
                Name = $"Site {id} (edited)"
            };

            var currentTime = DateTime.UtcNow;

            //act
            var service = new SiteService(_siteRepository.Object, _facilityRepository.Object, _logger.Object);
            service.Update(updatedItem);

            var updatedData = _siteData.FirstOrDefault(d => d.Id == id);

            //assert
            if (updatedData != null)
            {
                Assert.Equal(updatedData.Name, updatedItem.Name);
                Assert.True(updatedData.Updated >= currentTime);
            }
        }

        [Theory]
        [InlineData(1)]
        public void Delete_ValidId_DataDeleted(int id)
        {
            //act
            var service = new SiteService(_siteRepository.Object, _facilityRepository.Object, _logger.Object);
            service.Delete(id);

            //assert
            Assert.DoesNotContain(_siteData, d => d.Id == id);
        }

        [Theory]
        [InlineData("Site 1")]
        public void GetByName_ReturnItems(string name)
        {
            //act
            var service = new SiteService(_siteRepository.Object, _facilityRepository.Object, _logger.Object);
            var items = service.GetByName(name);

            //assert
            Assert.Single(items.Items);
            Assert.True(items.Items.TrueForAll(i => i.Name.Contains(name)));
        }

        [Theory]
        [InlineData("Site 2")]
        public void GetByName_ReturnEmpty(string name)
        {
            //act
            var service = new SiteService(_siteRepository.Object, _facilityRepository.Object, _logger.Object);
            var items = service.GetByName(name);

            //assert
            Assert.Empty(items.Items);
        }

        [Theory]
        [InlineData(1)]
        public void GetFacilities_ReturnItems(int siteId)
        {
            //act
            var service = new SiteService(_siteRepository.Object, _facilityRepository.Object, _logger.Object);
            var items = service.GetFacilities(siteId);

            //assert
            Assert.Single(items.Items);
            Assert.True(items.Items.TrueForAll(i => i.SiteId == siteId));
        }

        [Theory]
        [InlineData(2)]
        public void GetFacilities_ReturnEmpty(int siteId)
        {
            //act
            var service = new SiteService(_siteRepository.Object, _facilityRepository.Object, _logger.Object);
            var items = service.GetFacilities(siteId);

            //assert
            Assert.Empty(items.Items);
        }

        [Theory]
        [InlineData(1, 2)]
        public void AddFacility_ValidModel_ReturnId(int siteId, int facilityId)
        {
            var newItem = new Facility
            {
                Id = facilityId,
                SiteId = siteId
            };

            //act
            var service = new SiteService(_siteRepository.Object, _facilityRepository.Object, _logger.Object);
            var returnId = service.AddFacility(siteId, newItem);

            //assert
            Assert.Equal(facilityId, returnId);
            Assert.NotEqual(0, newItem.Id);
            Assert.NotEqual(DateTime.MinValue, newItem.Created);
            Assert.True(_facilityData.Count == 2);
        }

        [Theory]
        [InlineData(1, 2)]
        public void RemoveFacility_ValidModel(int siteId, int facilityId)
        {
            //act
            var service = new SiteService(_siteRepository.Object, _facilityRepository.Object, _logger.Object);
            service.RemoveFacility(siteId, facilityId);

            //assert
            Assert.DoesNotContain(_facilityData, d => d.Id == facilityId && d.SiteId == siteId);
        }
    }
}
