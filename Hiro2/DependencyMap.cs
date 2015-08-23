using System.Collections.Generic;

namespace Hiro2
{
    public class DependencyMap : IDependencyMap
    {
        private readonly IDictionary<IDependency, IInstantiationPoint> _dependencyMapEntries;

        public DependencyMap(IDictionary<IDependency, IInstantiationPoint> dependencyMapEntries)
        {
            _dependencyMapEntries = dependencyMapEntries;
        }

        public bool Contains(IDependency dependency)
        {
            return _dependencyMapEntries.ContainsKey(dependency);
        }

        public IEnumerable<IDependency> Keys => _dependencyMapEntries.Keys;

        public void Add(IDependency key, IInstantiationPoint item)
        {
            _dependencyMapEntries[key] = item;
        }

        public IInstantiationPoint GetItem(IDependency dependency)
        {
            return _dependencyMapEntries[dependency];
        }
    }
}