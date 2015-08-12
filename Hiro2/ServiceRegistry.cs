using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hiro2.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace Hiro2
{
    public class ServiceRegistry : IServiceRegistry
    {
        private readonly IDictionary<IDependency, ICollection<IInstantiationPoint>> _points = new Dictionary<IDependency, ICollection<IInstantiationPoint>>();

        public void Register(Type concreteType)
        {
            Register(concreteType, concreteType, ctors => ctors.Select(c => new TransientInstantiationPoint(new Constructor(new Dependency(concreteType), c))));
        }

        public void RegisterSingleton(Type serviceType, Type concreteType)
        {
            Register(serviceType, concreteType, ctors => ctors.Select(c => new SingletonInstantiationPoint(new Constructor(new Dependency(serviceType), c))));
        }

        public void Register<TConcreteType>()
        {
            Register<TConcreteType, TConcreteType>();
        }

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            var dependencyType = typeof(TInterface);
            var dependency = new Dependency(dependencyType);

            AddPointsAsTransient<TInterface, TImplementation>(dependency);
        }

        public void Register<TInterface>(Func<IServiceLocator, TInterface> factoryMethod, string serviceName)
        {
            var dependency = new Dependency(typeof(TInterface), serviceName);
            if (!_points.ContainsKey(dependency))
                _points[dependency] = new List<IInstantiationPoint>();

            _points[dependency].Add(new TransientInstantiationPoint(new FunctorInstantiationPoint(dependency, locator => factoryMethod(locator))));
        }

        public void Register<TInterface>(Func<IServiceLocator, TInterface> factoryMethod)
        {
            var dependency = new Dependency(typeof(TInterface));
            if (!_points.ContainsKey(dependency))
                _points[dependency] = new List<IInstantiationPoint>();

            _points[dependency].Add(new TransientInstantiationPoint(new FunctorInstantiationPoint(dependency, locator => factoryMethod(locator))));
        }

        public void RegisterSingleton<TInterface>(Func<IServiceLocator, TInterface> factoryMethod)
        {
            var dependency = new Dependency(typeof(TInterface));
            if (!_points.ContainsKey(dependency))
                _points[dependency] = new List<IInstantiationPoint>();

            _points[dependency].Add(new SingletonInstantiationPoint(new FunctorInstantiationPoint(dependency, locator => factoryMethod(locator))));
        }

        public void RegisterSingleton<TInterface>(Func<IServiceLocator, TInterface> factoryMethod, string serviceName)
        {
            var dependency = new Dependency(typeof(TInterface), serviceName);
            if (!_points.ContainsKey(dependency))
                _points[dependency] = new List<IInstantiationPoint>();

            _points[dependency].Add(new SingletonInstantiationPoint(new FunctorInstantiationPoint(dependency, locator => factoryMethod(locator))));
        }

        public void RegisterSingleton<TInterface, TImplementation>() where TImplementation : TInterface
        {
            AddPointsAsSingleton<TInterface, TImplementation>(new Dependency(typeof(TInterface)));
        }

        public void RegisterGeneric(Type serviceType, Type implementingType)
        {
            Register(serviceType, implementingType, ctors => ctors.Select(c => new GenericType(new Dependency(serviceType), c)));
        }

        public void RegisterGenericSingleton(Type serviceType, Type implementingType)
        {
            Register(serviceType, implementingType,
                ctors => ctors.Select(c => new SingletonInstantiationPoint(new GenericType(new Dependency(serviceType), c))));
        }

        public void Register<TInterface, TImplementation>(string name) where TImplementation : TInterface
        {
            var dependencyType = typeof(TInterface);
            var dependency = new Dependency(dependencyType, name);

            AddPointsAsTransient<TInterface, TImplementation>(dependency);
        }

        public IDictionary<IDependency, ICollection<IInstantiationPoint>> GetAllRegisteredPoints()
        {
            return _points;
        }

        private void AddInstantiationPoints<TInterface, TImplementation>(IDependency dependency,
            Func<IEnumerable<ConstructorInfo>, IEnumerable<IInstantiationPoint>> getPointsFromConstructor) where TImplementation : TInterface
        {
            var implementingType = typeof(TImplementation);
            var constructors = implementingType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            var points = getPointsFromConstructor(constructors);

            if (!_points.ContainsKey(dependency))
                _points[dependency] = new List<IInstantiationPoint>();

            var currentList = _points[dependency];
            foreach (var point in points)
            {
                currentList.Add(point);
            }
        }

        private void AddPointsAsTransient<TInterface, TImplementation>(IDependency dependency) where TImplementation : TInterface
        {
            AddInstantiationPoints<TInterface, TImplementation>(dependency, point => new TransientInstantiationPoint(point));
        }

        private void AddPointsAsSingleton<TInterface, TImplementation>(IDependency dependency) where TImplementation : TInterface
        {
            AddInstantiationPoints<TInterface, TImplementation>(dependency, point => new SingletonInstantiationPoint(point));
        }

        private void AddInstantiationPoints<TInterface, TImplementation>(IDependency dependency,
            Func<IInstantiationPoint, InstantiationPoint> makeInstancedInstantiationPoint)
            where TImplementation : TInterface
        {
            Func<IEnumerable<ConstructorInfo>, IEnumerable<IInstantiationPoint>> getPointsFromConstructor =
                ctors => ctors.Select(c => makeInstancedInstantiationPoint(new Constructor(dependency, c)));

            AddInstantiationPoints<TInterface, TImplementation>(dependency, getPointsFromConstructor);
        }

        private void Register(Type serviceType, Type implementingType, Func<IEnumerable<ConstructorInfo>, IEnumerable<IInstantiationPoint>> getPointsFromConstructor)
        {
            var dependency = new Dependency(serviceType);
            var constructors = implementingType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            var points = getPointsFromConstructor(constructors);

            if (!_points.ContainsKey(dependency))
                _points[dependency] = new List<IInstantiationPoint>();

            var currentList = _points[dependency];
            foreach (var point in points)
            {
                currentList.Add(point);
            }
        }
    }
}