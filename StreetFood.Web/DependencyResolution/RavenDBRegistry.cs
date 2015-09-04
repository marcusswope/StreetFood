using Raven.Client;
using Raven.Client.Document;
using StructureMap.Configuration.DSL;
using StructureMap.Web.Pipeline;

namespace StreetFood.Web.DependencyResolution
{
    public class RavenDBRegistry : Registry
    {
        public RavenDBRegistry()
        {
            var store = new DocumentStore
            {
                ConnectionStringName = "StreetFood",
            }.Initialize();

            For<IDocumentStore>().Singleton().Use(store);
            For<IDocumentSession>().LifecycleIs<HttpContextLifecycle>().Use(() => store.OpenSession());
        }
    }
}