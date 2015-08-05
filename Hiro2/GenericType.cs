using System.Collections.Generic;
using System.Reflection;

namespace Hiro2
{
    public class GenericType : InstantiationPoint
    {
        private readonly Constructor _constructor;
        public GenericType(IDependency dependency, ConstructorInfo genericConstructor) : base(dependency)
        {
            _constructor = new Constructor(dependency, genericConstructor);
        }

        public override IEnumerable<IDependency> GetRequiredDependencies()
        {
            return _constructor.GetRequiredDependencies();
        }

        public override IEnumerable<IInstantiationPoint> GetResolvedDependencies() => _constructor.GetResolvedDependencies();

        public override void AddResolveDependency(IInstantiationPoint otherPoint)
        {
            _constructor.AddResolveDependency(otherPoint);
        }

        public override bool CanBeResolvedFrom(IEnumerable<IDependency> availableDependencies)
        {
            return _constructor.CanBeResolvedFrom(availableDependencies);
        }

        public Constructor Constructor => _constructor;
    }
}