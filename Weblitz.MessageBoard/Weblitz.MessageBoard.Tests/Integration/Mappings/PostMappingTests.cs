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
    public class PostMappingTests : DataTestBase
    {
        private Forum _forum;
        private Guid _forumId;
        private Topic _topic;
        private Post _post;
        private Guid _topicId;
        private Guid _postId;
        private readonly IList<Post> _addedChildren = new List<Post>();
        private readonly IList<Attachment> _addedAttachments = new List<Attachment>();
        private readonly IList<Post> _removedChildren = new List<Post>();
        private readonly IList<Attachment> _removedAttachments = new List<Attachment>();

        [Test]
        public void PostMapping()
        {
            new Story("post mapping")
                .InOrderTo("check post persistence")
                .AsA("developer")
                .IWant("to create, read, update and delete posts")

                        .WithScenario("create post with no associated children or attachments")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(PostAssociatedToTopic)
                                .And(SessionIs_, Opened)
                            .When(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedPost_MatchSavedPost, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("create post with associated children but no attachments")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(PostAssociatedToTopic)
                                .And(SessionIs_, Opened)
                            .When(_ChildrenAddedToPost, 1)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedPost_MatchSavedPost, true)
                                .And(Post_ContainAddedChildren, true)
                                .And(AssociatedAttachments_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("create post with associated attachments but no children")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(PostAssociatedToTopic)
                                .And(SessionIs_, Opened)
                            .When(_AttachmentsAddedToPost, 2)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedPost_MatchSavedPost, true)
                                .And(Post_ContainAddedAttachments, true)
                                .And(AssociatedChildren_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("create post with associated children and attachments")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(PostAssociatedToTopic)
                                .And(SessionIs_, Opened)
                            .When(_ChildrenAddedToPost, 3)
                                .And(_AttachmentsAddedToPost, 2)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedPost_MatchSavedPost, true)
                                .And(Post_ContainAddedChildren, true)
                                .And(Post_ContainAddedAttachments, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("update post with no associated children or attachments")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(PostAssociatedToTopic)
                                .And(SessionIs_, Opened)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(PostIs_, Loaded)
                                .And(PostIs_, Modified)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedPost_MatchSavedPost, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("update post with children added")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(PostAssociatedToTopic)
                                .And(SessionIs_, Opened)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(PostIs_, Loaded)
                                .And(PostIs_, Modified)
                                .And(_ChildrenAddedToPost, 2)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedPost_MatchSavedPost, true)
                                .And(Post_ContainAddedChildren, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("update post with attachments added")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(PostAssociatedToTopic)
                                .And(SessionIs_, Opened)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(PostIs_, Loaded)
                                .And(PostIs_, Modified)
                                .And(_AttachmentsAddedToPost, 2)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedPost_MatchSavedPost, true)
                                .And(Post_ContainAddedAttachments, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("update post with children removed")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(PostAssociatedToTopic)
                                .And(SessionIs_, Opened)
                                .And(_ChildrenAddedToPost, 2)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(PostIs_, Loaded)
                                .And(PostIs_, Modified)
                                .And(_ChildrenRemovedFromPost, 1)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedPost_MatchSavedPost, true)
                                .And(Post_ContainRemovedChildren, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("update post with attachments removed")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(PostAssociatedToTopic)
                                .And(SessionIs_, Opened)
                                .And(_AttachmentsAddedToPost, 3)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(PostIs_, Loaded)
                                .And(PostIs_, Modified)
                                .And(_AttachmentsRemovedFromPost, 2)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedPost_MatchSavedPost, true)
                                .And(Post_ContainRemovedAttachments, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("delete post with no associated children or attachments")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(PostAssociatedToTopic)
                                .And(SessionIs_, Opened)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(PostIs_, Deleted)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(Post_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("delete post with associated children but no attachments")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(PostAssociatedToTopic)
                                .And(SessionIs_, Opened)
                                .And(_ChildrenAddedToPost, 2)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(PostIs_, Deleted)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(Post_Exist, false)
                                .And(AssociatedChildren_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("delete post with associated attachments but no children")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(PostAssociatedToTopic)
                                .And(SessionIs_, Opened)
                                .And(_AttachmentsAddedToPost, 3)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(PostIs_, Deleted)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(Post_Exist, false)
                                .And(AssociatedAttachments_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("delete post with associated children and attachments")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(PostAssociatedToTopic)
                                .And(SessionIs_, Opened)
                                .And(_ChildrenAddedToPost, 3)
                                .And(_AttachmentsAddedToPost, 2)
                                .And(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(PostIs_, Deleted)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(Post_Exist, false)
                                .And(AssociatedChildren_Exist, false)
                                .And(AssociatedAttachments_Exist, false)
                                .And(SessionIs_, Closed)
                .Execute();
        }

        private void ForumWith_Topics(int count)
        {
            _forum = ForumFixtures.ForumWithNoTopics;
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

        private void PostAssociatedToTopic()
        {
            SessionIs_(Opened);
            _post.Topic = _topic;
            TopicIs_(Saved);
            SessionIs_(Closed);
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

        private void TopicWith_PostsAnd_Attachments(int postCount, int attachmentCount)
        {
            _topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
            for (var i = 0; i < postCount; i++)
            {
                _topic.Add(PostFixtures.RootPostWithNoChildren(i));
            }
            for (var i = 0; i < attachmentCount; i++)
            {
                _topic.Add(AttachmentFixtures.Attachment(i));
            }
        }

        private void PostWith_ChildrenAnd_Attachments(int childCount, int attachmentCount)
        {
            _post = PostFixtures.RootPostWithNoChildren(1);
            if (childCount > 0)
            {
                _ChildrenAddedToPost(childCount);
            }
            if (attachmentCount > 0)
            {
                _AttachmentsAddedToPost(attachmentCount);
            }
        }

        private void PostIs_(string action)
        {
            switch (action)
            {
                case Loaded:
                    _post = Session.Load<Post>(_postId);
                    break;

                case Saved:
                    Session.SaveOrUpdate(_post);
                    Assert.That(Session.IsDirty());
                    Session.Flush();
                    _postId = _post.Id;
                    break;

                case Modified:
                    _post.Body = string.Format("{0} {1}", _post.Body, Modified);
                    _post.Flagged = !_post.Flagged;
                    break;

                case Deleted:
                    Session.Delete(_post);
                    Assert.That(Session.IsDirty());
                    Session.Flush();
                    break;
            }
        }

        private void LoadedPost_MatchSavedPost([BooleanParameterFormat("should", "should not")] bool matches)
        {
            var actual = Session.Load<Post>(_postId);

            Assert.That(actual, Is.EqualTo(_post));
            Assert.That(actual, Is.Not.SameAs(_post));
        }

        private void Post_ContainAddedChildren([BooleanParameterFormat("should", "should not")] bool contains)
        {
            var actual = Session.Get<Post>(_postId);

            foreach (var child in actual.Children)
            {
                if (contains)
                {
                    Assert.IsTrue(_addedChildren.Contains(child));
                }
                else
                {
                    Assert.IsFalse(_addedChildren.Contains(child));
                }
            }
        }

        private void AssociatedAttachments_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            foreach (var attachment in _post.Attachments)
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

        private void Post_ContainAddedAttachments([BooleanParameterFormat("should", "should not")] bool contains)
        {
            var actual = Session.Get<Post>(_postId);

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

        private void AssociatedChildren_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            foreach (var child in _post.Children)
            {
                var actual = Session.Get<Post>(child.Id);

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

        private void _ChildrenAddedToPost(int count)
        {
            Assert.That(count > 0);
            Assert.IsNotNull(_post);

            for (var i = 0; i < count; i++)
            {
                var child = PostFixtures.BranchPostWithNoChildren(i);
                _addedChildren.Add(child);
                _post.Add(child);
            }
        }

        private void _AttachmentsAddedToPost(int count)
        {
            Assert.That(count > 0);
            Assert.IsNotNull(_post);

            for (var i = 0; i < count; i++)
            {
                var attachment = AttachmentFixtures.Attachment(i);
                _addedAttachments.Add(attachment);
                _post.Add(attachment);
            }
        }

        private void _ChildrenRemovedFromPost(int count)
        {
            Assert.That(count <= _post.Children.Count());

            for (var i = 0; i < count; i++)
            {
                var child = _post.Children[0];
                _removedChildren.Add(child);
                _post.Remove(child);
            }
        }

        private void Post_ContainRemovedChildren([BooleanParameterFormat("should", "should not")] bool contains)
        {
            var actual = Session.Get<Post>(_postId);

            foreach (var child in actual.Children)
            {
                if (contains)
                {
                    Assert.IsTrue(_removedChildren.Contains(child));
                }
                else
                {
                    Assert.IsFalse(_removedChildren.Contains(child));
                }
            }
        }

        private void _AttachmentsRemovedFromPost(int count)
        {
            Assert.That(count <= _post.Attachments.Count());

            for (var i = 0; i < count; i++)
            {
                var attachment = _post.Attachments[0];
                _removedAttachments.Add(attachment);
                _post.Remove(attachment);
            }
        }

        private void Post_ContainRemovedAttachments([BooleanParameterFormat("should", "should not")] bool contains)
        {
            var actual = Session.Get<Post>(_postId);

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

        private void Post_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            var actual = Session.Get<Post>(_postId);

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