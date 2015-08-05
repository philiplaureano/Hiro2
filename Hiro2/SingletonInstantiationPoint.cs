using System.Collections.Generic;

namespace Hiro2
{
    public class SingletonInstantiationPoint : InstantiationPoint, ISingletonInstance
    {
        private readonly IInstantiationPoint _actualPoint;

        public SingletonInstantiationPoint(IInstantiationPoint actualPoint) : base(actualPoint.Dependency)
        {
            _actualPoint = actualPoint;
        }

        public override IEnumerable<IDependency> GetRequiredDependencies()
        {
            return _actualPoint.GetRequiredDependencies();
        }

        public override void AddResolveDependency(IInstantiationPoint otherPoint)
        {
            _actualPoint.AddResolveDependency(otherPoint);
        }

        public override IEnumerable<IInstantiationPoint> GetResolvedDependencies()
        {
            return _actualPoint.GetResolvedDependencies();
        }
    }
}