using StructureMap;

namespace StreetFood.Web.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
                {
                    c.AddRegistry<DefaultRegistry>();
                    c.AddRegistry<RavenDBRegistry>();
                });
        }
    }
}