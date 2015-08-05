using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiro2.Tests.SampleDomains.ConstructorResolution;

using NUnit.Framework;
using Shouldly;

namespace Hiro2.Tests
{
    [TestFixture]
    public class RegistrationTests
    {
        private IServiceRegistry _registry;

        [SetUp]
        public void Init()
        {
            _registry = GetRegistry();
        }

        [TearDown]
        public void Term()
        {
            _registry = null;
        }

        [Test]
        public void Should_create_constructor_instantiation_point_for_each_constructor_info()
        {
            _registry.Register<ClassWithMultipleResolvableConstructors>();

            var pointLists = _registry.GetAllRegisteredPoints().ToArray();
            pointLists.ShouldNotBeEmpty();

            var points = pointLists.First().Value;
            points.ShouldNotBe(null);
            points.Count().ShouldBe(2);

            var transientItems = points.Select(p => p as TransientInstantiationPoint).Where(p => p != null)
                .ToArray();

            transientItems.All(t => t.ActualPoint is Constructor)
                .ShouldBe(true);
        }

        [Test]
        public void Should_create_transient_points_for_each_constructor_on_a_registered_type()
        {
            _registry.Register<ClassWithMultipleResolvableConstructors>();

            var pointLists = _registry.GetAllRegisteredPoints().ToArray();
            pointLists.ShouldNotBeEmpty();

            var points = pointLists.First().Value;
            points.ShouldNotBe(null);
            points.Count().ShouldBe(2);

            points.Select(p => p as TransientInstantiationPoint).Where(p => p != null)
                .ToArray().Length.ShouldBe(2);
        }

        [Test]
        public void Should_create_singleton_points_for_each_constructor_on_a_registered_type()
        {
            _registry.RegisterSingleton(typeof(ClassWithMultipleResolvableConstructors), typeof(ClassWithMultipleResolvableConstructors));

            var pointLists = _registry.GetAllRegisteredPoints().ToArray();
            pointLists.ShouldNotBeEmpty();

            var points = pointLists.First().Value;
            points.ShouldNotBe(null);
            points.Count().ShouldBe(2);

            points.Select(p => p as SingletonInstantiationPoint).Where(p => p != null)
                .ToArray().Length.ShouldBe(2);
        }

        [Test]
        public void Should_create_generic_singleton_points()
        {
            _registry.RegisterGenericSingleton(typeof(IList<>), typeof(List));

            var points = _registry.GetAllRegisteredPoints().SelectMany(list => list.Value).ToArray();
            points.OfType<SingletonInstantiationPoint>().All(p => p.ActualPoint is GenericType).ShouldBe(true);
        }

        [Test]
        public void Should_create_generic_types()
        {
            _registry.RegisterGeneric(typeof(IList<>), typeof(List));

            var points = _registry.GetAllRegisteredPoints().SelectMany(list => list.Value).ToArray();
            points.OfType<TransientInstantiationPoint>().All(p => p.ActualPoint is GenericType).ShouldBe(true);
        }

        private IServiceRegistry GetRegistry()
        {
            return new ServiceRegistry();
        }
    }
}
