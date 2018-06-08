using Microsoft.Extensions.Logging;
using Moq;
using PG.BLL;
using PG.Model;
using PG.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PG.BLLTest
{
    public class SiteServiceTest
    {
        private readonly List<Site> _data;
        private readonly Mock<ISiteRepository> _siteRepository;
        private readonly Mock<IFacilityRepository> _facilityRepository;
        private readonly Mock<ILogger<SiteService>> _logger;
        
        public SiteServiceTest()
        {
            _data = new List<Site>
            {
                new Site
                {
                    Id = 1,
                    Name = "Site 1"
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
                return _data.FirstOrDefault(d => d.Id == id);
            });

            _siteRepository.Setup(r => r.Create(It.IsAny<Site>())).Returns(2).Callback((Site item) =>
            {
                item.Id = 2;
                _data.Add(item);
            });

            _siteRepository.Setup(r => r.Update(It.IsAny<Site>())).Callback((Site item) =>
            {
                var oldItem = _data.FirstOrDefault(d => d.Id == item.Id);
                if (oldItem != null)
                {
                    _data.Remove(oldItem);
                    _data.Add(item);
                }
            });

            _siteRepository.Setup(d => d.Delete(It.IsAny<int>())).Callback((int id) =>
            {
                var deleteddata = _data.FirstOrDefault(a => a.Id == id);
                if (deleteddata != null)
                    _data.Remove(deleteddata);
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
            Assert.True(_data.Count == 2);
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

            var updatedData = _data.FirstOrDefault(d => d.Id == id);

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
            Assert.DoesNotContain(_data, d => d.Id == id);
        }
    }
}
