// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Linq;

namespace PG.Common.Extensions
{
    public static class QueryableExtension
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            var totalCount = query.Count();
            var maxPageIndex = Math.Ceiling((double) totalCount / pageSize);
            
            if (pageIndex < 1)
                pageIndex = 1;

            if (pageIndex > maxPageIndex)
                pageIndex = (int)maxPageIndex;

            var source = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return new PagedList<T>(source, pageIndex, pageSize, totalCount);
        }
    }
}
