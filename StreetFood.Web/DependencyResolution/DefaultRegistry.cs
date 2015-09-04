using StreetFood.Authentication;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace StreetFood.Web.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.SingleImplementationsOfInterface();
                    scan.AssemblyContainingType<Account>();
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                });
        }
    }
}