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
    public class SiteRepositoryTest
    {
        private readonly PlaygroundDbContext _dbContext;
        private readonly Mock<IDistributedCache> _cache;
        private readonly Mock<ILogger<SiteRepository>> _logger;

        public SiteRepositoryTest()
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
            
            _dbContext.SaveChanges();

            _cache = new Mock<IDistributedCache>();
            _logger = new Mock<ILogger<SiteRepository>>();
        }

        [Theory]
        [InlineData(1)]
        public void Get_ReturnItem(int id)
        {
            var repository = new SiteRepository(_dbContext, _cache.Object, _logger.Object);
            var item = repository.Get(id);

            Assert.NotNull(item);
            Assert.Equal(id, item.Id);
        }

        [Theory]
        [InlineData(2)]
        public void Get_ReturnNull(int id)
        {
            var repository = new SiteRepository(_dbContext, _cache.Object, _logger.Object);
            var item = repository.Get(id);

            Assert.Null(item);
        }

        [Theory]
        [InlineData("Site")]
        public void Filter(string nameFilter)
        {
            var repository = new SiteRepository(_dbContext, _cache.Object, _logger.Object);
            var pagedItems = repository.Filter(1, 20, new OrderBySelector<Site, string>(OrderByType.Ascending, f => f.Name),
                f => f.Name.Contains(nameFilter));

            Assert.NotEmpty(pagedItems.Items);
            Assert.True(pagedItems.Items.TrueForAll(f => f.Name.Contains(nameFilter)));
        }

        [Theory]
        [InlineData(2)]
        public void Create(int newId)
        {
            var newItem = new Site
            {
                Id = newId,
                Name = $"Site {newId}"
            };

            var repository = new SiteRepository(_dbContext, _cache.Object, _logger.Object);
            var returnId = repository.Create(newItem);
            
            var item = _dbContext.Sites.Find(returnId);

            Assert.Equal(newId, returnId);
            Assert.NotNull(item);
            Assert.Equal(newItem.Name, item.Name);
        }

        [Theory]
        [InlineData(1)]
        public void Update(int id)
        {
            var updatedItem = _dbContext.Sites.Find(id);
            updatedItem.Name += " (edited)";
            
            var repository = new SiteRepository(_dbContext, _cache.Object, _logger.Object);
            repository.Update(updatedItem);

            var testedItem = _dbContext.Sites.Find(id);

            Assert.Equal(updatedItem.Name, testedItem.Name);
        }

        [Theory]
        [InlineData(1)]
        public void Delete(int id)
        {
            var repository = new SiteRepository(_dbContext, _cache.Object, _logger.Object);
            repository.Delete(id);

            var testedItem = _dbContext.Sites.Find(id);

            Assert.Null(testedItem);
        }
    }
}