﻿using System;

namespace DependencyInjectionContainer
{
    public class Dependency
    {
        public Dependency(Type _Interface, Type _Realization, bool _IsSingleton = false)
        {
            Interface = _Interface;
            Realization = _Realization;
            IsSingleton = _IsSingleton;
            Instance = null;
        }

        public bool IsSingleton { get; set; }
        public object Instance { get; set; }
        public Type Interface { get; set; }
        public Type Realization { get; set; }                
    }
}
