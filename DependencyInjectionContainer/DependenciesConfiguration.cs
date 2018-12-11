using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionContainer
{
    class DependenciesConfiguration
    {
        public DependenciesConfiguration()
        {
            dictionary = new ConcurrentDictionary<Type, List<Dependency>>();
        }

        private ConcurrentDictionary<Type, List<Dependency>> dictionary;

        public void Register(Type Interface, Type realization, bool isSingleton = false)
        {
            if (!realization.IsInterface && !realization.IsAbstract && Interface.IsAssignableFrom(realization))
            {
                Dependency dependency = new Dependency(Interface, realization, isSingleton);
                List<Dependency> dependencies;

                if (!dictionary.TryGetValue(Interface, out dependencies))
                {
                    dictionary.TryAdd(Interface, new List<Dependency>() { dependency });
                }
                else if (!dependencies.Contains(dependency))
                {
                    dependencies.Add(dependency);
                }
            }
        }

        IEnumerable<Dependency> GetDependencies(Type Interface)
        {
            List<Dependency> dependencies;
            return dictionary.TryGetValue(Interface, out dependencies) ? dependencies : null;
        }
    }
}
