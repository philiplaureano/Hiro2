using System.Collections.Generic;

namespace Hiro2
{
    public interface IDependencyMap : IMap<IDependency, IInstantiationPoint>
    {
    }
}