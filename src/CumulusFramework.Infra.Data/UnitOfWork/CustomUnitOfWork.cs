using CumulusFramework.Domain.Entity;
using CumulusFramework.Domain.Entity;

namespace CumulusFramework.Infra.Data.UnitOfWork
{
    public class CustomUnitOfWork : NHibernateUnitOfWork
    {
        public CustomUnitOfWork(DBEnvironment environment) : base(environment)
        {

        }
    }
}
