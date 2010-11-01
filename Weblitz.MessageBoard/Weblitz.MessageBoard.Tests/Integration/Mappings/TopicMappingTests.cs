using System;
using System.Linq;
using NUnit.Framework;
using StoryQ;
using StoryQ.Formatting.Parameters;
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

        [Test]
        public void TopicMapping()
        {
            new Story("topic mapping")
                .InOrderTo("check topic persistence")
                .AsA("developer")
                .IWant("to create, read, update and delete topics")

                        .WithScenario("create topic with no associated posts or attachments")
                            .Given(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedTopic_MatchSavedTopic, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("create topic with associated posts but no attachments")
                            .Given(TopicWith_PostsAnd_Attachments, 1, 0)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedTopic_MatchSavedTopic, true)
                                .And(Topic_ContainAddedPosts, true)
                                .And(AssociatedAttachments_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("create topic with associated attachments but no posts")
                            .Given(TopicWith_PostsAnd_Attachments, 0, 1)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedTopic_MatchSavedTopic, true)
                                .And(Topic_ContainAddedAttachments, true)
                                .And(AssociatedPosts_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("create topic with associated posts and attachments")
                            .Given(TopicWith_PostsAnd_Attachments, 3, 2)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedTopic_MatchSavedTopic, true)
                                .And(Topic_ContainAddedPosts, true)
                                .And(Topic_ContainAddedAttachments, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("update topic with no associated posts or attachments")
                            .Given(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(SessionIs_, Opened)
                                .And(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Loaded)
                                .And(TopicIs_, Modified)
                                .And(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedTopic_MatchSavedTopic, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("update topic with posts added")
                            .Given(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(SessionIs_, Opened)
                                .And(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Loaded)
                                .And(TopicIs_, Modified)
                                .And(_PostsAddedToTopic, 2)
                                .And(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedTopic_MatchSavedTopic, true)
                                .And(Topic_ContainAddedPosts, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("update topic with attachments added")
                            .Given(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(SessionIs_, Opened)
                                .And(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Loaded)
                                .And(TopicIs_, Modified)
                                .And(_AttachmentsAddedToTopic, 2)
                                .And(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedTopic_MatchSavedTopic, true)
                                .And(Topic_ContainAddedAttachments, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("update topic with posts removed")
                            .Given(TopicWith_PostsAnd_Attachments, 2, 0)
                                .And(SessionIs_, Opened)
                                .And(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Loaded)
                                .And(TopicIs_, Modified)
                                .And(_PostsRemovedFromTopic, 1)
                                .And(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedTopic_MatchSavedTopic, true)
                                .And(Topic_ContainRemovedPosts, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("update topic with attachments removed")
                            .Given(TopicWith_PostsAnd_Attachments, 0, 3)
                                .And(SessionIs_, Opened)
                                .And(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Loaded)
                                .And(TopicIs_, Modified)
                                .And(_AttachmentsRemovedFromTopic, 2)
                                .And(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedTopic_MatchSavedTopic, true)
                                .And(Topic_ContainRemovedAttachments, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("delete topic with no associated posts or attachments")
                            .Given(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(SessionIs_, Opened)
                                .And(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Deleted)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(Topic_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("delete forum with associated posts but no attachments")
                            .Given(TopicWith_PostsAnd_Attachments, 2, 0)
                                .And(SessionIs_, Opened)
                                .And(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Deleted)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(Topic_Exist, false)
                                .And(AssociatedPosts_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("delete forum with associated attachments but no posts")
                            .Given(TopicWith_PostsAnd_Attachments, 0, 3)
                                .And(SessionIs_, Opened)
                                .And(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Deleted)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(Topic_Exist, false)
                                .And(AssociatedAttachments_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("delete forum with associated attachments and posts")
                            .Given(TopicWith_PostsAnd_Attachments, 3, 2)
                                .And(SessionIs_, Opened)
                                .And(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Deleted)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(Topic_Exist, false)
                                .And(AssociatedPosts_Exist, false)
                                .And(AssociatedAttachments_Exist, false)
                                .And(SessionIs_, Closed)
                .Execute();
        }

        private void TopicWith_PostsAnd_Attachments(int postCount, int attachmentCount)
        {
            throw new NotImplementedException();
        }

        private void TopicIs_(string action)
        {
            throw new NotImplementedException();
        }

        private void LoadedTopic_MatchSavedTopic([BooleanParameterFormat("should", "should not")] bool matches)
        {
            throw new NotImplementedException();
        }

        private void Topic_ContainAddedPosts([BooleanParameterFormat("should", "should not")] bool contains)
        {
            throw new NotImplementedException();
        }

        private void AssociatedAttachments_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            throw new NotImplementedException();
        }

        private void Topic_ContainAddedAttachments([BooleanParameterFormat("should", "should not")] bool contains)
        {
            throw new NotImplementedException();
        }

        private void AssociatedPosts_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            throw new NotImplementedException();
        }

        private void _PostsAddedToTopic(int count)
        {
            throw new NotImplementedException();
        }

        private void _AttachmentsAddedToTopic(int count)
        {
            throw new NotImplementedException();
        }

        private void _PostsRemovedFromTopic(int count)
        {
            throw new NotImplementedException();
        }

        private void Topic_ContainRemovedPosts([BooleanParameterFormat("should", "should not")] bool contains)
        {
            throw new NotImplementedException();
        }

        private void _AttachmentsRemovedFromTopic(int count)
        {
            throw new NotImplementedException();
        }

        private void Topic_ContainRemovedAttachments([BooleanParameterFormat("should", "should not")] bool contains)
        {
            throw new NotImplementedException();
        }

        private void Topic_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            throw new NotImplementedException();
        }
    }
}