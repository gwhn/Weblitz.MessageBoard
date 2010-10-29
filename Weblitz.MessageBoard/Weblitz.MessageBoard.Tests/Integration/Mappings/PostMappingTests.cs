using NUnit.Framework;
using Weblitz.MessageBoard.Tests.Fixtures;

namespace Weblitz.MessageBoard.Tests.Integration.Mappings
{
    [TestFixture]
    public class PostMappingTests : DataTestBase
    {
        [Test]
        public void ShouldPersistPostWithNoChildren()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments;
            topic.Forum = forum;
            Persist(topic);
            var entity = PostFixtures.PostWithNoChildren;
            entity.Topic = topic;

            // Act
            Persist(entity);

            // Assert
            AssertLoadedEntityMatch(entity);
        }
    }
}