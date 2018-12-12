using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DependencyInjectionContainer;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DICTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SingletonTest()
        {
            var config = new DependenciesConfiguration();
            config.Register(typeof(IFoo),typeof(Foo), true);
            var provider = new DependencyProvider(config);
            IFoo foo1 = provider.Resolve<IFoo>();
            IFoo foo2 = provider.Resolve<IFoo>();
            Assert.ReferenceEquals(foo1, foo2);
        }

        [TestMethod]
        public void NotSingletonTest()
        {
            var config = new DependenciesConfiguration();
            config.Register(typeof(IFoo), typeof(Foo), false);
            var provider = new DependencyProvider(config);
            IFoo foo1 = provider.Resolve<IFoo>();
            IFoo foo2 = provider.Resolve<IFoo>();            
            Assert.AreNotEqual(foo1, foo2);
        }

        [TestMethod]
        public void SomeRealizationsTest()
        {
            var config = new DependenciesConfiguration();
            config.Register(typeof(IFoo), typeof(Foo), false);
            config.Register(typeof(IFoo), typeof(Foo2), false);
            var provider = new DependencyProvider(config);
            var objects = provider.ResolveAll<IFoo>();            
            Assert.AreEqual(objects.Count(), 2);
        }
    }
}
