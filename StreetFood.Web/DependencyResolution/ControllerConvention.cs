using System;
using System.Web.Mvc;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Pipeline;
using StructureMap.TypeRules;

namespace StreetFood.Web.DependencyResolution
{
    public class ControllerConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.CanBeCastTo<Controller>() && !type.IsAbstract)
            {
                registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
            }
        }
    }
}