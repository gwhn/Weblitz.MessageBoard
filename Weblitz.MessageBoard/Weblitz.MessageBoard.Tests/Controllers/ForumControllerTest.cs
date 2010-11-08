using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public class ForumControllerTest : ControllerTestBase
    {
        private IList<Forum> _forums;
        private IForumRepository _repository;
        private ActionResult _result;
        private Forum _forum;
        private Guid _id;
        private ForumController _controller;
        private ForumInput _input;

        [SetUp]
        public void SetUp()
        {
            _forums = null;
            _repository = null;
            _result = null;
            _forum = null;
            _id = Guid.Empty;
            _controller = null;
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
                                .And(ForumControllerCallsGetAllOnRepository)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModelShouldContainSummaries)

                        .WithScenario("forums but no existing topics")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(ListWith_Forums, 3)
                                .And(ForumControllerCallsGetAllOnRepository)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModelShouldContainSummaries)

                        .WithScenario("forums each with existing topics")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(ListWith_Forums, 0)
                                .And(EachForumContains_Topics, 2)
                                .And(ForumControllerCallsGetAllOnRepository)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModelShouldContainSummaries)

                        .WithScenario("forums each with existing topics but no existing posts")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(ListWith_Forums, 3)
                                .And(EachForumContains_Topics, 2)
                                .And(ForumControllerCallsGetAllOnRepository)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModelShouldContainSummaries)

                        .WithScenario("forums each with existing topics each with existing posts")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(ListWith_Forums, 1)
                                .And(EachForumContains_Topics, 2)
                                .And(EachTopicContains_Posts, 3)
                                .And(ForumControllerCallsGetAllOnRepository)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModelShouldContainSummaries)
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
                                .And(ForumControllerCallsGetByIdOnRepository)
                            .When(DetailsActionIsRequestedWith_Id, true)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_ContainDetails, true)

                        .WithScenario("forum not found")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(IdOfForumThat_Exist, false)
                            .When(DetailsActionIsRequestedWith_Id, false)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "No forum matches ID")
                                .And(ShouldRedirectTo_, "Index")
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
                                .And(ViewModelShouldContainEmptyInput)
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
                                .And(ForumControllerCallsSaveOnRepository)
                            .When(CreateActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "created successfully")
                                .And(ShouldRedirectTo_, "Details")

                        .WithScenario("fail to create forum with invalid input")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(_InputFor_Forum, false, false)
                            .When(CreateActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnViewResult)
                                .And(Message_Contain_, true, "failed to create forum")
                                .And(ShouldRenderDefaultView)
                                .And(ViewModelShouldContainEmptyInput)
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
                                .And(ForumControllerCallsGetByIdOnRepository)
                                .And(ForumControllerCallsSaveOnRepository)
                            .When(EditActionIsRequestedWithGetVerb)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModelShouldContainPopulatedInput)

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
                                .And(ForumControllerCallsSaveOnRepository)
                            .When(EditActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "updated successfully")
                                .And(ShouldRedirectTo_, "Details")

                        .WithScenario("fail to update forum with invalid input")
                            .Given(ForumRepositoryIsInitialized)
                                .And(ForumControllerIsInitialized)
                                .And(_InputFor_Forum, false, true)
                                .And(ForumControllerCallsGetByIdOnRepository)
                            .When(EditActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnViewResult)
                                .And(Message_Contain_, true, "failed to update forum")
                                .And(ShouldRenderDefaultView)
                                .And(ViewModelShouldContainPopulatedInput)

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
                                .And(ForumControllerCallsGetByIdOnRepository)
                            .When(DeleteActionIsRequestedWithGetVerb)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModelShouldContainDeleteItem)

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
                                .And(ForumControllerCallsGetByIdOnRepository)
                                .And(ForumControllerCallsDeleteOnRepository)
                            .When(DeleteActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "deleted successfully")
                                .And(ShouldRedirectTo_, "Index")

//                        .WithScenario("fail to delete forum with unknown id")
                .Execute();
        }

        private void DeleteActionIsRequestedWithPostVerb()
        {
            _result = _controller.Destroy(_id);
        }

        private void ForumControllerCallsDeleteOnRepository()
        {
            _repository.Stub(r => r.Delete(_forum));
        }

        private void DeleteActionIsRequestedWithGetVerb()
        {
            _result = _controller.Delete(_id);
        }

        private void ViewModelShouldContainDeleteItem()
        {
            var result = _result as ViewResult;
            Assert.IsNotNull(result);

            var model = result.ViewData.Model as DeleteItem;
            Assert.IsNotNull(model);

            Assert.That(model.Id != Guid.Empty);
            Assert.IsNotNull(model.Description);
            Assert.IsNotNull(model.CancelNavigation);
            Assert.IsNotNull(model.CancelNavigation.ActionName);
            Assert.IsNotNull(model.CancelNavigation.ControllerName);
        }

        private void EditActionIsRequestedWithPostVerb()
        {
            _result = _controller.Edit(_input);
        }

        private void ForumControllerCallsSaveOnRepository()
        {
            _repository.Stub(r => r.Save(_forum));

            SetEntityId(_forum, Guid.NewGuid());
        }

        private void SetEntityId(Entity entity, Guid id)
        {
            entity.GetType()
                .GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)
                .SetValue(entity, id, null);
        }

        private void ForumControllerCallsGetByIdOnRepository()
        {
            _repository.Stub(r => r.FindBy(_id)).Return(_forum);

            SetEntityId(_forum, Guid.NewGuid());
        }

        private void ForumControllerCallsGetAllOnRepository()
        {
            _repository.Stub(r => r.All()).Return(_forums.AsQueryable());
        }

        private void EditActionIsRequestedWithGetVerb()
        {
            _result = _controller.Edit(_id);
        }

        private void ViewModelShouldContainPopulatedInput()
        {
            var result = _result as ViewResult;
            Assert.IsNotNull(result);

            var model = result.ViewData.Model as ForumInput;
            Assert.IsNotNull(model);

            Assert.That(model.Id != Guid.Empty);
            Assert.That(model.Name != null);
        }

        private void ForumControllerIsInitialized()
        {
            _controller = new ForumController(_repository);

            var builder = new TestControllerBuilder();
            builder.InitializeController(_controller);
            builder.RouteData.Values["Controller"] = "Forum";
        }

        private void _InputFor_Forum([BooleanParameterFormat("valid", "invalid")] bool valid,
            [BooleanParameterFormat("existing", "new")] bool exists)
        {
            if (valid)
            {
                _forum = ForumFixtures.ForumWithNoTopics(1);

                _input = new ForumInput {Name = _forum.Name};                
            }
            else
            {
                _input = new ForumInput {Name = string.Empty};

                _controller.ModelState.AddModelError("Name", "Name is required");
            }

            if (exists)
            {
                _input.Id = Guid.NewGuid();
            }
        }

        private void CreateActionIsRequestedWithPostVerb()
        {
            _result = _controller.Create(_input);
        }

        private void CreateActionIsRequestedWithGetVerb()
        {
            _result = _controller.Create();
        }

        private void ViewModelShouldContainEmptyInput()
        {
            var result = _result as ViewResult;
            Assert.IsNotNull(result);

            var model = result.ViewData.Model as ForumInput;
            Assert.IsNotNull(model);

            Assert.That(model.Id == Guid.Empty);
            Assert.That(model.Name == null);
        }

        private void ShouldRedirectTo_(string action)
        {
            var result = _result as RedirectToRouteResult;
            Assert.IsNotNull(result);

            Assert.That(result.RouteValues["action"].ToString() == action);
        }

        private void Message_Contain_([BooleanParameterFormat("should", "should not")] bool contains, string text)
        {
            if (contains)
            {
                Assert.That(_controller.TempData.ContainsKey("Message"));

                var message = _controller.TempData["Message"] as string;
                Assert.IsNotNull(message);

                Assert.That(message.ToLowerInvariant().Contains(text.ToLowerInvariant()));
            }
            else
            {
                Assert.IsFalse(_controller.TempData.ContainsKey("Message"));
            }
        }

        private void ShouldReturnRedirectToRouteResult()
        {
            Assert.IsNotNull(_result);
            Assert.IsInstanceOf<RedirectToRouteResult>(_result);
        }

        private void ListWith_Forums(int count)
        {
            _forums = new List<Forum>();

            for (var i = 0; i < count; i++)
            {
                _forums.Add(ForumFixtures.ForumWithNoTopics(i));
            }
        }

        private void ForumRepositoryIsInitialized()
        {
            _repository = Stub<IForumRepository>();
        }

        private void IndexActionRequested()
        {
            _result = _controller.Index();
        }

        private void ShouldReturnViewResult()
        {
            Assert.IsNotNull(_result);
            Assert.IsInstanceOf<ViewResult>(_result);
        }

        private void ViewModelShouldContainSummaries()
        {
            var result = _result as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewData);
            Assert.IsNotNull(result.ViewData.Model);

            Assert.IsInstanceOf<ForumSummary[]>(result.ViewData.Model);
            var model = result.ViewData.Model as ForumSummary[];
            
            Assert.That(model.Length == _forums.Count);
        }

        private void EachForumContains_Topics(int count)
        {
            foreach (var forum in _forums)
            {
                for (var i = 0; i < count; i++)
                {
                    forum.Add(TopicFixtures.TopicWithNoPostsAndNoAttachments(i));
                }
            }
        }

        private void EachTopicContains_Posts(int count)
        {
            foreach (var topic in _forums.SelectMany(forum => forum.Topics))
            {
                for (var i = 0; i < count; i++)
                {
                    topic.Add(PostFixtures.RootPostWithNoChildren(i));
                }
            }
        }

        private void IdOfForumThat_Exist([BooleanParameterFormat("does", "does not")] bool exists)
        {
            if (exists)
            {
                _forum = ForumFixtures.ForumWithNoTopics(_id);

                _id = Guid.NewGuid();
            }
            else
            {
                _forum = default(Forum);

                _id = Guid.Empty;
            }
        }

        private void DetailsActionIsRequestedWith_Id([BooleanParameterFormat("matching", "unmatched")] bool matches)
        {
            if (matches)
            {
                _result = _controller.Details(_id);
            }
            else
            {
                _result = _controller.Details(_id);
            }
        }

        private void ShouldRenderDefaultView()
        {
            var result = _result as ViewResult;

            Assert.IsNotNull(result);

            Assert.That(result.AssertViewRendered().ViewName == string.Empty);
        }

        private void ViewModel_ContainDetails([BooleanParameterFormat("should", "should not")] bool contains)
        {
            var result = _result as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewData);
            Assert.IsNotNull(result.ViewData.Model);

            if (contains)
            {
                Assert.IsInstanceOf<ForumDetail>(result.ViewData.Model);
            }
            else
            {
                Assert.IsNotInstanceOf<ForumDetail>(result.ViewData.Model);
            }
        }
    }
}