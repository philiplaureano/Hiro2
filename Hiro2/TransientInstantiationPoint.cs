using System.Collections.Generic;

namespace Hiro2
{
    public class TransientInstantiationPoint : InstantiationPoint, ITransientInstance
    {
        private readonly IInstantiationPoint _actualPoint;

        public TransientInstantiationPoint(IInstantiationPoint actualPoint) : base(actualPoint.Dependency)
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