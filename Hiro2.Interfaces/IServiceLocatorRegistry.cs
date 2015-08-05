using System;
using Microsoft.Practices.ServiceLocation;

namespace Hiro2.Interfaces
{
    public interface IServiceLocatorRegistry
    {
        void Register(Type concreteType);
        void RegisterSingleton(Type serviceType, Type concreteType);
        void Register<TConcreteType>();
        void Register<TInterface, TImplementation>() where TImplementation : TInterface;
        void Register<TInterface>(Func<IServiceLocator, TInterface> factoryMethod);
        void RegisterSingleton<TInterface>(Func<IServiceLocator, TInterface> factoryMethod);
        void RegisterSingleton<TInterface, TImplementation>() where TImplementation : TInterface;
        void RegisterGeneric(Type serviceType, Type implementingType);
        void RegisterGenericSingleton(Type serviceType, Type implementingType);
        void Register<TInterface, TImplementation>(string name) where TImplementation : TInterface;
        void Register<TInterface>(Func<IServiceLocator, TInterface> factoryMethod, string serviceName);
    }
}