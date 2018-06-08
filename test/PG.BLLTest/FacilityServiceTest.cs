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
    public class FacilityServiceTest
    {
        private readonly List<Facility> _data;
        private readonly Mock<IFacilityRepository> _facilityRepository;
        private readonly Mock<ILogger<FacilityService>> _logger;

        public FacilityServiceTest()
        {
            _data = new List<Facility>
            {
                new Facility
                {
                    Id = 1,
                    Name = "Facitlity 1"
                }
            };

            _facilityRepository = new Mock<IFacilityRepository>();
            _logger = new Mock<ILogger<FacilityService>>();
        }

        [Theory]
        [InlineData(2)]
        public void Create_ValidModel_ReturnFacilityId(int newId)
        {
            //arrange
            _facilityRepository.Setup(a => a.Create(It.IsAny<Facility>())).Returns(newId).Callback((Facility c) =>
            {
                c.Id = newId;
                _data.Add(c);
            });
            var model = new Facility();

            //act
            var service = new FacilityService(_facilityRepository.Object, _logger.Object);
            var returnId = service.Create(model);

            //assert
            Assert.Equal(newId, returnId);
            Assert.NotEqual(0, model.Id);
            Assert.NotEqual(DateTime.MinValue, model.Created);
            Assert.True(_data.Count == 2);
        }

        [Theory]
        [InlineData(1)]
        public void Update_ValidModel_ReturnFacilityId(int id)
        {
            //arrange
            _facilityRepository.Setup(a => a.Update(It.IsAny<Facility>())).Callback((Facility c) =>
            {
                var getData = _data.FirstOrDefault(a => a.Id == id);
                if (getData != null)
                {
                    _data.Remove(getData);
                    _data.Add(c);
                }
            });

            var model = new Facility()
            {
                Id = id,
                Name = $"Facitlity {id} (edited)"
            };

            var currentTime = DateTime.UtcNow;

            //act
            var service = new FacilityService(_facilityRepository.Object, _logger.Object);
            service.Update(model);
            var updatedData = _data.First(a => a.Id == id);

            //assert
            Assert.NotNull(updatedData);
            Assert.Equal(updatedData.Name, model.Name);
            Assert.True(updatedData.Updated >= currentTime);
        }

        [Theory]
        [InlineData(1)]
        public void Delete_ValidId_DataDeleted(int id)
        {
            //arrange
            _facilityRepository.Setup(a => a.Delete(id)).Callback(() =>
            {
                var deleteddata = _data.First(a => a.Id == id);
                _data.Remove(deleteddata);
            });

            //act
            var service = new FacilityService(_facilityRepository.Object, _logger.Object);
            service.Delete(id);

            //assert
            Assert.True(_data.Count == 0);
        }

        [Theory]
        [InlineData(1)]
        public void GetById_ReturnFacilityIncludingSite(int id)
        {
            //arrange
            _facilityRepository.Setup(a => a.Get(id, f => f.Site)).Returns(() =>
            {
                var getData = _data.FirstOrDefault(item => item.Id == id);
                if (getData != null)
                    getData.Site = new Site();

                return getData;
            });

            //act
            var service = new FacilityService(_facilityRepository.Object, _logger.Object);
            var model = service.GetById(id);

            //assert
            Assert.NotNull(model);
            Assert.NotNull(model.Site);
            Assert.Equal(id, model.Id);
        }

        [Theory]
        [InlineData(2)]
        public void GetById_ReturnNull(int id)
        {
            //arrange
            _facilityRepository.Setup(a => a.Get(id, f => f.Site)).Returns(() =>
            {
                var getData = _data.FirstOrDefault(item => item.Id == id);
                if (getData != null)
                    getData.Site = new Site();

                return getData;
            });

            //act
            var service = new FacilityService(_facilityRepository.Object, _logger.Object);
            var model = service.GetById(id);

            //assert
            Assert.Null(model);
        }
    }
}
