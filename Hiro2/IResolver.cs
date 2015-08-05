using System.Collections.Generic;

namespace Hiro2
{
    public interface IResolver
    {
        IDictionary<IDependency, IInstantiationPoint> ResolveAll(IDictionary<IDependency, ICollection<IInstantiationPoint>> pointMap);
    }
}