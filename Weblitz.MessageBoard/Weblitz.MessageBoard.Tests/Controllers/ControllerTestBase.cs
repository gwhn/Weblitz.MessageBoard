using NHibernate;
using NHibernate.Context;
using NUnit.Framework;
using Weblitz.MessageBoard.Infrastructure.NHibernate;

namespace Weblitz.MessageBoard.Tests.Controllers
{
    [TestFixture]
    public class ControllerTestBase : TestBase
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var session = SessionManager.Instance.OpenSession();

            CallSessionContext.Bind(session);
        }

        [TestFixtureTearDown]
        public void TestFixtureTeardown()
        {
            var session = CallSessionContext.Unbind(SessionManager.Instance.SessionFactory);

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
            session.Dispose();
        }
    }
}