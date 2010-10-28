using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using Weblitz.MessageBoard.Infrastructure.NHibernate.Scopes;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate
{
    public class SessionFactoryBuilder : ISessionFactoryBuilder
    {
        private const string SessionFactory = "SessionFactory";

        private readonly SingletonInstanceScoper<ISessionFactory> _sessionFactorySingleton =
            new SingletonInstanceScoper<ISessionFactory>();

        public ISessionFactory Construct()
        {
            return _sessionFactorySingleton
                .GetScopedInstance(SessionFactory,
                                   () => new Configuration()
                                             .Configure()
                                             .AddAssembly(Assembly.GetExecutingAssembly())
                                             .BuildSessionFactory());
        }
    }
}