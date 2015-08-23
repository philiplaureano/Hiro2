using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace Hiro2
{
    public interface IFactoryMap
    {
        bool Contains(IDependency dependency);
        IEnumerable<IDependency> Dependencies { get; }
        void RegisterFactory(IDependency dependency, Func<IServiceLocator, object> factoryMethod);
        Func<IServiceLocator, object> GetFactory(IDependency dependency);
    }
}