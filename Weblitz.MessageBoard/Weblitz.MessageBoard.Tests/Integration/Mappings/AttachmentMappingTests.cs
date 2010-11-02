using System;
using System.Collections.Generic;
using NUnit.Framework;
using StoryQ;
using StoryQ.Formatting.Parameters;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Tests.Fixtures;

namespace Weblitz.MessageBoard.Tests.Integration.Mappings
{
    [TestFixture]
    public class AttachmentMappingTests : DataTestBase
    {
        private Forum _forum;
        private Topic _topic;
        private Guid _forumId;
        private Attachment _attachment;
        private Guid _topicId;
        private Guid _attachmentId;
        private Post _post;
        private readonly IList<Post> _addedChildren = new List<Post>();
        private readonly IList<Attachment> _addedAttachments = new List<Attachment>();
        private Guid _postId;

        [Test]
        public void AttachmentMapping()
        {
            new Story("attachment mapping")
                .InOrderTo("check attachment persistence")
                .AsA("developer")
                .IWant("to create, read, update and delete attachments")

//                        .WithScenario("create attachment associated with topic")
//                            .Given(ForumWith_Topics, 0)
//                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
//                                .And(TopicAssociatedToForum)
//                                .And(Attachment)
//                                .And(AttachmentAssociatedToTopic)
//                                .And(SessionIs_, Opened)
//                            .When(AttachmentIs_, Saved)
//                                .And(SessionIs_, Closed)
//                                .And(SessionIs_, Opened)
//                            .Then(LoadedAttachment_MatchSavedAttachment, true)
//                                .And(AssociatedTopic_Exist, true)
//                                .And(SessionIs_, Closed)
//
//                        .WithScenario("create attachment associated with post")
//                            .Given(ForumWith_Topics, 0)
//                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
//                                .And(TopicAssociatedToForum)
//                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
//                                .And(PostAssociatedToTopic)
//                                .And(Attachment)
//                                .And(AttachmentAssociatedToPost)
//                                .And(SessionIs_, Opened)
//                            .When(AttachmentIs_, Saved)
//                                .And(SessionIs_, Closed)
//                                .And(SessionIs_, Opened)
//                            .Then(LoadedAttachment_MatchSavedAttachment, true)
//                                .And(AssociatedPost_Exist, true)
//                                .And(SessionIs_, Closed)

                        .WithScenario("update attachment associated with topic")
                            .Given(ForumWith_Topics, 0)
                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(TopicAssociatedToForum)
                                .And(Attachment)
                                .And(AttachmentAssociatedToTopic)
                                .And(SessionIs_, Opened)
                                .And(AttachmentIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(AttachmentIs_, Loaded)
                                .And(AttachmentIs_, Modified)
                                .And(AttachmentIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedAttachment_MatchSavedAttachment, true)
                                .And(AssociatedTopic_Exist, true)
                                .And(SessionIs_, Closed)

//                        .WithScenario("update attachment associated with post")
//                            .Given(ForumWith_Topics, 0)
//                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
//                                .And(TopicAssociatedToForum)
//                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
//                                .And(PostAssociatedToTopic)
//                                .And(Attachment)
//                                .And(AttachmentAssociatedToPost)
//                                .And(SessionIs_, Opened)
//                                .And(AttachmentIs_, Saved)
//                                .And(SessionIs_, Closed)
//                                .And(SessionIs_, Opened)
//                            .When(AttachmentIs_, Loaded)
//                                .And(AttachmentIs_, Modified)
//                                .And(AttachmentIs_, Saved)
//                                .And(SessionIs_, Closed)
//                                .And(SessionIs_, Opened)
//                            .Then(LoadedAttachment_MatchSavedAttachment, true)
//                                .And(AssociatedPost_Exist, true)
//                                .And(SessionIs_, Closed)
//
//                        .WithScenario("delete attachment associated with topic")
//                            .Given(ForumWith_Topics, 0)
//                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
//                                .And(TopicAssociatedToForum)
//                                .And(Attachment)
//                                .And(AttachmentAssociatedToTopic)
//                                .And(SessionIs_, Opened)
//                                .And(AttachmentIs_, Saved)
//                                .And(SessionIs_, Closed)
//                                .And(SessionIs_, Opened)
//                            .When(AttachmentIs_, Deleted)
//                                .And(SessionIs_, Closed)
//                                .And(SessionIs_, Opened)
//                            .Then(Attachment_Exist, false)
//                                .And(SessionIs_, Closed)
//
//                        .WithScenario("delete attachment associated with post")
//                            .Given(ForumWith_Topics, 0)
//                                .And(TopicWith_PostsAnd_Attachments, 0, 0)
//                                .And(TopicAssociatedToForum)
//                                .And(PostWith_ChildrenAnd_Attachments, 0, 0)
//                                .And(PostAssociatedToTopic)
//                                .And(Attachment)
//                                .And(AttachmentAssociatedToPost)
//                                .And(SessionIs_, Opened)
//                                .And(AttachmentIs_, Saved)
//                                .And(SessionIs_, Closed)
//                                .And(SessionIs_, Opened)
//                            .When(AttachmentIs_, Deleted)
//                                .And(SessionIs_, Closed)
//                                .And(SessionIs_, Opened)
//                            .Then(Attachment_Exist, false)
//                                .And(SessionIs_, Closed)
                .Execute();
        }

        private void PostAssociatedToTopic()
        {
            SessionIs_(Opened);
            _post.Topic = _topic;
            TopicIs_(Saved);
            SessionIs_(Closed);
        }

        private void Attachment()
        {
            _attachment = AttachmentFixtures.Attachment(1);
        }

        private void TopicAssociatedToForum()
        {
            SessionIs_(Opened);
            _topic.Forum = _forum;
            ForumIs_(Saved);
            SessionIs_(Closed);
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

        private void ForumWith_Topics(int count)
        {
            _forum = ForumFixtures.ForumWithNoTopics;
            for (var i = 0; i < count; i++)
            {
                _forum.Add(TopicFixtures.TopicWithNoPostsAndNoAttachments(i));
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

        private void AttachmentAssociatedToTopic()
        {
            SessionIs_(Opened);
            _attachment.Entry = _topic;
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

        private void AttachmentIs_(string action)
        {
            switch (action)
            {
                case Loaded:
                    _attachment = Session.Load<Attachment>(_attachmentId);
                    break;

                case Saved:
                    Session.SaveOrUpdate(_attachment);
                    Assert.That(Session.IsDirty());
                    Session.Flush();
                    _attachmentId = _attachment.Id;
                    break;

                case Modified:
                    _attachment.FileName = string.Format("/{0}/{1}", Modified, _attachment.FileName);
                    _attachment.ContentType = string.Format("{0}/{1}", Modified, _attachment.ContentType);
                    _attachment.ContentLength += 123;
                    break;

                case Deleted:
                    Session.Delete(_attachment);
                    Assert.That(Session.IsDirty());
                    Session.Flush();
                    break;
            }
        }

        private void LoadedAttachment_MatchSavedAttachment([BooleanParameterFormat("should", "should not")] bool matches)
        {
            var actual = Session.Load<Attachment>(_attachmentId);

            Assert.That(actual, Is.EqualTo(_attachment));
            Assert.That(actual, Is.Not.SameAs(_attachment));
        }

        private void AssociatedTopic_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            var actual = Session.Get<Topic>(_attachment.Entry.Id);

            if (exists)
            {
                Assert.IsNotNull(actual);
            }
            else
            {
                Assert.IsNull(actual);
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

        private void AttachmentAssociatedToPost()
        {
            SessionIs_(Opened);
            _attachment.Entry = _post;
            PostIs_(Saved);
            SessionIs_(Closed);
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

        private void AssociatedPost_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            var actual = Session.Get<Post>(_attachment.Entry.Id);

            if (exists)
            {
                Assert.IsNotNull(actual);
            }
            else
            {
                Assert.IsNull(actual);
            }
        }

        private void Attachment_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            var actual = Session.Get<Attachment>(_attachmentId);

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