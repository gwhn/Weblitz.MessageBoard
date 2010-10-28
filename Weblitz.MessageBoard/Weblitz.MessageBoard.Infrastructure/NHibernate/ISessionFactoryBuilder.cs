using NHibernate;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate
{
    public interface ISessionFactoryBuilder
    {
        ISessionFactory Construct();
    }
}