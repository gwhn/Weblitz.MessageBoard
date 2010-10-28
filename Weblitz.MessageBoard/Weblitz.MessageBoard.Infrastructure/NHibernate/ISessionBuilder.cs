using NHibernate;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate
{
    public interface ISessionBuilder
    {
        ISession Construct();
    }
}