using CumulusFramework.Domain.DependencyInjection;
using System;

namespace CumulusFramework.Domain.Data
{
    public abstract class CRUDServiceBase
    {
        protected readonly IDependencyInjectionContainer dependencyInjectionContainer;
        public CRUDServiceBase(IDependencyInjectionContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            dependencyInjectionContainer = container;
        }

    }
}
