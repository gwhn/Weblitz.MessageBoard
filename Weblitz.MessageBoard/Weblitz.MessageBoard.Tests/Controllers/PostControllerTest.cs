using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Rhino.Mocks;
using StoryQ;
using StoryQ.Formatting.Parameters;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Core.Domain.Repositories;
using Weblitz.MessageBoard.Tests.Fixtures;
using Weblitz.MessageBoard.Web.Controllers;
using Weblitz.MessageBoard.Web.Models;

namespace Weblitz.MessageBoard.Tests.Controllers
{
    [TestFixture]
    public class PostControllerTest : ControllerTestBase
    {
        private PostInput _input;

        [SetUp]
        public void SetUp()
        {
            ForumId = Guid.Empty;
            Result = null;
            Controller = null;
            Topic = null;
            Forums = null;
            Forum = null;
            TopicRepository = null;
            ForumRepository = null;

            _input = null;
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void PostDetails()
        {
            new Story("post details")
                .InOrderTo("read responses on a topic")
                .AsA("user")
                .IWant("to view post details")

                        .WithScenario("post found")
                            .Given(PostRepositoryIsInitialized)
                                .And(TopicRepositoryIsInitialized)
                                .And(PostControllerIsInitialized)
                                .And(IdOfPostThat_Exist, true)
                                .And(CallToFindByIdOnPostRepository)
                            .When(DetailsActionIsRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<PostDetail>, true)

                        .WithScenario("post not found")
                            .Given(PostRepositoryIsInitialized)
                                .And(TopicRepositoryIsInitialized)
                                .And(PostControllerIsInitialized)
                                .And(IdOfPostThat_Exist, false)
                                .And(CallToFindByIdOnPostRepository)
                            .When(DetailsActionIsRequested)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "No post matches ID")
                                .And(ShouldRedirectTo__, "Forum", "Index")
                .Execute();
        }

        [Test]
        public void PostGetCreate()
        {
            new Story("post get create")
                .InOrderTo("post to a topic of discussion")
                .AsA("user")
                .IWant("to input new post data")

                        .WithScenario("new post")
                            .Given(PostRepositoryIsInitialized)
                                .And(TopicRepositoryIsInitialized)
                                .And(PostControllerIsInitialized)
                                .And(IdOfTopicThat_Exist, true)
                                .And(CallToFindByIdOnTopicRepository)
                            .When(CreateActionIsRequestedWithGetVerb)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<PostInput>, true)
                .Execute();
        }

        [Test]
        public void PostPostCreate()
        {
            new Story("post post create")
                .InOrderTo("post a response to a topic")
                .AsA("user")
                .IWant("to save new post input")

                        .WithScenario("create post to topic successfully")
                            .Given(PostRepositoryIsInitialized)
                                .And(TopicRepositoryIsInitialized)
                                .And(PostControllerIsInitialized)
                                .And(_InputFor_Post_Parent, true, false, false)
                                .And(CallToFindByIdOnTopicRepository)
                                .And(CallToSaveOnPostRepository)
                            .When(CreateActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "created successfully")
                                .And(ShouldRedirectTo__, "Post", "Details")

                        .WithScenario("fail to create post to topic with invalid input")
                            .Given(PostRepositoryIsInitialized)
                                .And(TopicRepositoryIsInitialized)
                                .And(PostControllerIsInitialized)
                                .And(_InputFor_Post_Parent, false, false, false)
                            .When(CreateActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "failed to create post")
                                .And(ShouldRedirectTo__, "Post", "Create")

                        .WithScenario("create post to parent post successfully")
                            .Given(PostRepositoryIsInitialized)
                                .And(TopicRepositoryIsInitialized)
                                .And(PostControllerIsInitialized)
                                .And(_InputFor_Post_Parent, true, false, true)
                                .And(CallToFindByIdOnTopicRepository)
                                .And(CallToFindByIdOnPostRepository)
                                .And(CallToSaveOnPostRepository)
                            .When(CreateActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "created successfully")
                                .And(ShouldRedirectTo__, "Post", "Details")

                        .WithScenario("fail to create post to parent post with invalid input")
                            .Given(PostRepositoryIsInitialized)
                                .And(TopicRepositoryIsInitialized)
                                .And(PostControllerIsInitialized)
                                .And(_InputFor_Post_Parent, false, false, true)
                            .When(CreateActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "failed to create post")
                                .And(ShouldRedirectTo__, "Post", "Create")
                .Execute();
        }

        private void CreateActionIsRequestedWithPostVerb()
        {
            Result = (Controller as PostController).Create(_input);
        }

        private void CallToSaveOnPostRepository()
        {
            PostRepository.Stub(r => r.Save(Post));
        }

        private void _InputFor_Post_Parent([BooleanParameterFormat("valid", "invalid")] bool valid,
            [BooleanParameterFormat("existing", "new")] bool exists, 
            [BooleanParameterFormat("with", "without")] bool belongs)
        {
            if (valid)
            {
                Forum = ForumFixtures.ForumWithNoTopics(1);

                Topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);
                Topic.Forum = Forum;

                if (belongs)
                {
                    Parent = PostFixtures.RootPostWithNoChildren(1);
                    Parent.Topic = Topic;

                    Post = PostFixtures.BranchPostWithNoChildren(1);
                    Post.Topic = Topic;
                    Post.Parent = Parent;

                    _input = new PostInput
                    {
                        Body = Post.Body,
                        ParentId = Post.Parent.Id,
                        TopicId = Topic.Id
                    };                    
                }
                else
                {
                    Post = PostFixtures.RootPostWithNoChildren(1);
                    Post.Topic = Topic;

                    _input = new PostInput
                    {
                        Body = Post.Body,
                        ParentId = null,
                        TopicId = Topic.Id
                    };
                }

            }
            else
            {
                _input = new PostInput
                             {
                                 Body = string.Empty,
                                 TopicId = Guid.Empty
                             };

                Controller.ModelState.AddModelError("Title", "Title is required");
                Controller.ModelState.AddModelError("TopicId", "Topic is required");
            }

            if (exists)
            {
                _input.Id = Guid.NewGuid();
            }
        }

        private void CreateActionIsRequestedWithGetVerb()
        {
            Result = (Controller as PostController).Create(TopicId, ParentId);
        }

        private void CallToFindByIdOnTopicRepository()
        {
            TopicRepository.Stub(r => r.FindBy(TopicId)).Return(Topic);
        }

        private void DetailsActionIsRequested()
        {
            Result = (Controller as PostController).Details(PostId);
        }

        private void CallToFindByIdOnPostRepository()
        {
            PostRepository.Stub(r => r.FindBy(PostId)).Return(Post);
        }

        private void PostControllerIsInitialized()
        {
            Controller = new PostController(PostRepository, TopicRepository);

            var builder = new TestControllerBuilder();
            builder.InitializeController(Controller);
            builder.RouteData.Values["Controller"] = "Post";
        }
    }
}