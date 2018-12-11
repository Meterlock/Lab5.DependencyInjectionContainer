using System;
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
            dictionary = new Dictionary<Type, List<Dependency>>();
        }

        private Dictionary<Type, List<Dependency>> dictionary;

        public void Register(Type Interface, Type realization, bool isSingleton = false) { }
    }
}
