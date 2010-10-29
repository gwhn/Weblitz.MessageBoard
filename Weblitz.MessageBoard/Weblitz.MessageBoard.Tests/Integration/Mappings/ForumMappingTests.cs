using NUnit.Framework;
using Weblitz.MessageBoard.Tests.Fixtures;

namespace Weblitz.MessageBoard.Tests.Integration.Mappings
{
    [TestFixture]
    public class ForumMappingTests : DataTestBase
    {
        [Test]
        public void ShouldPersistForumWithNoTopics()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;

            // Act
            Persist(forum);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(forum);
        }
    }
}