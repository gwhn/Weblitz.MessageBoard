using System;
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
    public class TopicControllerTest : ControllerTestBase
    {
        private IKeyedRepository<Topic, Guid> _topicRepository;
        private IKeyedRepository<Forum, Guid> _forumRepository;
        private TopicInput _input;

        [SetUp]
        public void SetUp()
        {
            ForumId = Guid.Empty;
            Result = null;
            Controller = null;

            _topicRepository = null;
            Topic = null;
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void TopicDetails()
        {
            new Story("topic details")
                .InOrderTo("browse posts in a topic")
                .AsA("user")
                .IWant("to view topic details")

                        .WithScenario("topic found")
                            .Given(TopicRepositoryIsInitialized)
                                .And(ForumRepositoryIsInitialized)
                                .And(TopicControllerIsInitialized)
                                .And(IdOfTopicThat_Exist, true)
                                .And(TopicControllerCallsFindByIdOnTopicRepository)
                            .When(DetailsActionIsRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<TopicDetail>, true)

                        .WithScenario("topic not found")
                            .Given(TopicRepositoryIsInitialized)
                                .And(ForumRepositoryIsInitialized)
                                .And(TopicControllerIsInitialized)
                                .And(IdOfTopicThat_Exist, false)
                                .And(TopicControllerCallsFindByIdOnTopicRepository)
                            .When(DetailsActionIsRequested)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "No topic matches ID")
                                .And(ShouldRedirectTo__, "Forum", "Index")
                .Execute();
        }

        [Test]
        public void TopicGetCreate()
        {
            new Story("topic get create")
                .InOrderTo("start a new topic of discussion")
                .AsA("administrator")
                .IWant("to input new topic data")

                        .WithScenario("new topic")
                            .Given(TopicRepositoryIsInitialized)
                                .And(ForumRepositoryIsInitialized)
                                .And(TopicControllerIsInitialized)
                                .And(IdOfForumThat_Exist, true)
                                .And(ListWith_Forums, 3)
                                .And(TopicControllerCallsAllOnForumRepository)
                            .When(CreateActionIsRequestedWithGetVerb)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<TopicInput>, true)
                .Execute();
        }

        [Test]
        public void ForumPostCreate()
        {
            new Story("topic post create")
                .InOrderTo("start a new topic of discussion")
                .AsA("administrator")
                .IWant("to save new topic input")

                        .WithScenario("create topic successfully")
                            .Given(TopicRepositoryIsInitialized)
                                .And(ForumRepositoryIsInitialized)
                                .And(TopicControllerIsInitialized)
                                .And(ListWith_Forums, 15)
                                .And(TopicControllerCallsAllOnForumRepository)
                                .And(_InputFor_Topic, true, false)
                                .And(TopicControllerCallsSaveOnForumRepository)
                            .When(CreateActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "created successfully")
                                .And(ShouldRedirectTo__, "Topic", "Details")

                        .WithScenario("fail to create topic with invalid input")
                            .Given(TopicRepositoryIsInitialized)
                                .And(ForumRepositoryIsInitialized)
                                .And(TopicControllerIsInitialized)
                                .And(_InputFor_Topic, false, false)
                            .When(CreateActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "failed to create topic")
                                .And(ShouldRedirectTo__, "Topic", "Create")
                .Execute();
        }

        private void CreateActionIsRequestedWithPostVerb()
        {
            Result = (Controller as TopicController).Create(_input);
        }

        private void TopicControllerCallsSaveOnForumRepository()
        {
            _topicRepository.Stub(r => r.Save(Topic));
        }

        private void _InputFor_Topic([BooleanParameterFormat("valid", "invalid")] bool valid,
            [BooleanParameterFormat("existing", "new")] bool exists)
        {
            if (valid)
            {
                Forum = ForumFixtures.ForumWithNoTopics(1);

                Topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(1);

                Topic.Forum = Forum;

                _input = new TopicInput
                             {
                                 Title = Topic.Title,
                                 Body = Topic.Body,
                                 Closed = false,
                                 Sticky = true,
                                 ForumId = Forum.Id,
                                 Forums = new SelectList(_forumRepository.All().ToArray(), "Id", "Name")
                             };
            }
            else
            {
                _input = new TopicInput
                             {
                                 Title = string.Empty,
                                 Body = string.Empty,
                                 ForumId = Guid.Empty
                             };

                Controller.ModelState.AddModelError("Title", "Title is required");
                Controller.ModelState.AddModelError("Body", "Body is required");
                Controller.ModelState.AddModelError("ForumId", "Forum is required");
            }

            if (exists)
            {
                _input.Id = Guid.NewGuid();
            }
        }

        private void TopicControllerCallsAllOnForumRepository()
        {
            _forumRepository.Stub(r => r.All()).Return(Forums.AsQueryable());
        }

        private void ForumRepositoryIsInitialized()
        {
            _forumRepository = Stub<IKeyedRepository<Forum, Guid>>();
        }

        private void CreateActionIsRequestedWithGetVerb()
        {
            Result = (Controller as TopicController).Create(ForumId);
        }

        private void TopicRepositoryIsInitialized()
        {
            _topicRepository = Stub<IKeyedRepository<Topic, Guid>>();
        }

        private void TopicControllerIsInitialized()
        {
            Controller = new TopicController(_topicRepository, _forumRepository);

            var builder = new TestControllerBuilder();
            builder.InitializeController(Controller);
            builder.RouteData.Values["Controller"] = "Topic";
        }

        private void TopicControllerCallsFindByIdOnTopicRepository()
        {
            _topicRepository.Stub(r => r.FindBy(ForumId)).Return(Topic);

            SetEntityId(Topic, Guid.NewGuid());
        }

        private void DetailsActionIsRequested()
        {
            Result = (Controller as TopicController).Details(ForumId);
        }
    }
}