using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hiro2
{
    public class Constructor : InstantiationPoint
    {
        private readonly IEnumerable<IDependency> _requiredDependencies;
        public Constructor(IDependency dependency, ConstructorInfo constructor) : base(dependency)
        {
            ConstructorInfo = constructor;

            _requiredDependencies = constructor.GetParameters()
                .Select(p => new Dependency(p.ParameterType))
                .ToArray();
        }

        public override IEnumerable<IDependency> GetRequiredDependencies()
        {
            return _requiredDependencies;
        }

        public ConstructorInfo ConstructorInfo { get; }
    }
}