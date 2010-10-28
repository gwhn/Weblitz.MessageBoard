using NHibernate;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate.Builders
{
    public interface ISessionBuilder
    {
        ISession Construct();
    }
}