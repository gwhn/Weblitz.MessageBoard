using NUnit.Framework;
using Weblitz.MessageBoard.Tests.Fixtures;

namespace Weblitz.MessageBoard.Tests.Integration.Mappings
{
    [TestFixture]
    public class TopicMappingTests : DataTestBase
    {
        [Test]
        public void ShouldPersistTopicWithNoPostsAndNoAttachments()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var entity = TopicFixtures.TopicWithNoPostsAndNoAttachments;
            entity.Forum = forum;

            // Act
            Persist(entity);

            // Assert
            AssertLoadedEntityMatch(entity);
        }
    }
}