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
                    Name = "Facility 1"
                }
            };
            
            _facilityRepository = new Mock<IFacilityRepository>();
            _logger = new Mock<ILogger<FacilityService>>();

            SetupRepository();
        }

        private void SetupRepository()
        {
            _facilityRepository.Setup(r => r.Get(It.IsAny<int>(), f => f.Site)).Returns((int id, Expression<Func<Facility, object>>[] includeProperties) =>
            {
                var item = _data.FirstOrDefault(d => d.Id == id);
                if (item != null)
                    item.Site = new Site();

                return item;
            });

            _facilityRepository.Setup(r => r.Create(It.IsAny<Facility>())).Returns(2).Callback((Facility item) =>
            {
                item.Id = 2;
                _data.Add(item);
            });

            _facilityRepository.Setup(r => r.Update(It.IsAny<Facility>())).Callback((Facility item) =>
            {
                var oldItem = _data.FirstOrDefault(d => d.Id == item.Id);
                if (oldItem != null)
                {
                    _data.Remove(oldItem);
                    _data.Add(item);
                }
            });

            _facilityRepository.Setup(d => d.Delete(It.IsAny<int>())).Callback((int id) =>
            {
                var deleteddata = _data.FirstOrDefault(a => a.Id == id);
                if (deleteddata != null)
                    _data.Remove(deleteddata);
            });
        }

        [Theory]
        [InlineData(1)]
        public void GetById_ReturnItemWithSite(int id)
        {
            //act
            var service = new FacilityService(_facilityRepository.Object, _logger.Object);
            var item = service.GetById(id);

            //assert
            Assert.NotNull(item);
            Assert.NotNull(item.Site);
            Assert.Equal(id, item.Id);
        }

        [Theory]
        [InlineData(2)]
        public void GetById_ReturnNull(int id)
        {
            //act
            var service = new FacilityService(_facilityRepository.Object, _logger.Object);
            var item = service.GetById(id);

            //assert
            Assert.Null(item);
        }

        [Theory]
        [InlineData(2)]
        public void Create_ValidModel_ReturnId(int newId)
        {
            //arrange
            var newItem = new Facility();

            //act
            var service = new FacilityService(_facilityRepository.Object, _logger.Object);
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
            var updatedItem = new Facility
            {
                Id = id,
                Name = $"Facility {id} (edited)"
            };

            var currentTime = DateTime.UtcNow;

            //act
            var service = new FacilityService(_facilityRepository.Object, _logger.Object);
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
            var service = new FacilityService(_facilityRepository.Object, _logger.Object);
            service.Delete(id);

            //assert
            Assert.DoesNotContain(_data, d => d.Id == id);
        }
    }
}
