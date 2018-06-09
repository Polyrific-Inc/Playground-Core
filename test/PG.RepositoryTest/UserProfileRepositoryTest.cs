// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using PG.DataAccess;
using PG.Model;
using PG.Model.Identity;
using PG.Repository;
using Xunit;

namespace PG.RepositoryTest
{
    public class UserProfileRepositoryTest
    {
        private readonly PlaygroundDbContext _dbContext;
        private readonly Mock<IDistributedCache> _cache;
        private readonly Mock<ILogger<UserProfileRepository>> _logger;

        public UserProfileRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<PlaygroundDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new PlaygroundDbContext(options);

            _dbContext.Users.Add(new ApplicationUser
            {
                Id = 1,
                Email = "john.doe@example.com"
            });

            _dbContext.UserProfiles.Add(new UserProfile
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                AppUserId = 1
            });
            
            _dbContext.SaveChanges();

            _cache = new Mock<IDistributedCache>();
            _logger = new Mock<ILogger<UserProfileRepository>>();
        }

        [Theory]
        [InlineData(1)]
        public void Get_ReturnItem(int id)
        {
            var repository = new UserProfileRepository(_dbContext, _cache.Object, _logger.Object);
            var item = repository.Get(id);

            Assert.NotNull(item);
            Assert.Equal(id, item.Id);
        }

        [Theory]
        [InlineData(1)]
        public void Get_IncludeProperties_ReturnItem(int id)
        {
            var repository = new UserProfileRepository(_dbContext, _cache.Object, _logger.Object);
            var item = repository.Get(id, u => u.AppUser);

            Assert.NotNull(item);
            Assert.NotNull(item.AppUser);
        }

        [Theory]
        [InlineData("Doe")]
        public void Filter(string nameFilter)
        {
            var repository = new UserProfileRepository(_dbContext, _cache.Object, _logger.Object);
            var pagedItems = repository.Filter(1, 20, new OrderBySelector<UserProfile, string>(OrderByType.Ascending, u => u.FirstName),
                f => f.FirstName.Contains(nameFilter) || f.LastName.Contains(nameFilter));

            Assert.NotEmpty(pagedItems.Items);
            Assert.True(pagedItems.Items.TrueForAll(f => f.FirstName.Contains(nameFilter) || f.LastName.Contains(nameFilter)));
        }

        [Theory]
        [InlineData(2)]
        public void Create(int newId)
        {
            var newItem = new UserProfile
            {
                Id = newId,
                FirstName = "Jane",
                LastName = "Doe"
            };

            var repository = new UserProfileRepository(_dbContext, _cache.Object, _logger.Object);
            var returnId = repository.Create(newItem);
            
            var item = _dbContext.UserProfiles.Find(returnId);

            Assert.Equal(newId, returnId);
            Assert.NotNull(item);
            Assert.Equal(newItem.FirstName, item.FirstName);
        }

        [Theory]
        [InlineData(1)]
        public void Update(int id)
        {
            var updatedItem = _dbContext.UserProfiles.Find(id);
            updatedItem.LastName += " Jr.";
            
            var repository = new UserProfileRepository(_dbContext, _cache.Object, _logger.Object);
            repository.Update(updatedItem);

            var testedItem = _dbContext.UserProfiles.Find(id);

            Assert.Equal(updatedItem.LastName, testedItem.LastName);
        }

        [Theory]
        [InlineData(1)]
        public void Delete(int id)
        {
            var repository = new UserProfileRepository(_dbContext, _cache.Object, _logger.Object);
            repository.Delete(id);

            var testedItem = _dbContext.UserProfiles.Find(id);

            Assert.Null(testedItem);
        }
    }
}