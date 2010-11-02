using System;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate
{
    public sealed class SessionManager
    {
        private static readonly SessionManager Manager = new SessionManager();

        private readonly ISessionFactory _factory;

        private SessionManager()
        {
            _factory = new Configuration()
                .Configure()
                .AddAssembly(Assembly.GetExecutingAssembly())
                .SetInterceptor(new AuditInterceptor())
                .BuildSessionFactory();
        }

        public static SessionManager Instance
        {
            get { return Manager; }
        }

        public ISessionFactory SessionFactory
        {
            get { return _factory; }
        }

        public ISession OpenSession()
        {
            return _factory.OpenSession();
        }
    }
}