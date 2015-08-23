using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace Hiro2
{
    public class FactoryMap : IFactoryMap
    {
        private readonly IDictionary<IDependency, Func<IServiceLocator, object>> _factoryMap =
            new ConcurrentDictionary<IDependency, Func<IServiceLocator, object>>();

        public bool Contains(IDependency dependency)
        {
            return _factoryMap.ContainsKey(dependency);
        }

        public void Add(IDependency dependency, Func<IServiceLocator, object> factoryMethod)
        {
            _factoryMap[dependency] = factoryMethod;
        }

        public Func<IServiceLocator, object> GetItem(IDependency dependency)
        {
            return _factoryMap[dependency];
        }

        public IEnumerable<IDependency> Keys => _factoryMap.Keys;
    }
}