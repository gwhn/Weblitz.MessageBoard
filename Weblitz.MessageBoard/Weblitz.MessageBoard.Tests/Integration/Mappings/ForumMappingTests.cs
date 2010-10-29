using System.Linq;
using NUnit.Framework;
using Weblitz.MessageBoard.Core.Domain.Model;
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

        [Test]
        public void ShouldPersistForumWithOneTopic()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments;
            forum.Add(topic);

            // Act
            Persist(forum);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(forum);
        }

        [Test]
        public void ShouldDeleteOrphanTopicWhenRemovedFromForum()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments;
            forum.Add(topic);
            Persist(forum);
            var id = forum.Id;

            // Act
            using (var s = Session())
            {
                forum = s.Load<Forum>(id);
                foreach (var t in forum.Topics.ToList()) forum.Remove(t);
                s.SaveOrUpdate(forum);
                s.Flush();
            }

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(forum);
        }


    }
}