using NUnit.Framework;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Tests.Integration.Mappings
{
    [TestFixture]
    public class DataTestBase : IntegrationTestBase
    {
        protected static void Persist<T>(T entity) where T : Entity
        {
            using (var s = Session())
            {
                s.SaveOrUpdate(entity);
                s.Flush();
            }
        }

        protected static void AssertPersistedEntityMatchesLoadedEntity<T>(T entity) where T : Entity
        {
            using (var s = Session())
            {
                var actual = s.Load<T>(entity.Id);

                Assert.That(actual, Is.EqualTo(entity));
                Assert.That(actual, Is.Not.SameAs(entity));

                AssertObjectsMatch(entity, actual);
            }
        }

    }
}