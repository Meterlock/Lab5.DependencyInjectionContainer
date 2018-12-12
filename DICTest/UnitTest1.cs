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

        [TestMethod]
        public void ResolveGenTypeTest()
        {
            var config = new DependenciesConfiguration();
            config.Register(typeof(IFoo), typeof(Foo));
            config.Register(typeof(GenClass<IFoo>), typeof(GenClass<IFoo>));
            var provider = new DependencyProvider(config);
            GenClass<IFoo> obj = provider.Resolve<GenClass<IFoo>>();
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void WrongRegisterTest()
        {
            var config = new DependenciesConfiguration();
            config.Register(typeof(IWrong), typeof(WrongClass1));
            var provider = new DependencyProvider(config);
            IWrong obj = provider.Resolve<IWrong>();
            Assert.IsNull(obj);
        }

        [TestMethod]
        public void NotWrongRegisterTest()
        {
            var config = new DependenciesConfiguration();
            config.Register(typeof(WrongClass1), typeof(NotWrong));
            var provider = new DependencyProvider(config);
            IWrong obj = provider.Resolve<WrongClass1>();
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void NestedTest()
        {
            var config = new DependenciesConfiguration();
            config.Register(typeof(WrongClass1), typeof(NotWrong));
            config.Register(typeof(IFoo), typeof(NotWrongInFoo));
            var provider = new DependencyProvider(config);
            NotWrongInFoo obj = (NotWrongInFoo)provider.Resolve<IFoo>();
            Assert.IsNotNull(obj.notWrong);
        }
    }
}
