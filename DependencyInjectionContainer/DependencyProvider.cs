using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

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
            Type type = typeof(T);
            Dependency dependency = configuration.GetDependency(type);

            if ((dependency == null) && type.IsGenericType)
            {
                dependency = configuration.GetDependency(type.GetGenericTypeDefinition());
            }

            if (dependency != null)
            {
                return (T)GetInstance(dependency);
            }

            return null;
        }

        private object GetInstance(Dependency dependency)
        {
            if (dependency.IsSingleton)
            {
                if (dependency.Instance == null)
                {
                    lock (dependency)
                    {
                        if (dependency.Instance == null)
                        {
                            dependency.Instance = Create(dependency);
                        }
                    }
                }
                return dependency.Instance;
            }
            
            return Create(dependency);
        }

        private object Create(Dependency dependency)
        {
            object result = null;

            if (!cycleStack.Contains(dependency.Interface))
            {
                cycleStack.Push(dependency.Interface);

                Type instanceType = dependency.Realization;
                if (instanceType.IsGenericTypeDefinition)
                {
                    instanceType = instanceType.MakeGenericType(dependency.Interface.GenericTypeArguments);
                }

                ConstructorInfo[] constructors = instanceType.GetConstructors().OrderByDescending(x => x.GetParameters().Length).ToArray();
                bool isCreated = false;
                int i = 0;         
                while (!isCreated && (i <= constructors.Count() - 1))
                {
                    try
                    {
                        ConstructorInfo current = constructors[i];
                        object[] parameters = GetConstructorParameters(current);
                        result = Activator.CreateInstance(instanceType, parameters);
                        isCreated = true;
                    }
                    catch
                    {
                        isCreated = false;
                        i++;
                    }
                }

                Type type;
                cycleStack.TryPop(out type);

                if (isCreated) return result;
                else return null;
            }

            return result;
        }

        private object[] GetConstructorParameters(ConstructorInfo constructor)
        {
            ParameterInfo[] parameterInfo = constructor.GetParameters();
            var parameters = new object[parameterInfo.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = GetInstance(configuration.GetDependency(parameterInfo[i].ParameterType));
            }
            return parameters;
        }


        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            IList collection = null;
            Type type = typeof(T);
            var dependencies = configuration.GetDependencies(type);
            if (dependencies != null)
            {
                collection = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
                foreach (var dependency in dependencies)
                {
                    collection.Add(GetInstance(dependency));
                }
            }
            return (IEnumerable<T>)collection;
        }
    }
}
