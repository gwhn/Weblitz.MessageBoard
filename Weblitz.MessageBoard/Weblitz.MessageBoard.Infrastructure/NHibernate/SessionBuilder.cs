using System;
using NHibernate;
using Weblitz.MessageBoard.Core;
using Weblitz.MessageBoard.Infrastructure.NHibernate.Scopes;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate
{
    public class SessionBuilder : AbstractFactoryBase<EmptyInterceptor>, ISessionBuilder
    {
        private const string NhibernateSession = "NHibernate.ISession";

        private readonly HybridInstanceScoper<ISession> _scoper;
        private readonly ISessionFactoryBuilder _builder;

        public static Func<EmptyInterceptor> GetDefault = DefaultUnconfiguredState;

        public SessionBuilder()
        {
            _scoper = new HybridInstanceScoper<ISession>();
            _builder = new SessionFactoryBuilder();
        }

        public ISession Construct()
        {
            var instance = GetScopedInstance();
            if (!instance.IsOpen)
            {
                _scoper.ClearScopedInstance(NhibernateSession);
                return GetScopedInstance();
            }
            return instance;
        }

        private ISession GetScopedInstance()
        {
            return _scoper.GetScopedInstance(NhibernateSession,
                                             () =>
                                                 {
                                                     var interceptor = GetDefault();
                                                     var factory = _builder.Construct();
                                                     var session = interceptor == null
                                                                       ? factory.OpenSession()
                                                                       : factory.OpenSession(interceptor);
                                                     session.FlushMode = FlushMode.Commit;
                                                     return session;
                                                 });
        }
    }
}