using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Hiro2
{
    public class Resolver : IResolver
    {
        public IDictionary<IDependency, IInstantiationPoint> ResolveAll(IDictionary<IDependency, ICollection<IInstantiationPoint>> pointMap)
        {
            var currentMap = new ConcurrentDictionary<IDependency, ICollection<IInstantiationPoint>>();
            foreach (var key in pointMap.Keys)
            {
                currentMap[key] = new List<IInstantiationPoint>();
                foreach (var item in pointMap[key])
                {
                    currentMap[key].Add(item);
                }
            }

            var instantiationPoints = currentMap.Keys.SelectMany(dependency => pointMap[dependency]).ToArray();
            var pointsWithNoDependencies = instantiationPoints
                .Where(p => !p.GetRequiredDependencies().Any()).ToList();

            var immediatelyAvailableServices = pointsWithNoDependencies.ToDictionary(p => p.Dependency);
            var availableServices = new Dictionary<IDependency, IInstantiationPoint>();
            foreach (var key in immediatelyAvailableServices.Keys)
            {
                availableServices[key] = immediatelyAvailableServices[key];
            }

            var pointsGroupedByDependency = GetPointsGroupedByDependency(instantiationPoints);

            ResolveFrom(pointsGroupedByDependency, availableServices);

            var unresolvedDependencies = pointsGroupedByDependency.Keys.ToArray();
            var enumerableDependencies = ResolveEnumerableDependencies(unresolvedDependencies, availableServices, pointsGroupedByDependency);

            // Resolve all generic services
            ResolveGenericDependencies(unresolvedDependencies, enumerableDependencies, availableServices, pointsGroupedByDependency);

            return availableServices;
        }

        private static IDictionary<IDependency, ICollection<IInstantiationPoint>> GetPointsGroupedByDependency(IInstantiationPoint[] instantiationPoints)
        {
            var pointsGroupedByDependency = new ConcurrentDictionary<IDependency, ICollection<IInstantiationPoint>>();
            foreach (var point in instantiationPoints)
            {
                var dependencies = point.GetRequiredDependencies().ToArray();
                var nonPrimitiveDependencies = dependencies.Where(dependency => !dependency.DependencyType.IsPrimitive);
                foreach (var dependency in nonPrimitiveDependencies)
                {
                    if (!pointsGroupedByDependency.ContainsKey(dependency))
                        pointsGroupedByDependency[dependency] = new List<IInstantiationPoint>();

                    pointsGroupedByDependency[dependency].Add(point);
                }
            }
            return pointsGroupedByDependency;
        }

        private static IEnumerable<IDependency> ResolveEnumerableDependencies(IEnumerable<IDependency> unresolvedDependencies,
            IDictionary<IDependency, IInstantiationPoint> availableServices, IDictionary<IDependency, ICollection<IInstantiationPoint>> pointsGroupedByDependency)
        {
            // Resolve all IEnumerable services
            var enumerableDependencies =
                unresolvedDependencies.Where(
                    d => d?.DependencyType != null &&
                         d.DependencyType.IsGenericType &&
                         d.DependencyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    .Where(d => availableServices.Keys.Any(k => k.DependencyType == d.DependencyType.GenericTypeArguments[0]))
                    .ToArray();

            foreach (var dependency in enumerableDependencies)
            {
                var serviceType = dependency.DependencyType.GenericTypeArguments[0];
                var availableDependenciesOfThatType = availableServices.Keys.Where(k => k.DependencyType == serviceType);

                var enumerablePoint = new EnumerableInstantiationPoint(dependency, availableDependenciesOfThatType);
                availableServices[dependency] = enumerablePoint;
            }

            ResolveFrom(pointsGroupedByDependency, availableServices);
            return enumerableDependencies;
        }

        private static void ResolveGenericDependencies(IEnumerable<IDependency> unresolvedDependencies,
            IEnumerable<IDependency> enumerableDependencies, IDictionary<IDependency, IInstantiationPoint> availableServices,
            IDictionary<IDependency, ICollection<IInstantiationPoint>> pointsGroupedByDependency)
        {
            var genericDependencies = unresolvedDependencies.Where(
                d => d?.DependencyType != null &&
                     d.DependencyType.IsGenericType && !d.DependencyType.IsGenericTypeDefinition &&
                     d.DependencyType.GetGenericTypeDefinition() != typeof(IEnumerable<>) &&
                     !enumerableDependencies.Contains(d));

            var availableGenericDependencies =
                availableServices.Keys.Where(
                    d => d?.DependencyType != null && d.DependencyType.IsGenericTypeDefinition &&
                         d.DependencyType.GetGenericTypeDefinition() != typeof(IEnumerable<>)).ToArray();

            var satisfiableGenericDependencies = genericDependencies.Where(d => d?.DependencyType != null &&
                                                                                availableGenericDependencies.Any(
                                                                                    ad =>
                                                                                        ad.DependencyType ==
                                                                                        d.DependencyType
                                                                                            .GetGenericTypeDefinition()))
                .ToArray();

            foreach (var dependency in satisfiableGenericDependencies)
            {
                var dependencyType = dependency.DependencyType;
                var typeDefinition = dependencyType.GetGenericTypeDefinition();
                var typeArguments = dependencyType.GenericTypeArguments;

                var availableMatchingPoints =
                    availableGenericDependencies.Where(d => d.DependencyType == typeDefinition)
                        .Select(d => availableServices[d] as GenericType)
                        .ToArray();

                foreach (var point in availableMatchingPoints)
                {
                    var newPoint = new GenericTypeInstantiation(dependency, point.Constructor, typeArguments);
                    availableServices[dependency] = newPoint;
                }
            }

            // Update the map with the injected generic services
            ResolveFrom(pointsGroupedByDependency, availableServices);
        }

        private static void ResolveFrom(IDictionary<IDependency,
            ICollection<IInstantiationPoint>> pointsGroupedByDependency,
            IDictionary<IDependency, IInstantiationPoint> availableServices)
        {
            while (true)
            {
                var numberOfResolvedPoints = Resolve(pointsGroupedByDependency, availableServices);
                if (numberOfResolvedPoints == 0)
                    break;
            }
        }

        private static int Resolve(IDictionary<IDependency, ICollection<IInstantiationPoint>> pointsGroupedByDependency, IDictionary<IDependency, IInstantiationPoint> availableServices)
        {
            var updatedPoints = new List<IInstantiationPoint>();
            foreach (var dependency in availableServices.Keys)
            {
                var availablePoint = availableServices[dependency];
                if (!pointsGroupedByDependency.ContainsKey(dependency))
                    continue;

                var currentPoints = pointsGroupedByDependency[dependency];
                foreach (var point in currentPoints)
                {
                    point.AddResolveDependency(availablePoint);
                    updatedPoints.Add(point);
                }

                pointsGroupedByDependency.Remove(dependency);
            }

            var newlyAvailableServices = updatedPoints.Where(p => p.CanBeResolvedFrom(availableServices.Keys)).ToArray();
            foreach (var point in newlyAvailableServices)
            {
                var currentDependency = point.Dependency;
                availableServices[currentDependency] = point;
            }

            return newlyAvailableServices.Length;
        }
    }
}