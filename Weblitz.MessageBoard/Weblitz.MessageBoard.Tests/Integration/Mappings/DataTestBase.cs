using NUnit.Framework;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Tests.Integration.Mappings
{
    [TestFixture]
    public class DataTestBase : IntegrationTestBase
    {
        protected static void Persist<T>(T entity) where T : Entity
        {
            using (var session = BuildSession())
            {
                session.SaveOrUpdate(entity);
                session.Flush();
            }
        }

        protected static void AssertPersistedEntityMatchesLoadedEntity<T>(T entity) where T : Entity
        {
            using (var session = BuildSession())
            {
                var actual = session.Load<T>(entity.Id);

                AssertObjectsMatch(entity, actual);
            }
        }

    }
}