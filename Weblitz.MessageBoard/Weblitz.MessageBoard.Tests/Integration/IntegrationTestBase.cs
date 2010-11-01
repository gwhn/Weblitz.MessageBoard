using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate;
using NUnit.Framework;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Infrastructure.NHibernate.Builders;
using Weblitz.MessageBoard.Tests.Fixtures;

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
            SessionBuilder.GetDefault = () => null;
        }

        protected static ISession BuildSession()
        {
            return new SessionBuilder().Construct();
        }

        protected void SessionIs_(string action)
        {
            switch (action)
            {
                case Opened:
                    Session = BuildSession();
                    Assert.That(Session.IsOpen);
                    break;

                case Closed:
                    Session.Close();
                    Assert.That(!Session.IsOpen);
                    Session.Dispose();
                    break;
            }
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