using NHibernate;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate.Builders
{
    public interface ISessionFactoryBuilder
    {
        ISessionFactory Construct();
    }
}