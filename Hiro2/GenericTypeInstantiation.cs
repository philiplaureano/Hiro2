using System;
using System.Collections.Generic;

namespace Hiro2
{
    public class GenericTypeInstantiation : InstantiationPoint
    {
        public GenericTypeInstantiation(IDependency dependency, Constructor constructor, IEnumerable<Type> typeArguments) : base(dependency)
        {
            if (dependency == null)
                throw new ArgumentNullException(nameof(dependency));

            if (constructor == null)
                throw new ArgumentNullException(nameof(constructor));

            Constructor = constructor;
            TypeArguments = typeArguments;
        }

        public Constructor Constructor { get; }
        public IEnumerable<Type> TypeArguments { get; }

        public override IEnumerable<IInstantiationPoint> GetResolvedDependencies() => Constructor.GetResolvedDependencies();

        public override IEnumerable<IDependency> GetRequiredDependencies()
        {
            return Constructor?.GetRequiredDependencies();
        }

        public override void AddResolveDependency(IInstantiationPoint otherPoint)
        {
            Constructor?.AddResolveDependency(otherPoint);
        }

        public override bool CanBeResolvedFrom(IEnumerable<IDependency> availableDependencies)
        {
            return Constructor != null && Constructor.CanBeResolvedFrom(availableDependencies);
        }
    }
}