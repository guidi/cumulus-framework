using CumulusFramework.Domain.Entity;
using System;

namespace CumulusFramework.Domain.DependencyInjection
{
    public interface IDependencyInjectionContainer
    {
        void RegisterDependencies();
        void RegisterDependencies(DBEnvironment environment);
        Object Resolve(Type type);
        TService Resolve<TService>() where TService : class;
        TService Register<TService>(Func<TService> instanceCreator) where TService : class;
        void Register<TService>() where TService : class;
        void RegisterSingleton<TService, TImplementation>() where TService : class
            where TImplementation : class, TService;
        void VerifyConsistency();
    }
}
