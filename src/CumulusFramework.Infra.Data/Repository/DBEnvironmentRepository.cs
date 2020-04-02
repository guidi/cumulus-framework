using System;
using CumulusFramework.Domain.Data.Interface;
using CumulusFramework.Domain.Entity;
using NHibernate.Linq;
using System.Linq;
using System.Collections.Generic;

namespace CumulusFramework.Infra.Data.Repository
{
    public class DBEnvironmentRepository : Repository<DBEnvironment>, IDBEnvironmentRepository
    {
        public DBEnvironmentRepository(IUnitOfWork unitOfWork) : base(unitOfWork){}

        public List<DBEnvironment> GetByEnvironmentType(Int32 type)
        {
            return Session.Query<DBEnvironment>().Where(d => d.EnvironmentType == type).ToList();
        }

        public DBEnvironment GetByName(String name)
        {
            return Session.Query<DBEnvironment>().Where(d => d.Name == name).FirstOrDefault();
        }
    }
}
