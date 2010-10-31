using NUnit.Framework;
using Weblitz.MessageBoard.Tests.Fixtures;

namespace Weblitz.MessageBoard.Tests.Integration.Mappings
{
    [TestFixture]
    public class AttachmentMappingTests : DataTestBase
    {
        [Test]
        public void ShouldPersistTopicAttachment()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
            topic.Forum = forum;
            Persist(topic);
            var attachment = AttachmentFixtures.Attachment;
            attachment.Entry = topic;
            
            // Act
            Persist(attachment);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(attachment);
        }

        [Test]
        public void ShouldPersistPostAttachment()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
            topic.Forum = forum;
            Persist(topic);
            var post = PostFixtures.RootPostWithNoChildren;
            post.Topic = topic;
            Persist(post);
            var attachment = AttachmentFixtures.Attachment;
            attachment.Entry = post;
            
            // Act
            Persist(attachment);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(attachment);
        }
    }
}