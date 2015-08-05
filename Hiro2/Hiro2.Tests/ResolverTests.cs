using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hiro2.Tests.SampleDomains.ConstructorResolution;
using Hiro2.Tests.SampleDomains.Generics;
using Hiro2.Tests.SampleDomains.Vehicles;
using NUnit.Framework;
using Shouldly;

namespace Hiro2.Tests
{
    [TestFixture]
    public class ResolverTests
    {
        private IResolver _resolver;
        private IServiceRegistry _registry;
        [SetUp]
        public void Init()
        {
            _resolver = GetResolver();
            _registry = new ServiceRegistry();
        }

        [TearDown]
        public void Term()
        {
            _resolver = null;
            _registry = null;
        }

        [Test]
        public void Should_resolve_transient_type()
        {
            _registry.Register<IVehicle, CarWithNoEngine>();

            // Connect the dependencies
            var map = GetServiceMap();

            var targetDependency = new Dependency(typeof (IVehicle));
            map.ContainsKey(targetDependency)
                .ShouldBe(true);

            var targetPoint = map[targetDependency];
            targetPoint.ShouldBeOfType<TransientInstantiationPoint>();

            var constructor = (targetPoint as TransientInstantiationPoint)?.ActualPoint as Constructor;
            constructor.ShouldNotBe(null);
            constructor.ConstructorInfo.DeclaringType.ShouldBe(typeof(CarWithNoEngine));
        }

        [Test]
        public void Should_resolve_constructor_with_most_number_of_resolvable_parameters()
        {
            _registry.RegisterGeneric(typeof(ICollection<>), typeof(List<>));
            _registry.Register<ISampleInterface, ClassWithMultipleResolvableConstructors>();

            var map = GetServiceMap();

            var targetDependency = new Dependency(typeof(ISampleInterface));
            map.ContainsKey(targetDependency)
                .ShouldBe(true);

            var targetPoint = map[targetDependency];
            targetPoint.ShouldBeOfType<TransientInstantiationPoint>();

            var constructor = (targetPoint as TransientInstantiationPoint)?.ActualPoint as Constructor;
            constructor.ShouldNotBe(null);

            var constructorInfo = constructor.ConstructorInfo;
            constructorInfo.GetParameters().Count().ShouldBe(2);
        }

        [Test]
        public void Should_resolve_singleton()
        {
            _registry.RegisterSingleton<IVehicle, CarWithNoEngine>();

            // Connect the dependencies
            var map = GetServiceMap();

            var targetDependency = new Dependency(typeof(IVehicle));
            map.ContainsKey(targetDependency)
                .ShouldBe(true);

            var targetPoint = map[targetDependency];
            targetPoint.ShouldBeOfType<SingletonInstantiationPoint>();

            var constructor = (targetPoint as SingletonInstantiationPoint)?.ActualPoint as Constructor;
            constructor.ShouldNotBe(null);
            constructor.ConstructorInfo.DeclaringType.ShouldBe(typeof(CarWithNoEngine));
        }

        [Test]
        public void Should_resolve_generic_type()
        {
            _registry.RegisterGeneric(typeof(ICollection<>), typeof(List<>));
            _registry.Register<ClassWithGenericDependency>();

            var map = GetServiceMap();

            var targetDependency = new Dependency(typeof(ClassWithGenericDependency));
            map.ContainsKey(targetDependency)
                .ShouldBe(true);

            var targetPoint = map[targetDependency];
            targetPoint.ShouldBeOfType<TransientInstantiationPoint>();

            var constructor = (targetPoint as TransientInstantiationPoint)?.ActualPoint as Constructor;
            constructor.ShouldNotBe(null);
            constructor.ConstructorInfo.DeclaringType.ShouldBe(typeof(ClassWithGenericDependency));
        }

        [Test]
        public void Should_resolve_enumerables()
        {
            _registry.Register<IEngine, StockEngine>();
            _registry.Register<IVehicle, MultiEngineCar>();

            var map = GetServiceMap();

            var targetDependency = new Dependency(typeof(IVehicle));
            map.ContainsKey(targetDependency)
                .ShouldBe(true);

            map.ContainsKey(new Dependency(typeof(IEngine))).ShouldBe(true);

            var targetPoint = map[targetDependency];
            targetPoint.ShouldBeOfType<TransientInstantiationPoint>();

            var constructor = (targetPoint as TransientInstantiationPoint)?.ActualPoint as Constructor;
            constructor.ShouldNotBe(null);
            constructor.ConstructorInfo.DeclaringType.ShouldBe(typeof(MultiEngineCar));
        }

        private IDictionary<IDependency, IInstantiationPoint> GetServiceMap()
        {
            var points = _registry.GetAllRegisteredPoints();
            var map = _resolver.ResolveAll(points);
            return map;
        }
        private IResolver GetResolver()
        {
            return new Resolver();
        }
    }
}