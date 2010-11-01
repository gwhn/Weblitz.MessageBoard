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
            var root = PostFixtures.RootPostWithNoChildren(1);
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
            var root = PostFixtures.RootPostWithNoChildren(1);
            root.Topic = topic;
            Persist(root);
            var branch = PostFixtures.BranchPostWithNoChildren(1);
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
            var root = PostFixtures.RootPostWithNoChildren(1);
            root.Topic = topic;
            var branch = PostFixtures.BranchPostWithNoChildren(1);
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
            var root = PostFixtures.RootPostWithNoChildren(1);
            root.Topic = topic;
            var attachment = AttachmentFixtures.Attachment(1);
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
            var root = PostFixtures.RootPostWithNoChildren(1);
            root.Topic = topic;
            var branch = PostFixtures.BranchPostWithNoChildren(1);
            root.Add(branch);
            var attachment = AttachmentFixtures.Attachment(1);
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
            var root = PostFixtures.RootPostWithNoChildren(1);
            root.Topic = topic;
            var branch = PostFixtures.BranchPostWithNoChildren(1);
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

        [Test]
        public void PostMapping()
        {
            new Story("post mapping")
                .InOrderTo("check post persistence")
                .AsA("developer")
                .IWant("to create, read, update and delete posts")

                        .WithScenario("create post with no associated children or attachments")
                            .Given(PostWith_ChildrenAnd_Attachments, 0, 0)
                                .And(SessionIs_, Opened)
                            .When(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedPost_MatchSavedPost, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("create post with associated children but no attachments")
                            .Given(PostWith_ChildrenAnd_Attachments, 1, 0)
                                .And(SessionIs_, Opened)
                            .When(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedPost_MatchSavedPost, true)
                                .And(Post_ContainAddedChildren, true)
                                .And(AssociatedAttachments_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("create post with associated attachments but no children")
                            .Given(PostWith_ChildrenAnd_Attachments, 0, 1)
                                .And(SessionIs_, Opened)
                            .When(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedPost_MatchSavedPost, true)
                                .And(Post_ContainAddedAttachments, true)
                                .And(AssociatedChildren_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("create post with associated children and attachments")
                            .Given(PostWith_ChildrenAnd_Attachments, 3, 2)
                                .And(SessionIs_, Opened)
                            .When(PostIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedPost_MatchSavedPost, true)
                                .And(Post_ContainAddedChildren, true)
                                .And(Post_ContainAddedAttachments, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("update post with no associated children or attachments")
                            .Given(PostWith_ChildrenAnd_Attachments, 0, 0)
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
                            .Given(PostWith_ChildrenAnd_Attachments, 0, 0)
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
                            .Given(PostWith_ChildrenAnd_Attachments, 0, 0)
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
                            .Given(PostWith_ChildrenAnd_Attachments, 2, 0)
                                .And(SessionIs_, Opened)
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
                            .Given(PostWith_ChildrenAnd_Attachments, 0, 3)
                                .And(SessionIs_, Opened)
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
                            .Given(PostWith_ChildrenAnd_Attachments, 0, 0)
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
                            .Given(PostWith_ChildrenAnd_Attachments, 2, 0)
                                .And(SessionIs_, Opened)
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
                            .Given(PostWith_ChildrenAnd_Attachments, 0, 3)
                                .And(SessionIs_, Opened)
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
                            .Given(PostWith_ChildrenAnd_Attachments, 3, 2)
                                .And(SessionIs_, Opened)
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

        private void PostWith_ChildrenAnd_Attachments(int childCount, int attachmentCount)
        {
            throw new NotImplementedException();
        }

        private void PostIs_(string action)
        {
            throw new NotImplementedException();
        }

        private void LoadedPost_MatchSavedPost([BooleanParameterFormat("should", "should not")] bool matches)
        {
            throw new NotImplementedException();
        }

        private void Post_ContainAddedChildren([BooleanParameterFormat("should", "should not")] bool contains)
        {
            throw new NotImplementedException();
        }

        private void AssociatedAttachments_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            throw new NotImplementedException();
        }

        private void Post_ContainAddedAttachments([BooleanParameterFormat("should", "should not")] bool contains)
        {
            throw new NotImplementedException();
        }

        private void AssociatedChildren_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            throw new NotImplementedException();
        }

        private void _ChildrenAddedToPost(int count)
        {
            throw new NotImplementedException();
        }

        private void _AttachmentsAddedToPost(int count)
        {
            throw new NotImplementedException();
        }

        private void _ChildrenRemovedFromPost(int count)
        {
            throw new NotImplementedException();
        }

        private void Post_ContainRemovedChildren([BooleanParameterFormat("should", "should not")] bool contains)
        {
            throw new NotImplementedException();
        }

        private void _AttachmentsRemovedFromPost(int count)
        {
            throw new NotImplementedException();
        }

        private void Post_ContainRemovedAttachments([BooleanParameterFormat("should", "should not")] bool contains)
        {
            throw new NotImplementedException();
        }

        private void Post_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            throw new NotImplementedException();
        }
    }
}