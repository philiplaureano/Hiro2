using System.Collections.Generic;
using System.Linq;

namespace Hiro2
{
    public abstract class InstantiationPoint : IInstantiationPoint
    {
        private readonly List<IInstantiationPoint> _resolvedPoints = new List<IInstantiationPoint>();
        protected InstantiationPoint(IDependency dependency)
        {
            Dependency = dependency;
        }

        public IDependency Dependency { get; }
        public abstract IEnumerable<IDependency> GetRequiredDependencies();

        public virtual void AddResolveDependency(IInstantiationPoint otherPoint)
        {
            // Ignore the dependency if it is the same dependency as the current point
            if (Equals(otherPoint.Dependency, Dependency))
                return;

            _resolvedPoints.Add(otherPoint);
        }

        public virtual IEnumerable<IInstantiationPoint> GetResolvedDependencies() => _resolvedPoints.ToArray();

        public virtual bool CanBeResolvedFrom(IEnumerable<IDependency> availableDependencies)
        {
            var requiredDependencies = GetRequiredDependencies();

            var currentlyAvailableDependencies = new HashSet<IDependency>(availableDependencies);

            return requiredDependencies.All(dependency => currentlyAvailableDependencies.Contains(dependency));
        }
    }
}