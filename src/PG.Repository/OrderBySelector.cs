// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Linq.Expressions;
using PG.Model;

namespace PG.Repository
{
    public class OrderBySelector<TEntity, TKey>
        where TEntity : BaseModel
    {
        public OrderBySelector(OrderByType type, Expression<Func<TEntity, TKey>> selector)
        {
            Type = type;
            Selector = selector;
        }

        public OrderByType Type { get; set; }
        public Expression<Func<TEntity, TKey>> Selector { get; set; }
    }

    public enum OrderByType
    {
        Ascending,
        Descending
    }
}