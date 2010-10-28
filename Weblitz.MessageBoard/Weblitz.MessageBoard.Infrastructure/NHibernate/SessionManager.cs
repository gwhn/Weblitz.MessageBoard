using System;
using System.IO;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate
{
    public sealed class SessionManager
    {
        private static readonly SessionManager Manager = new SessionManager();

        private readonly ISessionFactory _sessionFactory;

        private SessionManager()
        {
            if (_sessionFactory == null)
            {
                Configuration configuration;
                if (AppDomain.CurrentDomain.BaseDirectory.Contains("Test"))
                {
                    configuration = new Configuration().Configure(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.config"));
                }
                else
                {
                    configuration = new Configuration().Configure(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "web.config"));
                }
                if (configuration == null)
                {
                    throw new InvalidOperationException("NHibernate configuration is null.");
                }
                configuration.AddAssembly(Assembly.GetExecutingAssembly());
                _sessionFactory = configuration.BuildSessionFactory();
                if (_sessionFactory == null)
                    throw new InvalidOperationException("Call to BuildSessionFactory() returned null.");
            }
        }

        public static SessionManager Instance
        {
            get { return Manager; }
        }

        public static ISessionFactory SessionFactory
        {
            get { return Instance._sessionFactory; }
        }

        public static ISession OpenSession()
        {
            return Instance._sessionFactory.OpenSession();
        }
    }
}