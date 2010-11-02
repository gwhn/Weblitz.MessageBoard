using NHibernate;
using NUnit.Framework;
using Weblitz.MessageBoard.Infrastructure.NHibernate;

namespace Weblitz.MessageBoard.Tests.Integration
{
    [TestFixture]
    public class IntegrationTestBase : TestBase
    {
        protected ISession Session;

        protected const string Opened = "Opened";
        protected const string Closed = "Closed";
        protected const string Loaded = "Loaded";
        protected const string Saved = "Saved";
        protected const string Modified = "Modified";
        protected const string Deleted = "Deleted";

        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
        }

        [TestFixtureTearDown]
        public virtual void FixtureTearDown()
        {
        }

        protected static ISession OpenSession()
        {
            return SessionManager.Instance.OpenSession();
        }

        protected void SessionIs_(string action)
        {
            switch (action)
            {
                case Opened:
                    Session = OpenSession();
                    Assert.That(Session.IsOpen);
                    break;

                case Closed:
                    Session.Close();
                    Assert.That(!Session.IsOpen);
                    Session.Dispose();
                    break;
            }
        }
    }
}