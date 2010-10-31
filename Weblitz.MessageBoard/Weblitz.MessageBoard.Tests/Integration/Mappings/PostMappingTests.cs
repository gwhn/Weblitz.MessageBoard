using System.Linq;
using NUnit.Framework;
using Weblitz.MessageBoard.Core.Domain.Model;
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
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
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
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
            topic.Forum = forum;
            Persist(topic);
            var root = PostFixtures.RootPostWithNoChildren;
            root.Topic = topic;
            Persist(root);
            var branch = PostFixtures.BranchPostWithNoChildren;
            branch.Topic = topic;
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
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
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
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
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
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
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

        [Test]
        public void ShouldDeleteOrphanBranchPostWhenRemovedFromRootPost()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
            topic.Forum = forum;
            Persist(topic);
            var root = PostFixtures.RootPostWithNoChildren;
            root.Topic = topic;
            var branch = PostFixtures.BranchPostWithNoChildren;
            root.Add(branch);
            Persist(root);
            var id = root.Id;

            // Act
            using (var s = BuildSession())
            {
                root = s.Load<Post>(id);
                root.Remove(root.Children[0]);
                s.SaveOrUpdate(root);
                s.Flush();
            }

            // Assert
            using (var s = BuildSession())
            {
                var actual = s.Load<Post>(id);

                Assert.That(actual, Is.EqualTo(root));
                Assert.That(actual, Is.Not.SameAs(root));

                var count = actual.Children.Count();
                Assert.That(count == 0);
                Assert.That(count, Is.EqualTo(root.Children.Count()));
            }
        }
    }
}