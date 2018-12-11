using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionContainer
{
    class DependencyProvider
    {
        public DependencyProvider(DependenciesConfiguration config)
        {
            configuration = config;
            cycleStack = new ConcurrentStack<Type>();
        }

        private DependenciesConfiguration configuration;
        private ConcurrentStack<Type> cycleStack;

        public T Resolve<T>() where T : class
        {
            return (T) new Object();
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return (IEnumerable<T>)new Object();
        }
    }
}
