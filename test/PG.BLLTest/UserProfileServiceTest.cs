// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using Moq;
using PG.BLL;
using PG.Model;
using PG.Model.Identity;
using PG.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace PG.BLLTest
{
    public class UserProfileServiceTest
    {
        private readonly List<UserProfile> _data;
        private readonly Mock<IUserProfileRepository> _userProfileRepository;
        private readonly Mock<ILogger<UserProfileService>> _logger;

        public UserProfileServiceTest()
        {
            _data = new List<UserProfile>
            {
                new UserProfile
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe"
                }
            };
            
            _userProfileRepository = new Mock<IUserProfileRepository>();
            _logger = new Mock<ILogger<UserProfileService>>();

            SetupRepository();
        }

        private void SetupRepository()
        {
            _userProfileRepository.Setup(r => r.Get(It.IsAny<int>(), u => u.AppUser)).Returns((int id, Expression<Func<UserProfile, object>>[] includeProperties) =>
            {
                var item = _data.FirstOrDefault(d => d.Id == id);
                if (item != null)
                    item.AppUser = new ApplicationUser();

                return item;
            });

            _userProfileRepository.Setup(r => r.Create(It.IsAny<UserProfile>())).Returns(2).Callback((UserProfile item) =>
            {
                item.Id = 2;
                _data.Add(item);
            });

            _userProfileRepository.Setup(r => r.Update(It.IsAny<UserProfile>())).Callback((UserProfile item) =>
            {
                var oldItem = _data.FirstOrDefault(d => d.Id == item.Id);
                if (oldItem != null)
                {
                    _data.Remove(oldItem);
                    _data.Add(item);
                }
            });

            _userProfileRepository.Setup(d => d.Delete(It.IsAny<int>())).Callback((int id) =>
            {
                var deleteddata = _data.FirstOrDefault(a => a.Id == id);
                if (deleteddata != null)
                    _data.Remove(deleteddata);
            });
        }

        [Theory]
        [InlineData(1)]
        public void GetById_ReturnItemWithAppUser(int id)
        {
            //act
            var service = new UserProfileService(_userProfileRepository.Object, _logger.Object);
            var item = service.GetById(id);

            //assert
            Assert.NotNull(item);
            Assert.NotNull(item.AppUser);
            Assert.Equal(id, item.Id);
        }

        [Theory]
        [InlineData(2)]
        public void GetById_ReturnNull(int id)
        {
            //act
            var service = new UserProfileService(_userProfileRepository.Object, _logger.Object);
            var item = service.GetById(id);

            //assert
            Assert.Null(item);
        }

        [Theory]
        [InlineData(2)]
        public void Create_ValidModel_ReturnId(int newId)
        {
            //arrange
            var newItem = new UserProfile();

            //act
            var service = new UserProfileService(_userProfileRepository.Object, _logger.Object);
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
            var updatedItem = new UserProfile
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe Jr."
            };

            var currentTime = DateTime.UtcNow;

            //act
            var service = new UserProfileService(_userProfileRepository.Object, _logger.Object);
            service.Update(updatedItem);

            var updatedData = _data.FirstOrDefault(d => d.Id == id);

            //assert
            if (updatedData != null)
            {
                Assert.Equal(updatedData.LastName, updatedItem.LastName);
                Assert.True(updatedData.Updated >= currentTime);
            }
        }

        [Theory]
        [InlineData(1)]
        public void Delete_ValidId_DataDeleted(int id)
        {
            //act
            var service = new UserProfileService(_userProfileRepository.Object, _logger.Object);
            service.Delete(id);

            //assert
            Assert.DoesNotContain(_data, d => d.Id == id);
        }
    }
}
