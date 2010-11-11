using System;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Rhino.Mocks;
using StoryQ;
using StoryQ.Formatting.Parameters;
using Weblitz.MessageBoard.Tests.Fixtures;
using Weblitz.MessageBoard.Web.Controllers;
using Weblitz.MessageBoard.Web.Models;

namespace Weblitz.MessageBoard.Tests.Controllers
{
    [TestFixture]
    public class TopicControllerTest : ControllerTestBase
    {
        private TopicInput _input;

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
                                .And(CallToFindByIdOnTopicRepository)
                            .When(DetailsActionIsRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<TopicDetail>, true)

                        .WithScenario("topic not found")
                            .Given(TopicRepositoryIsInitialized)
                                .And(ForumRepositoryIsInitialized)
                                .And(TopicControllerIsInitialized)
                                .And(IdOfTopicThat_Exist, false)
                                .And(CallToFindByIdOnTopicRepository)
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
                                .And(CallToFindByIdOnForumRepository)
                                .And(ListWith_Forums, 3)
                                .And(CallToAllOnForumRepository)
                            .When(CreateActionIsRequestedWithGetVerb)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<TopicInput>, true)
                .Execute();
        }

        [Test]
        public void TopicPostCreate()
        {
            new Story("topic post create")
                .InOrderTo("start a new topic of discussion")
                .AsA("administrator")
                .IWant("to save new topic input")

                        .WithScenario("create topic successfully")
                            .Given(TopicRepositoryIsInitialized)
                                .And(ForumRepositoryIsInitialized)
                                .And(TopicControllerIsInitialized)
                                .And(ListWith_Forums, 5)
                                .And(CallToAllOnForumRepository)
                                .And(_InputFor_Topic, true, false)
                                .And(CallToFindByIdOnForumRepository)
                                .And(CallToSaveOnTopicRepository)
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

        [Test]
        public void TopicGetEdit()
        {
            new Story("topic get edit")
                .InOrderTo("modify the topic of discussion")
                .AsA("administrator")
                .IWant("to input modified topic data")

                        .WithScenario("edit topic")
                            .Given(TopicRepositoryIsInitialized)
                                .And(ForumRepositoryIsInitialized)
                                .And(TopicControllerIsInitialized)
                                .And(IdOfTopicThat_Exist, true)
                                .And(ListWith_Forums, 15)
                                .And(CallToAllOnForumRepository)
                                .And(CallToFindByIdOnTopicRepository)
                                .And(CallToSaveOnTopicRepository)
                            .When(EditActionIsRequestedWithGetVerb)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<TopicInput>, true)

//                        .WithScenario("edit topic with unknown id")
                .Execute();
        }

        [Test]
        public void TopicPostEdit()
        {
            new Story("topic post edit")
                .InOrderTo("modify the topic of discussion")
                .AsA("administrator")
                .IWant("to save modified topic input")

                        .WithScenario("update topic successfully")
                            .Given(TopicRepositoryIsInitialized)
                                .And(ForumRepositoryIsInitialized)
                                .And(TopicControllerIsInitialized)
                                .And(ListWith_Forums, 8)
                                .And(CallToAllOnForumRepository)
                                .And(_InputFor_Topic, true, true)
                                .And(CallToSaveOnTopicRepository)
                            .When(EditActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "updated successfully")
                                .And(ShouldRedirectTo__, "Topic", "Details")

                        .WithScenario("fail to update topic with invalid input")
                            .Given(TopicRepositoryIsInitialized)
                                .And(ForumRepositoryIsInitialized)
                                .And(TopicControllerIsInitialized)
                                .And(_InputFor_Topic, false, true)
                            .When(EditActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnViewResult)
                                .And(Message_Contain_, true, "failed to update topic")
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<TopicInput>, true)

//                        .WithScenario("fail to update topic with unknown id")
                .Execute();
        }

        [Test]
        public void TopicGetDelete()
        {
            new Story("topic get delete")
                .InOrderTo("remove the topic of discussion")
                .AsA("administrator")
                .IWant("to delete selected topic")

                        .WithScenario("delete topic")
                            .Given(TopicRepositoryIsInitialized)
                                .And(ForumRepositoryIsInitialized)
                                .And(TopicControllerIsInitialized)
                                .And(IdOfTopicThat_Exist, true)
                                .And(CallToFindByIdOnTopicRepository)
                            .When(DeleteActionIsRequestedWithGetVerb)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<DeleteItem>, true)

//                        .WithScenario("delete forum with unknown id")
                .Execute();
        }

        [Test]
        public void TopicPostDelete()
        {
            new Story("topic post delete")
                .InOrderTo("remove the topic of discussion")
                .AsA("administrator")
                .IWant("to confirm deletion of selected topic")

                        .WithScenario("delete topic successfully")
                            .Given(TopicRepositoryIsInitialized)
                                .And(ForumRepositoryIsInitialized)
                                .And(TopicControllerIsInitialized)
                                .And(IdOfTopicThat_Exist, true)
                                .And(CallToFindByIdOnTopicRepository)
                                .And(CallToDeleteOnTopicRepository)
                            .When(DeleteActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "deleted successfully")
                                .And(ShouldRedirectTo__, "Forum", "Details")

//                        .WithScenario("fail to delete topic with unknown id")
                .Execute();
        }

        private void DeleteActionIsRequestedWithPostVerb()
        {
            Result = (Controller as TopicController).Destroy(TopicId);
        }

        private void CallToDeleteOnTopicRepository()
        {
            TopicRepository.Stub(r => r.Delete(Topic));
        }

        private void DeleteActionIsRequestedWithGetVerb()
        {
            Result = (Controller as TopicController).Delete(TopicId);
        }

        private void EditActionIsRequestedWithPostVerb()
        {
            Result = (Controller as TopicController).Edit(_input);
        }

        private void CallToFindByIdOnForumRepository()
        {
            ForumRepository.Stub(r => r.FindBy(ForumId)).IgnoreArguments().Return(Forum);

            SetEntityId(Forum, Guid.NewGuid());
        }

        private void EditActionIsRequestedWithGetVerb()
        {
            Result = (Controller as TopicController).Edit(TopicId);
        }

        private void CreateActionIsRequestedWithPostVerb()
        {
            Result = (Controller as TopicController).Create(_input);
        }

        private void CallToSaveOnTopicRepository()
        {
            TopicRepository.Stub(r => r.Save(Topic));
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
                                 Forums = new SelectList(ForumRepository.All().ToArray(), "Id", "Name")
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

        private void CallToAllOnForumRepository()
        {
            ForumRepository.Stub(r => r.All()).Return(Forums.AsQueryable());
        }

        private void CreateActionIsRequestedWithGetVerb()
        {
            Result = (Controller as TopicController).Create(ForumId);
        }

        private void TopicControllerIsInitialized()
        {
            Controller = new TopicController(TopicRepository, ForumRepository);

            var builder = new TestControllerBuilder();
            builder.InitializeController(Controller);
            builder.RouteData.Values["Controller"] = "Topic";
        }

        private void CallToFindByIdOnTopicRepository()
        {
            TopicRepository.Stub(r => r.FindBy(TopicId)).IgnoreArguments().Return(Topic);

            SetEntityId(Topic, Guid.NewGuid());
        }

        private void DetailsActionIsRequested()
        {
            Result = (Controller as TopicController).Details(ForumId);
        }
    }
}