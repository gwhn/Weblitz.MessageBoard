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
            var entity = ForumFixtures.ForumWithNoTopics;

            // Act
            Persist(entity);

            // Assert
            AssertLoadedEntityMatch(entity);
        }
    }
}