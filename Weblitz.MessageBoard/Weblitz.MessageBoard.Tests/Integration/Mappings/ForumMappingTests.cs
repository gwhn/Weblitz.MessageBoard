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
                forum.Remove(forum.Topics[0]);
                s.SaveOrUpdate(forum);
                s.Flush();
            }

            // Assert
            using (var s = Session())
            {
                var actual = s.Load<Forum>(id);

                Assert.That(actual, Is.EqualTo(forum));
                Assert.That(actual, Is.Not.SameAs(forum));

                var count = actual.Topics.Count();
                Assert.That(count == 0);
                Assert.That(count, Is.EqualTo(forum.Topics.Count()));
            }
        }
    }
}