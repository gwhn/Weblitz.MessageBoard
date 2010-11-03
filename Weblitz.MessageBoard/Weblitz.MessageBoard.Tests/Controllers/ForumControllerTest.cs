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
    public class ForumControllerTest : ControllerTestBase
    {
        private IList<Forum> _forums;
        private IForumRepository _repository;
        private ActionResult _result;
        private Forum _forum;
        private Guid _id;
        private Guid _goodId;
        private Guid _badId;
        private ForumController _controller;
        private ForumInput _input;

        [Test]
        public void ForumIndex()
        {
            new Story("forum index")
                .InOrderTo("browse forums")
                .AsA("user")
                .IWant("to view forum summaries")

                        .WithScenario("no existing forums")
                            .Given(ListWith_Forums, 0)
                                .And(ForumRepositoryIsInitialized)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_ContainSummaries, true)

                        .WithScenario("forums but no existing topics")
                            .Given(ListWith_Forums, 3)
                                .And(ForumRepositoryIsInitialized)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_ContainSummaries, true)

                        .WithScenario("forums each with existing topics")
                            .Given(ListWith_Forums, 0)
                                .And(EachForumContains_Topics, 2)
                                .And(ForumRepositoryIsInitialized)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_ContainSummaries, true)

                        .WithScenario("forums each with existing topics but no existing posts")
                            .Given(ListWith_Forums, 3)
                                .And(EachForumContains_Topics, 2)
                                .And(ForumRepositoryIsInitialized)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_ContainSummaries, true)

                        .WithScenario("forums each with existing topics each with existing posts")
                            .Given(ListWith_Forums, 1)
                                .And(EachForumContains_Topics, 2)
                                .And(EachTopicContains_Posts, 3)
                                .And(ForumRepositoryIsInitialized)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_ContainSummaries, true)
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
                            .Given(IdOfForumThat_Exist, true)
                                .And(ForumRepositoryIsInitialized)
                            .When(DetailsActionIsRequestedWith_Id, true)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderDefaultView)
                                .And(ViewModel_ContainDetails, true)

                        .WithScenario("forum not found")
                            .Given(IdOfForumThat_Exist, false)
                                .And(ForumRepositoryIsInitialized)
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
                .IWant("to input name of new forum")

                        .WithScenario("new forum")
                            .Given(ForumRepositoryIsInitialized)
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

                        .WithScenario("create forum")
                            .Given(_ForumInput, true)
                                .And(ForumRepositoryIsInitialized)
                            .When(CreateActionIsRequestedWithPostVerb)
                            .Then(ShouldReturnRedirectToRouteResult)
                                .And(Message_Contain_, true, "created successfully")
                                .And(ShouldRedirectTo_, "Details")
                .Execute();
        }

        private void _ForumInput([BooleanParameterFormat("valid", "invalid")] bool valid)
        {
            _input = valid ? 
                new ForumInput {Name = ForumFixtures.ForumWithNoTopics(1).Name} : 
                new ForumInput {Name = string.Empty};
        }

        private void CreateActionIsRequestedWithPostVerb()
        {
            _controller = new ForumController(_repository);

            _result = _controller.Create(_input);
        }

        private void ShouldRedirectToDetails()
        {
            throw new NotImplementedException();
        }

        private void CreateActionIsRequestedWithGetVerb()
        {
            _controller = new ForumController(_repository);

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

                Assert.That(message.Contains(text));
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
            _repository.Stub(r => r.GetAll()).Return(_forums.ToArray());

            _controller = new ForumController(_repository);

            _result = _controller.Index();
        }

        private void ShouldReturnViewResult()
        {
            Assert.IsNotNull(_result);
            Assert.IsInstanceOf<ViewResult>(_result);
        }

        private void ViewModel_ContainSummaries([BooleanParameterFormat("should", "should not")] bool contains)
        {
            var result = _result as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewData);
            Assert.IsNotNull(result.ViewData.Model);

            if (contains)
            {
                Assert.IsInstanceOf<ForumSummary[]>(result.ViewData.Model);
                var model = result.ViewData.Model as ForumSummary[];
                Assert.That(model.Length == _forums.Count);
            }
            else
            {
                Assert.IsNotInstanceOf<ForumSummary[]>(result.ViewData.Model);
            }
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
                _goodId = Guid.NewGuid();
            }
            else
            {
                _badId = Guid.Empty;
            }
        }

        private void DetailsActionIsRequestedWith_Id([BooleanParameterFormat("matching", "unmatched")] bool matches)
        {
            _controller = new ForumController(_repository);

            if (matches)
            {
                var match = ForumFixtures.ForumWithNoTopics(_goodId);

                _repository.Stub(r => r.GetById(_goodId)).IgnoreArguments().Return(match);

                _result = _controller.Details(_goodId);
            }
            else
            {
                var unmatched = default(Forum);

                _repository.Stub(r => r.GetById(_badId)).IgnoreArguments().Return(unmatched);

                _result = _controller.Details(_badId);
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

        private void ShouldReturnActionResult()
        {
            throw new NotImplementedException();
        }

        private void ShouldRedirectToNotFoundAction()
        {
            throw new NotImplementedException();
        }
    }
}