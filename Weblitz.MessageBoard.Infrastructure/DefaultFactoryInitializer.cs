using StructureMap;
using Weblitz.MessageBoard.Core;
using Weblitz.MessageBoard.Web.Controllers.Factories;

namespace Weblitz.MessageBoard.Infrastructure
{
    public class DefaultFactoryInitializer : IRequiresConfigurationOnStartup
    {
        public void Configure()
        {
            ControllerFactory.CreateDependency = type => ObjectFactory.GetInstance(type);
        }
    }
}