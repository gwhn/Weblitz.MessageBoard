using System;
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
        private IKeyedRepository<Topic, Guid> _repository;
        private Topic _topic;

        [SetUp]
        public void SetUp()
        {
            Id = Guid.Empty;
            Result = null;
            Controller = null;

            _repository = null;
            _topic = null;
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
                                .And(TopicControllerIsInitialized)
                                .And(IdOfTopicThat_Exist, true)
                                .And(TopicControllerCallsFindById)
                            .When(DetailsActionIsRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<TopicDetail>, true)

                        .WithScenario("topic not found")
                            .Given(TopicRepositoryIsInitialized)
                                .And(TopicControllerIsInitialized)
                                .And(IdOfTopicThat_Exist, false)
                                .And(TopicControllerCallsFindById)
                            .When(DetailsActionIsRequested)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "No topic matches ID")
                                .And(ShouldRedirectTo__, "Forum", "Index")
                .Execute();
        }

        private void TopicRepositoryIsInitialized()
        {
            _repository = Stub<IKeyedRepository<Topic, Guid>>();
        }

        private void TopicControllerIsInitialized()
        {
            Controller = new TopicController(_repository);

            var builder = new TestControllerBuilder();
            builder.InitializeController(Controller);
            builder.RouteData.Values["Controller"] = "Topic";
        }

        private void IdOfTopicThat_Exist([BooleanParameterFormat("does", "does not")] bool exists)
        {
            if (exists)
            {
                Id = Guid.NewGuid();
                _topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(Id);
                _topic.Forum = ForumFixtures.ForumWithNoTopics(Id);
            }
            else
            {
                Id = Guid.Empty;
                _topic = default(Topic);
            }
        }

        private void TopicControllerCallsFindById()
        {
            _repository.Stub(r => r.FindBy(Id)).Return(_topic);

            SetEntityId(_topic, Guid.NewGuid());
        }

        private void DetailsActionIsRequested()
        {
            Result = (Controller as TopicController).Details(Id);
        }
    }
}