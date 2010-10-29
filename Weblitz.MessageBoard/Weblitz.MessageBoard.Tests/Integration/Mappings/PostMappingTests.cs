using NUnit.Framework;
using Weblitz.MessageBoard.Tests.Fixtures;

namespace Weblitz.MessageBoard.Tests.Integration.Mappings
{
    [TestFixture]
    public class PostMappingTests : DataTestBase
    {
        [Test]
        public void ShouldPersistRootPostWithNoChildrenAndNoAttachments()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments;
            topic.Forum = forum;
            Persist(topic);
            var root = PostFixtures.RootPostWithNoChildren;
            root.Topic = topic;

            // Act
            Persist(root);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(root);
        }

        [Test]
        public void ShouldPersistBranchPostWithNoChildrenAndNoAttachments()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments;
            topic.Forum = forum;
            Persist(topic);
            var root = PostFixtures.RootPostWithNoChildren;
            root.Topic = topic;
            Persist(root);
            var branch = PostFixtures.BranchPostWithNoChildren;
            branch.Parent = root;

            // Act
            Persist(branch);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(branch);
        }

        [Test]
        public void ShouldPersistRootPostWithOneChildAndNoAttachments()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments;
            topic.Forum = forum;
            Persist(topic);
            var root = PostFixtures.RootPostWithNoChildren;
            root.Topic = topic;
            var branch = PostFixtures.BranchPostWithNoChildren;
            root.Add(branch);

            // Act
            Persist(root);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(root);
        }

        [Test]
        public void ShouldPersistRootPostWithNoChildrenAndOneAttachment()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments;
            topic.Forum = forum;
            Persist(topic);
            var root = PostFixtures.RootPostWithNoChildren;
            root.Topic = topic;
            var attachment = AttachmentFixtures.Attachment;
            root.Add(attachment);

            // Act
            Persist(root);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(root);
        }

        [Test]
        public void ShouldPersistRootPostWithOneChildAndOneAttachment()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments;
            topic.Forum = forum;
            Persist(topic);
            var root = PostFixtures.RootPostWithNoChildren;
            root.Topic = topic;
            var branch = PostFixtures.BranchPostWithNoChildren;
            root.Add(branch);
            var attachment = AttachmentFixtures.Attachment;
            root.Add(attachment);

            // Act
            Persist(root);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(root);
        }

    }
}