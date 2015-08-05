using System.Collections.Generic;
using Hiro2.Interfaces;

namespace Hiro2
{
    public interface IServiceRegistry : IServiceLocatorRegistry
    {
        IDictionary<IDependency, ICollection<IInstantiationPoint>> GetAllRegisteredPoints();
    }
}