using CumulusFramework.Domain.Entity;
using System;
using System.Collections.Generic;

namespace CumulusFramework.Domain.Data.Interface
{
    public interface IDBEnvironmentRepository: IRepository<DBEnvironment>
    {
        DBEnvironment GetByName(String name);
        List<DBEnvironment> GetByEnvironmentType(Int32 type);
    }
}
