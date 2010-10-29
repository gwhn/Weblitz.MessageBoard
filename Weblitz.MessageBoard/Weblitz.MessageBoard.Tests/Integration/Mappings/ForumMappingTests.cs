using System.Reflection;
using NUnit.Framework;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Infrastructure.NHibernate.Builders;
using Weblitz.MessageBoard.Tests.Fixtures;

namespace Weblitz.MessageBoard.Tests.Integration.Mappings
{
    [TestFixture]
    public class ForumMappingTests : DataTestBase
    {
        [Test]
        public void ShouldPersistForum()
        {
            // Arrange
            var entity = ForumFixtures.ForumWithNoTopics;

            // Act
            using (var s = Session())
            {
                s.SaveOrUpdate(entity);
                s.Flush();
            }

            // Assert
            using (var s = Session())
            {
                var reloadedEntity = s.Load<Forum>(entity.Id);

                Assert.That(reloadedEntity, Is.EqualTo(entity));
                Assert.That(reloadedEntity, Is.Not.SameAs(entity));

                AssertObjectsMatch(entity, reloadedEntity);
            }
        }

        protected static void AssertObjectsMatch(object obj1, object obj2)
        {
            Assert.AreNotSame(obj1, obj2);
            Assert.AreEqual(obj1, obj2);

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