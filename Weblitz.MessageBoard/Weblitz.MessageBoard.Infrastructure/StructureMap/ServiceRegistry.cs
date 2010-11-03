using StructureMap.Configuration.DSL;
using Weblitz.MessageBoard.Core.Domain.Repositories;
using Weblitz.MessageBoard.Infrastructure.NHibernate;

namespace Weblitz.MessageBoard.Infrastructure.StructureMap
{
    public class ServiceRegistry : Registry
    {
        public ServiceRegistry()
        {
            For<IForumRepository>().Use<ForumRepository>();
        }
    }
}