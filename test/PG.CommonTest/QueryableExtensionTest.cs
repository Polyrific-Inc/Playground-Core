using System;
using System.Collections.Generic;
using System.Linq;
using PG.Common.Extensions;
using Xunit;

namespace PG.CommonTest
{
    public class QueryableExtensionTest
    {
        private readonly IQueryable<string> _data;

        public QueryableExtensionTest()
        {
            _data = new List<string>
            {
                "Apple",
                "Banana",
                "Dragon Fruit",
                "Grape",
                "Papaya",
                "Water Melon"
            }.AsQueryable();
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 3)]
        [InlineData(3, 3)]
        public void ToPagedList(int pageIndex, int pageSize)
        {
            var pagedList = _data.ToPagedList(pageIndex, pageSize);
            var maxPageIndex = Math.Ceiling((double) _data.Count() / pageSize);

            Assert.True(pagedList.PageIndex > 0, "Page index should start from 1");
            Assert.True(pagedList.PageIndex <= maxPageIndex, $"Page index should not be greater than the max index ({maxPageIndex})");
            Assert.True(pagedList.Items.Count <= pageSize, $"Item count ({pagedList.Items.Count}) should not be greater than the page size ({pageSize})");
            Assert.True(pagedList.TotalCount == _data.Count(), $"Total count ({pagedList.TotalCount}) should equal with the whole data count ({_data.Count()})");
        }
    }
}
