using System;
using Autofac;

namespace ImperaPlus.Domain
{
    /// <summary>
    /// Ugly workaround for the removal of "ObjectMaterialized"
    /// </summary>
    public static class DomainDepsResolver
    {
        public static Func<ILifetimeScope> ScopeGen { get; set; }

        public static T Resolve<T>()
        {
            return ScopeGen().Resolve<T>();
        }
    }
}
