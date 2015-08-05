using System.Collections.Generic;

namespace Hiro2
{
    public class EnumerableInstantiationPoint : InstantiationPoint
    {
        private readonly IEnumerable<IDependency> _dependenciesToInstantiate;

        public EnumerableInstantiationPoint(IDependency dependency, IEnumerable<IDependency> dependenciesToInstantiate) : base(dependency)
        {
            _dependenciesToInstantiate = dependenciesToInstantiate;
        }

        public override IEnumerable<IDependency> GetRequiredDependencies()
        {
            return _dependenciesToInstantiate;
        }
    }
}