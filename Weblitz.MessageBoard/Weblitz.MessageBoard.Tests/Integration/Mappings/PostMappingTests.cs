using NUnit.Framework;
using Weblitz.MessageBoard.Tests.Fixtures;

namespace Weblitz.MessageBoard.Tests.Integration.Mappings
{
    [TestFixture]
    public class PostMappingTests : DataTestBase
    {
        [Test]
        public void ShouldPersistRootPostWithNoChildren()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments;
            topic.Forum = forum;
            Persist(topic);
            var post = PostFixtures.RootPostWithNoChildren;
            post.Topic = topic;

            // Act
            Persist(post);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(post);
        }

        [Test]
        public void ShouldPersistBranchPostWithNoChildren()
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
    }
}