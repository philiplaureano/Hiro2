using System.Collections.Generic;

namespace Hiro2
{
    public interface IDependencyMap
    {
        bool Contains(IDependency dependency);

        IEnumerable<IDependency> Dependencies { get; }
        IInstantiationPoint GetInstantiationPoint(IDependency dependency);
    }
}