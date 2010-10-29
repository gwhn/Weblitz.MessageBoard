using System.Linq;
using System.Reflection;
using NHibernate;
using NUnit.Framework;
using Weblitz.MessageBoard.Infrastructure.NHibernate.Builders;

namespace Weblitz.MessageBoard.Tests.Integration
{
    [TestFixture]
    public class IntegrationTestBase : TestBase
    {
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            SessionBuilder.GetDefault = () => null;
        }

        protected static ISession Session()
        {
            return new SessionBuilder().Construct();
        }
    }
}