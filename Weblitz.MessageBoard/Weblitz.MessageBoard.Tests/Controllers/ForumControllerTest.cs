using System;
using System.Linq;
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
    public class ForumControllerTest : ControllerTestBase
    {
        private IKeyedRepository<Forum, Guid> _repository;
        private ForumInput _input;
        
        [SetUp]
        public void SetUp()
        {
            ForumId = Guid.Empty;
            Result = null;
            Controller = null;

            Forums = null;
            _repository = null;
            Forum = null;
            _input = null;
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void ForumIndex()
        {
            new Story("forum index")
                .InOrderTo("browse forums")
                .AsA("user")
                .IWant("to view forum summaries")

                        .WithScenario("no existing forums")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(ListWith_Forums, 0)
                                .And(ShouldCallGetAllOnForumRepository)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<ForumSummary[]>, true)

                        .WithScenario("forums but no existing topics")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(ListWith_Forums, 3)
                                .And(ShouldCallGetAllOnForumRepository)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<ForumSummary[]>, true)

                        .WithScenario("forums each with existing topics")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(ListWith_Forums, 0)
                                .And(EachForumContains_Topics, 2)
                                .And(ShouldCallGetAllOnForumRepository)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<ForumSummary[]>, true)

                        .WithScenario("forums each with existing topics but no existing posts")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(ListWith_Forums, 3)
                                .And(EachForumContains_Topics, 2)
                                .And(ShouldCallGetAllOnForumRepository)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<ForumSummary[]>, true)

                        .WithScenario("forums each with existing topics each with existing posts")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(ListWith_Forums, 1)
                                .And(EachForumContains_Topics, 2)
                                .And(EachTopicContains_Posts, 3)
                                .And(ShouldCallGetAllOnForumRepository)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<ForumSummary[]>, true)
                .Execute();
        }

        [Test]
        public void ForumDetails()
        {
            new Story("forum details")
                .InOrderTo("browse topics in a forum")
                .AsA("user")
                .IWant("to view forum details")

                        .WithScenario("forum found")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(IdOfForumThat_Exist, true)
                                .And(ShouldCallFindByIdOnForumRepository)
                            .When(DetailsActionIsRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<ForumDetail>, true)

                        .WithScenario("forum not found")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(IdOfForumThat_Exist, false)
                                .And(ShouldCallFindByIdOnForumRepository)
                            .When(DetailsActionIsRequested)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "No forum matches ID")
                                .And(ShouldRedirectTo__, "Forum", "Index")
                .Execute();
        }

        [Test]
        public void ForumGetCreate()
        {
            new Story("forum get create")
                .InOrderTo("start a new discussion")
                .AsA("administrator")
                .IWant("to input new forum data")

                        .WithScenario("new forum")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                            .When(CreateActionIsRequestedWithGetVerb)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<ForumInput>, true)
                .Execute();
        }

        [Test]
        public void ForumPostCreate()
        {
            new Story("forum post create")
                .InOrderTo("start a new discussion")
                .AsA("administrator")
                .IWant("to save new forum input")

                        .WithScenario("create forum successfully")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(_InputFor_Forum, true, false)
                                .And(ShouldCallSaveOnForumRepository)
                            .When(CreateActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "created successfully")
                                .And(ShouldRedirectTo__, "Forum", "Details")

                        .WithScenario("fail to create forum with invalid input")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(_InputFor_Forum, false, false)
                            .When(CreateActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnViewResult)
                                .And(Message_Contain_, true, "failed to create forum")
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<ForumInput>, true)
                .Execute();
        }

        [Test]
        public void ForumGetEdit()
        {
            new Story("forum get edit")
                .InOrderTo("modify the subject of discussion")
                .AsA("administrator")
                .IWant("to input modified forum data")

                        .WithScenario("edit forum")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(IdOfForumThat_Exist, true)
                                .And(ShouldCallFindByIdOnForumRepository)
                                .And(ShouldCallSaveOnForumRepository)
                            .When(EditActionIsRequestedWithGetVerb)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<ForumInput>, true)

//                        .WithScenario("edit forum with unknown id")
                .Execute();
        }

        [Test]
        public void ForumPostEdit()
        {
            new Story("forum post edit")
                .InOrderTo("modify the subject of discussion")
                .AsA("administrator")
                .IWant("to save modified forum input")

                        .WithScenario("update forum successfully")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(_InputFor_Forum, true, true)
                                .And(ShouldCallSaveOnForumRepository)
                            .When(EditActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "updated successfully")
                                .And(ShouldRedirectTo__, "Forum", "Details")

                        .WithScenario("fail to update forum with invalid input")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(_InputFor_Forum, false, true)
                                .And(ShouldCallFindByIdOnForumRepository)
                            .When(EditActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnViewResult)
                                .And(Message_Contain_, true, "failed to update forum")
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<ForumInput>, true)

//                        .WithScenario("fail to update forum with unknown id")
                .Execute();
        }

        [Test]
        public void ForumGetDelete()
        {
            new Story("forum get delete")
                .InOrderTo("remove the subject of discussion")
                .AsA("administrator")
                .IWant("to delete selected forum")

                        .WithScenario("delete forum")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(IdOfForumThat_Exist, true)
                                .And(ShouldCallFindByIdOnForumRepository)
                            .When(DeleteActionIsRequestedWithGetVerb)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_Contain<DeleteItem>, true)

//                        .WithScenario("delete forum with unknown id")
                .Execute();
        }

        [Test]
        public void ForumPostDelete()
        {
            new Story("forum post delete")
                .InOrderTo("remove the subject of discussion")
                .AsA("administrator")
                .IWant("to confirm deletion of selected forum")

                        .WithScenario("delete forum successfully")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(IdOfForumThat_Exist, true)
                                .And(ShouldCallFindByIdOnForumRepository)
                                .And(ShouldCallDeleteOnForumRepository)
                            .When(DeleteActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "deleted successfully")
                                .And(ShouldRedirectTo__, "Forum", "Index")

//                        .WithScenario("fail to delete forum with unknown id")
                .Execute();
        }

        private void DeleteActionIsRequestedWithPostVerb()
        {
            Result = (Controller as ForumController).Destroy(ForumId);
        }

        private void ShouldCallDeleteOnForumRepository()
        {
            _repository.Stub(r => r.Delete(Forum));
        }

        private void DeleteActionIsRequestedWithGetVerb()
        {
            Result = (Controller as ForumController).Delete(ForumId);
        }

        private void EditActionIsRequestedWithPostVerb()
        {
            Result = (Controller as ForumController).Edit(_input);
        }

        private void ShouldCallSaveOnForumRepository()
        {
            _repository.Stub(r => r.Save(Forum));

            SetEntityId(Forum, Guid.NewGuid());
        }

        private void ShouldCallFindByIdOnForumRepository()
        {
            _repository.Stub(r => r.FindBy(ForumId)).Return(Forum);

            SetEntityId(Forum, Guid.NewGuid());
        }

        private void ShouldCallGetAllOnForumRepository()
        {
            _repository.Stub(r => r.All()).Return(Forums.AsQueryable());
        }

        private void EditActionIsRequestedWithGetVerb()
        {
            Result = (Controller as ForumController).Edit(ForumId);
        }

        private void ForumControllerIsInitialized()
        {
            Controller = new ForumController(_repository);

            var builder = new TestControllerBuilder();
            builder.InitializeController(Controller);
            builder.RouteData.Values["Controller"] = "Forum";
        }

        private void _InputFor_Forum([BooleanParameterFormat("valid", "invalid")] bool valid,
            [BooleanParameterFormat("existing", "new")] bool exists)
        {
            if (valid)
            {
                Forum = ForumFixtures.ForumWithNoTopics(1);

                _input = new ForumInput {Name = Forum.Name};                
            }
            else
            {
                _input = new ForumInput {Name = string.Empty};

                Controller.ModelState.AddModelError("Name", "Name is required");
            }

            if (exists)
            {
                _input.Id = Guid.NewGuid();
            }
        }

        private void CreateActionIsRequestedWithPostVerb()
        {
            Result = (Controller as ForumController).Create(_input);
        }

        private void CreateActionIsRequestedWithGetVerb()
        {
            Result = (Controller as ForumController).Create();
        }

        private void ForumRepositoryIsInitialized()
        {
            _repository = Stub<IKeyedRepository<Forum, Guid>>();
        }

        private void IndexActionRequested()
        {
            Result = (Controller as ForumController).Index();
        }

        private void DetailsActionIsRequested()
        {
            Result = (Controller as ForumController).Details(ForumId);
        }
    }
}