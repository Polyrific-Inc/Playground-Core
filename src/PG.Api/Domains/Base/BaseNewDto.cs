// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using PG.Model;

namespace PG.Api.Domains.Base
{
    public abstract class BaseNewDto<TEntity>
        where TEntity : BaseModel, new()
    {

    }
}
