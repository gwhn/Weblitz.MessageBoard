using StructureMap;
using Weblitz.MessageBoard.Core;

namespace Weblitz.MessageBoard.Infrastructure.StructureMap
{
    internal static class DependencyRegistration
    {
        public static void Register()
        {
            ObjectFactory.ResetDefaults();

            ObjectFactory.Initialize(x => x.Scan(s =>
                                                     {
                                                         s.WithDefaultConventions();
                                                         s.TheCallingAssembly();
                                                         s.LookForRegistries();
                                                         s.AddAllTypesOf<IRequiresConfigurationOnStartup>();
                                                     }));

            foreach (var instance in ObjectFactory.GetAllInstances<IRequiresConfigurationOnStartup>())
            {
                instance.Configure();
            }
        }
    }
}