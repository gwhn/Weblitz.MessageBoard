using System.Linq;
using NUnit.Framework;
using Weblitz.MessageBoard.Core.Domain.Model;
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
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
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
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
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
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
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
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
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

        [Test]
        public void ShouldDeleteOrphanPostWhenRemovedFromTopic()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
            topic.Forum = forum;
            var post = PostFixtures.RootPostWithNoChildren;
            topic.Add(post);
            Persist(topic);
            var id = topic.Id;

            // Act
            using (var s = BuildSession())
            {
                topic = s.Load<Topic>(id);
                topic.Remove(topic.Posts[0]);
                s.SaveOrUpdate(topic);
                s.Flush();
            }

            // Assert
            using (var s = BuildSession())
            {
                var actual = s.Load<Topic>(id);

                Assert.That(actual, Is.EqualTo(topic));
                Assert.That(actual, Is.Not.SameAs(topic));

                var count = actual.Posts.Count();
                Assert.That(count == 0);
                Assert.That(count, Is.EqualTo(topic.Posts.Count()));
            }
        }

        [Test]
        public void ShouldDeleteOrphanAttachmentWhenRemovedFromTopic()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
            topic.Forum = forum;
            var attachment = AttachmentFixtures.Attachment;
            topic.Add(attachment);
            Persist(topic);
            var id = topic.Id;

            // Act
            using (var s = BuildSession())
            {
                topic = s.Load<Topic>(id);
                topic.Remove(topic.Attachments[0]);
                s.SaveOrUpdate(topic);
                s.Flush();
            }

            // Assert
            using (var s = BuildSession())
            {
                var actual = s.Load<Topic>(id);

                Assert.That(actual, Is.EqualTo(topic));
                Assert.That(actual, Is.Not.SameAs(topic));

                var count = actual.Attachments.Count();
                Assert.That(count == 0);
                Assert.That(count, Is.EqualTo(topic.Attachments.Count()));
            }
        }

    }
}