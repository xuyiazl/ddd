﻿using DDD.Domain.Core.Interfaces;
using XUCore.NetCore.Data.DbService;

namespace DDD.Persistence
{
    public class NigelDbUnitOfWork : UnitOfWork, INigelDbUnitOfWork
    {
        public NigelDbUnitOfWork(INigelDbContext dbContext) : base(dbContext) { }
    }
}
