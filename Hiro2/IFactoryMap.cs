using System;
using Microsoft.Practices.ServiceLocation;

namespace Hiro2
{
    public interface IFactoryMap : IMap<IDependency, Func<IServiceLocator, object>>
    {
    }
}