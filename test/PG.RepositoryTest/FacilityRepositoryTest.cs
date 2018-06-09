// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using PG.DataAccess;
using PG.Model;
using PG.Repository;
using Xunit;

namespace PG.RepositoryTest
{
    public class FacilityRepositoryTest
    {
        private readonly PlaygroundDbContext _dbContext;
        private readonly Mock<IDistributedCache> _cache;
        private readonly Mock<ILogger<FacilityRepository>> _logger;

        public FacilityRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<PlaygroundDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new PlaygroundDbContext(options);

            _dbContext.Sites.Add(new Site
            {
                Id = 1,
                Name = "Site 1"
            });

            _dbContext.Facilities.Add(new Facility
            {
                Id = 1,
                Name = "Facility 1",
                SiteId = 1
            });

            _dbContext.SaveChanges();

            _cache = new Mock<IDistributedCache>();
            _logger = new Mock<ILogger<FacilityRepository>>();
        }

        [Theory]
        [InlineData(1)]
        public void Get_ReturnItem(int id)
        {
            var repository = new FacilityRepository(_dbContext, _cache.Object, _logger.Object);
            var item = repository.Get(id);

            Assert.NotNull(item);
            Assert.Equal(id, item.Id);
        }

        [Theory]
        [InlineData(1)]
        public void Get_IncludeProperties_ReturnItem(int id)
        {
            var repository = new FacilityRepository(_dbContext, _cache.Object, _logger.Object);
            var item = repository.Get(id);

            Assert.NotNull(item);
            Assert.NotNull(item.Site);
        }

        [Theory]
        [InlineData("Facility")]
        public void Filter(string nameFilter)
        {
            var repository = new FacilityRepository(_dbContext, _cache.Object, _logger.Object);
            var pagedItems = repository.Filter(1, 20, new OrderBySelector<Facility, string>(OrderByType.Ascending, f => f.Name),
                f => f.Name.Contains(nameFilter));

            Assert.NotEmpty(pagedItems.Items);
            Assert.True(pagedItems.Items.TrueForAll(f => f.Name.Contains(nameFilter)));
        }

        [Theory]
        [InlineData(2)]
        public void Create(int newId)
        {
            var newItem = new Facility
            {
                Id = newId,
                Name = $"Facility {newId}"
            };

            var repository = new FacilityRepository(_dbContext, _cache.Object, _logger.Object);
            var returnId = repository.Create(newItem);
            
            var item = _dbContext.Facilities.Find(returnId);

            Assert.Equal(newId, returnId);
            Assert.NotNull(item);
            Assert.Equal(newItem.Name, item.Name);
        }

        [Theory]
        [InlineData(1)]
        public void Update(int id)
        {
            var updatedItem = _dbContext.Facilities.Find(id);
            updatedItem.Name += " (edited)";
            
            var repository = new FacilityRepository(_dbContext, _cache.Object, _logger.Object);
            repository.Update(updatedItem);

            var testedItem = _dbContext.Facilities.Find(id);

            Assert.Equal(updatedItem.Name, testedItem.Name);
        }

        [Theory]
        [InlineData(1)]
        public void Delete(int id)
        {
            var repository = new FacilityRepository(_dbContext, _cache.Object, _logger.Object);
            repository.Delete(id);

            var testedItem = _dbContext.Facilities.Find(id);

            Assert.Null(testedItem);
        }
    }
    
}
