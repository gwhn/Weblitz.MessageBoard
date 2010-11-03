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
                                                         s.TheCallingAssembly();
                                                         s.LookForRegistries();
                                                         s.AddAllTypesOf<IRequireConfigurationOnStartup>();
                                                     }));

//            ObjectFactory.Initialize(x => x.AddRegistry<ServiceRegistry>());

            foreach (var instance in ObjectFactory.GetAllInstances<IRequireConfigurationOnStartup>())
            {
                instance.Configure();
            }
        }
    }
}