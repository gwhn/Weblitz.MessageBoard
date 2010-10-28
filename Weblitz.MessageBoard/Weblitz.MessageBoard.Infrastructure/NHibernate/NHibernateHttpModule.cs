using System;
using System.Web;
using NHibernate.Context;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate
{
    public class NHibernateHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
            context.EndRequest += EndRequest;
        }

        private static void BeginRequest(object sender, EventArgs e)
        {
            ManagedWebSessionContext.Bind(HttpContext.Current, SessionManager.SessionFactory.OpenSession());
        }

        private static void EndRequest(object sender, EventArgs e)
        {
            var session = ManagedWebSessionContext.Unbind(HttpContext.Current, SessionManager.SessionFactory);

            if (session == null) return;

            if (session.Transaction != null && session.Transaction.IsActive)
            {
                session.Transaction.Rollback();
            }
            else
            {
                session.Flush();
            }
            session.Close();
        }

        public void Dispose()
        {
        }
    }
}