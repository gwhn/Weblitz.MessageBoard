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
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments;
            topic.Forum = forum;

            // Act
            Persist(topic);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(topic);
        }

        [Test]
        public void ShouldPersistTopicWithOnePostAndNoAttachments()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments;
            topic.Forum = forum;
            var post = PostFixtures.RootPostWithNoChildren;
            topic.Add(post);

            // Act
            Persist(topic);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(topic);
        }

        [Test]
        public void ShouldPersistTopicWithNoPostAndOneAttachment()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments;
            topic.Forum = forum;
            var attachment = AttachmentFixtures.Attachment;
            topic.Add(attachment);

            // Act
            Persist(topic);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(topic);
        }

        [Test]
        public void ShouldPersistTopicWithOnePostAndOneAttachment()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments;
            topic.Forum = forum;
            var post = PostFixtures.RootPostWithNoChildren;
            topic.Add(post);
            var attachment = AttachmentFixtures.Attachment;
            topic.Add(attachment);

            // Act
            Persist(topic);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(topic);
        }
    }
}