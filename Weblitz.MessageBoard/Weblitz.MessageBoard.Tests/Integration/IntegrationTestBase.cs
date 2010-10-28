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

        protected ISession Session
        {
            get { return new SessionBuilder().Construct(); }
        }

        protected static bool ObjectsMatch(object obj1, object obj2)
        {
            var infos = obj1.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            return !(from info in infos
                     let value1 = info.GetValue(obj1, null)
                     let value2 = info.GetValue(obj2, null)
                     where !value1.Equals(value2)
                     select value1).Any();
        }
    }
}