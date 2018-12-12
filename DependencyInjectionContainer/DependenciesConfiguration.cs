using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjectionContainer
{
    public class DependenciesConfiguration
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
                var dependency = new Dependency(Interface, realization, isSingleton);
                List<Dependency> dependencies;

                if (!dictionary.TryGetValue(Interface, out dependencies))
                {
                    lock (dictionary)
                    {
                        if (!dictionary.TryGetValue(Interface, out dependencies))
                        {
                            dictionary.TryAdd(Interface, new List<Dependency>() { dependency });
                            return;
                        }
                    }                    
                }

                if (!dependencies.Contains(dependency))
                {
                    lock (dependencies)
                    {
                        if (!dependencies.Contains(dependency))
                        {
                            dependencies.Add(dependency);
                        }
                    }                    
                }
            }
        }

        public Dependency GetDependency(Type Interface)
        {
            List<Dependency> dependencies;
            return dictionary.TryGetValue(Interface, out dependencies) ? dependencies.Last() : null;
        }

        public IEnumerable<Dependency> GetDependencies(Type Interface)
        {
            List<Dependency> dependencies;
            return dictionary.TryGetValue(Interface, out dependencies) ? dependencies : null;
        }
    }
}
