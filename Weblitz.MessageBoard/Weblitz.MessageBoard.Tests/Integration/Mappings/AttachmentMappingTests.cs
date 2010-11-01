using System;
using NUnit.Framework;
using StoryQ;
using StoryQ.Formatting.Parameters;
using Weblitz.MessageBoard.Tests.Fixtures;

namespace Weblitz.MessageBoard.Tests.Integration.Mappings
{
    [TestFixture]
    public class AttachmentMappingTests : DataTestBase
    {
        [Test]
        public void ShouldPersistTopicAttachment()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
            topic.Forum = forum;
            Persist(topic);
            var attachment = AttachmentFixtures.Attachment;
            attachment.Entry = topic;
            
            // Act
            Persist(attachment);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(attachment);
        }

        [Test]
        public void ShouldPersistPostAttachment()
        {
            // Arrange
            var forum = ForumFixtures.ForumWithNoTopics;
            Persist(forum);
            var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
            topic.Forum = forum;
            Persist(topic);
            var post = PostFixtures.RootPostWithNoChildren;
            post.Topic = topic;
            Persist(post);
            var attachment = AttachmentFixtures.Attachment;
            attachment.Entry = post;
            
            // Act
            Persist(attachment);

            // Assert
            AssertPersistedEntityMatchesLoadedEntity(attachment);
        }

        [Test]
        public void AttachmentMapping()
        {
            new Story("attachment mapping")
                .InOrderTo("check attachment persistence")
                .AsA("developer")
                .IWant("to create, read, update and delete attachments")

                        .WithScenario("create attachment associated with topic")
                            .Given(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(AttachmentAssociatedToTopic)
                                .And(SessionIs_, Opened)
                            .When(AttachmentIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedAttachment_MatchSavedAttachment, true)
                                .And(AssociatedTopic_Exist, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("create attachment associated with post")
                            .Given(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(AttachmentAssociatedToPost)
                                .And(SessionIs_, Opened)
                            .When(AttachmentIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedAttachment_MatchSavedAttachment, true)
                                .And(AssociatedPost_Exist, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("update attachment associated with topic")
                            .Given(TopicWith_PostsAnd_Attachments, 0, 0)
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

                        .WithScenario("update attachment associated with post")
                            .Given(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(AttachmentAssociatedToPost)
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
                                .And(AssociatedPost_Exist, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("delete attachment associated with topic")
                            .Given(TopicWith_PostsAnd_Attachments, 0, 0)
                                .And(AttachmentAssociatedToTopic)
                                .And(SessionIs_, Opened)
                                .And(AttachmentIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(AttachmentIs_, Deleted)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(Attachment_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("delete attachment associated with post")
                            .Given(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(AttachmentAssociatedToPost)
                                .And(SessionIs_, Opened)
                                .And(AttachmentIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(AttachmentIs_, Deleted)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(Attachment_Exist, false)
                                .And(SessionIs_, Closed)
                .Execute();
        }

        private void TopicWith_PostsAnd_Attachments(int postCount, int attachmentCount)
        {
            throw new NotImplementedException();
        }

        private void AttachmentAssociatedToTopic()
        {
            throw new NotImplementedException();
        }

        private void AttachmentIs_(string action)
        {
            throw new NotImplementedException();
        }

        private void LoadedAttachment_MatchSavedAttachment([BooleanParameterFormat("should", "should not")] bool matches)
        {
            throw new NotImplementedException();
        }

        private void AssociatedTopic_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            throw new NotImplementedException();
        }

        private void PostWith_ChildrenAnd_Attachments(int childCount, int attachmentCount)
        {
            throw new NotImplementedException();
        }

        private void AttachmentAssociatedToPost()
        {
            throw new NotImplementedException();
        }

        private void AssociatedPost_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            throw new NotImplementedException();
        }

        private void Attachment_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            throw new NotImplementedException();
        }
    }
}