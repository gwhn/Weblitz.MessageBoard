using System;
using System.Linq;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Rhino.Mocks;
using StoryQ;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Core.Domain.Repositories;
using Weblitz.MessageBoard.Web.Controllers;
using Weblitz.MessageBoard.Web.Models;

namespace Weblitz.MessageBoard.Tests.Controllers
{
    [TestFixture]
    public class TopicControllerTest : ControllerTestBase
    {
        private IKeyedRepository<Topic, Guid> _topicRepository;
        private IKeyedRepository<Forum, Guid> _forumRepository;

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