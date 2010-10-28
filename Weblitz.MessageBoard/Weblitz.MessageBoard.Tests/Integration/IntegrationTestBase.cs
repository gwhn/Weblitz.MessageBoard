using NUnit.Framework;
using Weblitz.MessageBoard.Infrastructure.NHibernate.Builders;

namespace Weblitz.MessageBoard.Tests.Integration
{
    [TestFixture]
    public class IntegrationTestBase
    {
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            SessionBuilder.GetDefault = () => null;
        }
    }
}