using System;
using System.Collections.Generic;

namespace Hiro2.Interfaces
{
    public interface IServiceLocator
    {
        object Resolve(Type serviceType);
        T Resolve<T>();
        T Resolve<T>(string serviceName);
        IEnumerable<T> ResolveAll<T>();
    }
}