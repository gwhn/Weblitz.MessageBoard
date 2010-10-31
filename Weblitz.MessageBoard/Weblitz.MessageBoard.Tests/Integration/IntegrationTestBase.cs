using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NHibernate;
using NUnit.Framework;
using Weblitz.MessageBoard.Infrastructure.NHibernate.Builders;

namespace Weblitz.MessageBoard.Tests.Integration
{
    [TestFixture]
    public class IntegrationTestBase : TestBase
    {
        protected ISession Session;

        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            SessionBuilder.GetDefault = () => null;
        }

        protected static ISession BuildSession()
        {
            return new SessionBuilder().Construct();
        }

        protected static void AssertObjectsMatch(object obj1, object obj2)
        {
            Assert.That(obj1, Is.EqualTo(obj2));
            Assert.That(obj1, Is.Not.SameAs(obj2));

            var infos = obj1.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var info in infos)
            {
                var value1 = info.GetValue(obj1, null);
                var value2 = info.GetValue(obj2, null);
                Assert.AreEqual(value1, value2, string.Format("Property {0} doesn't match", info.Name));
            }
        }
    }
}