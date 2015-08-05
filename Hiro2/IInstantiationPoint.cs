using System.Collections.Generic;

namespace Hiro2
{
    public interface IInstantiationPoint
    {
        IDependency Dependency { get; }
        IEnumerable<IInstantiationPoint> GetResolvedDependencies();
        IEnumerable<IDependency> GetRequiredDependencies();
        void AddResolveDependency(IInstantiationPoint otherPoint);
        bool CanBeResolvedFrom(IEnumerable<IDependency> availableDependencies);
    }
}