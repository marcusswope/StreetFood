using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using StreetFood.Web;
using StreetFood.Web.DependencyResolution;
using StructureMap;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(StructuremapMvc), "Start")]
[assembly: ApplicationShutdownMethod(typeof(StructuremapMvc), "End")]

namespace StreetFood.Web
{
    public static class StructuremapMvc
    {
        public static void End()
        {
        }

        public static void Start()
        {
            var container = IoC.Initialize();
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(container));
        }
    }

    public class StructureMapServiceLocator : ServiceLocatorImplBase
    {
        private readonly IContainer _container;

        public IContainer Container
        {
            get
            {
                return this._container;
            }
        }

        public StructureMapServiceLocator(IContainer container)
        {
            this._container = container;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (!string.IsNullOrEmpty(key))
                return this._container.GetInstance((Type)serviceType, key);
            return this._container.GetInstance((Type)serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return Enumerable.AsEnumerable<object>(Enumerable.Cast<object>((IEnumerable)this._container.GetAllInstances((Type)serviceType)));
        }

        public override TService GetInstance<TService>()
        {
            return this._container.GetInstance<TService>();
        }

        public override TService GetInstance<TService>(string key)
        {
            return this._container.GetInstance<TService>(key);
        }

        public virtual IEnumerable<TService> GetAllInstances<TService>()
        {
            return (IEnumerable<TService>)this._container.GetAllInstances<TService>();
        }
    }

    public class StructureMapDependencyResolver : IDependencyResolver
    {
        private readonly IContainer _container;

        public StructureMapDependencyResolver(IContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType.IsAbstract || serviceType.IsInterface)
            {
                return _container.TryGetInstance(serviceType);
            }
            else
            {
                return _container.GetInstance(serviceType);
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances<object>().Where(s => s.GetType() == serviceType);
        }
    }
}