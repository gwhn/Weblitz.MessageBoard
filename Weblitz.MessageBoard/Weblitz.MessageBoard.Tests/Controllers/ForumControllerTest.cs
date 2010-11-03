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
                                .And(ShouldWriteErrorMessage)
                                .And(ShouldRedirectTo_, "Index")
                .Execute();
        }

        private void ShouldRedirectTo_(string action)
        {
            var result = _result as RedirectToRouteResult;
            Assert.IsNotNull(result);

            Assert.That(result.RouteValues["action"].ToString() == action);
        }

        private void ShouldWriteErrorMessage()
        {
            Assert.That(_controller.TempData.ContainsKey("Message"));
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