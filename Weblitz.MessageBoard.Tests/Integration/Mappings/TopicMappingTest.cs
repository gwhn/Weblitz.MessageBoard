using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using StoryQ;
using StoryQ.Formatting.Parameters;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Tests.Fixtures;

namespace Weblitz.MessageBoard.Tests.Integration.Mappings
{
    [TestFixture]
    public class TopicMappingTest : DataTestBase
    {
        private Forum _forum;
        private Topic _topic;
        private readonly IList<Post> _addedPosts = new List<Post>();
        private readonly IList<Attachment> _addedAttachments = new List<Attachment>();
        private readonly IList<Post> _removedPosts = new List<Post>();
        private readonly IList<Attachment> _removedAttachments = new List<Attachment>();
        private Guid _topicId;
        private Guid _forumId;

        [Test]
        public void TopicMapping()
        {
            new Story("topic mapping")
                .InOrderTo("check topic persistence")
                .AsA("developer")
                .IWant("to create, read, update and delete topics")

                        .WithScenario("create topic with no associated posts or attachments")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedTopic_MatchSavedTopic, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("create topic with associated posts but no attachments")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 1, 0)
                                .And(TopicAssociatedToForum)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedTopic_MatchSavedTopic, true)
                                .And(Topic_ContainAddedPosts, true)
                                .And(AssociatedAttachments_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("create topic with associated attachments but no posts")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 1)
                                .And(TopicAssociatedToForum)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedTopic_MatchSavedTopic, true)
                                .And(Topic_ContainAddedAttachments, true)
                                .And(AssociatedPosts_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("create topic with associated posts and attachments")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 3, 2)
                                .And(TopicAssociatedToForum)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedTopic_MatchSavedTopic, true)
                                .And(Topic_ContainAddedPosts, true)
                                .And(Topic_ContainAddedAttachments, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("update topic with no associated posts or attachments")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
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
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
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
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
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
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 2, 0)
                                .And(TopicAssociatedToForum)
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
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 3)
                                .And(TopicAssociatedToForum)
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
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(SessionIs_, Opened)
                                .And(TopicIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(TopicIs_, Deleted)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(Topic_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("delete topic with associated posts but no attachments")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 2, 0)
                                .And(TopicAssociatedToForum)
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

                        .WithScenario("delete topic with associated attachments but no posts")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 3)
                                .And(TopicAssociatedToForum)
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

                        .WithScenario("delete topic with associated posts and attachments")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 3, 2)
                                .And(TopicAssociatedToForum)
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

        private void ForumWith_Topics(int count)
        {
            _forum = ForumFixtures.ForumWithNoTopics(1);
            for (var i = 0; i < count; i++)
            {
                _forum.Add(TopicFixtures.TopicWithNoPostsAndNoAttachments(i));
            }
        }

        private void ForumIs_(string action)
        {
            switch (action)
            {
                case Loaded:
                    _forum = Session.Load<Forum>(_forumId);
                    break;

                case Saved:
                    Session.SaveOrUpdate(_forum);
                    Assert.That(Session.IsDirty());
                    Session.Flush();
                    _forumId = _forum.Id;
                    break;

                case Modified:
                    _forum.Name = string.Format("{0} {1}", _forum.Name, Modified);
                    break;

                case Deleted:
                    Session.Delete(_forum);
                    Assert.That(Session.IsDirty());
                    Session.Flush();
                    break;
            }
        }
        
        private void TopicAssociatedToForum()
        {
            SessionIs_(Opened);
            _topic.Forum = _forum;
            ForumIs_(Saved);
            SessionIs_(Closed);
        }

        private void TopicWith_PostsAnd_Attachments(int postCount, int attachmentCount)
        {
            _topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
            if (postCount > 0) 
            {
                _PostsAddedToTopic(postCount);
            }
            if (attachmentCount > 0)
            {
                _AttachmentsAddedToTopic(attachmentCount);
            }
        }

        private void TopicIs_(string action)
        {
            switch (action)
            {
                case Loaded:
                    _topic = Session.Load<Topic>(_topicId);
                    break;

                case Saved:
                    Session.SaveOrUpdate(_topic);
                    Assert.That(Session.IsDirty());
                    Session.Flush();
                    _topicId = _topic.Id;
                    break;

                case Modified:
                    _topic.Title = string.Format("{0} {1}", _topic.Title, Modified);
                    _topic.Body = string.Format("{0} {1}", _topic.Body, Modified);
                    _topic.Closed = !_topic.Closed;
                    _topic.Sticky = !_topic.Sticky;
                    break;

                case Deleted:
                    Session.Delete(_topic);
                    Assert.That(Session.IsDirty());
                    Session.Flush();
                    break;
            }
        }

        private void LoadedTopic_MatchSavedTopic([BooleanParameterFormat("should", "should not")] bool matches)
        {
            var actual = Session.Load<Topic>(_topicId);

            Assert.That(actual, Is.EqualTo(_topic));
            Assert.That(actual, Is.Not.SameAs(_topic));
        }

        private void Topic_ContainAddedPosts([BooleanParameterFormat("should", "should not")] bool contains)
        {
            var actual = Session.Get<Topic>(_topicId);

            foreach (var post in actual.Posts)
            {
                if (contains)
                {
                    Assert.IsTrue(_addedPosts.Contains(post));
                }
                else
                {
                    Assert.IsFalse(_addedPosts.Contains(post));
                }
            }
        }

        private void AssociatedAttachments_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            foreach (var attachment in _topic.Attachments)
            {
                var actual = Session.Get<Attachment>(attachment.Id);

                if (exists)
                {
                    Assert.IsNotNull(actual);
                }
                else
                {
                    Assert.IsNull(actual);
                }
            }
        }

        private void Topic_ContainAddedAttachments([BooleanParameterFormat("should", "should not")] bool contains)
        {
            var actual = Session.Get<Topic>(_topicId);

            foreach (var attachment in actual.Attachments)
            {
                if (contains)
                {
                    Assert.IsTrue(_addedAttachments.Contains(attachment));
                }
                else
                {
                    Assert.IsFalse(_addedAttachments.Contains(attachment));
                }
            }
        }

        private void AssociatedPosts_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            foreach (var post in _topic.Posts)
            {
                var actual = Session.Get<Post>(post.Id);

                if (exists)
                {
                    Assert.IsNotNull(actual);
                }
                else
                {
                    Assert.IsNull(actual);
                }
            }
        }

        private void _PostsAddedToTopic(int count)
        {
            Assert.That(count > 0);
            Assert.IsNotNull(_topic);

            for (var i = 0; i < count; i++)
            {
                var post = PostFixtures.RootPostWithNoChildren(i);
                _addedPosts.Add(post);
                _topic.Add(post);
            }
        }

        private void _AttachmentsAddedToTopic(int count)
        {
            Assert.That(count > 0);
            Assert.IsNotNull(_topic);

            for (var i = 0; i < count; i++)
            {
                var attachment = AttachmentFixtures.Attachment(i);
                _addedAttachments.Add(attachment);
                _topic.Add(attachment);
            }
        }

        private void _PostsRemovedFromTopic(int count)
        {
            Assert.That(count <= _topic.Posts.Count());

            for (var i = 0; i < count; i++)
            {
                var post = _topic.Posts[0];
                _removedPosts.Add(post);
                _topic.Remove(post);
            }
        }

        private void Topic_ContainRemovedPosts([BooleanParameterFormat("should", "should not")] bool contains)
        {
            var actual = Session.Get<Topic>(_topicId);

            foreach (var post in actual.Posts)
            {
                if (contains)
                {
                    Assert.IsTrue(_removedPosts.Contains(post));
                }
                else
                {
                    Assert.IsFalse(_removedPosts.Contains(post));
                }
            }
        }

        private void _AttachmentsRemovedFromTopic(int count)
        {
            Assert.That(count <= _topic.Attachments.Count());

            for (var i = 0; i < count; i++)
            {
                var attachment = _topic.Attachments[0];
                _removedAttachments.Add(attachment);
                _topic.Remove(attachment);
            }
        }

        private void Topic_ContainRemovedAttachments([BooleanParameterFormat("should", "should not")] bool contains)
        {
            var actual = Session.Get<Topic>(_topicId);

            foreach (var attachment in actual.Attachments)
            {
                if (contains)
                {
                    Assert.IsTrue(_removedAttachments.Contains(attachment));
                }
                else
                {
                    Assert.IsFalse(_removedAttachments.Contains(attachment));
                }
            }
        }

        private void Topic_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            var actual = Session.Get<Topic>(_topicId);

            if (exists)
            {
                Assert.IsNotNull(actual);
            }
            else
            {
                Assert.IsNull(actual);
            }
        }
    }
}