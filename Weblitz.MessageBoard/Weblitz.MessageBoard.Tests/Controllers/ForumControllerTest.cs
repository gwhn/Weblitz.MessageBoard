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
        private ViewResult _result;

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
                                .And(ShouldRenderIndexView)
                                .And(ViewModel_ContainSummaries, true)

                        .WithScenario("forums but no existing topics")
                            .Given(ListWith_Forums, 3)
                                .And(ForumRepositoryIsInitialized)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderIndexView)
                                .And(ViewModel_ContainSummaries, true)

                        .WithScenario("forums each with existing topics")
                            .Given(ListWith_Forums, 0)
                                .And(EachForumContains_Topics, 2)
                                .And(ForumRepositoryIsInitialized)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderIndexView)
                                .And(ViewModel_ContainSummaries, true)

                        .WithScenario("forums each with existing topics but no existing posts")
                            .Given(ListWith_Forums, 3)
                                .And(EachForumContains_Topics, 2)
                                .And(ForumRepositoryIsInitialized)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderIndexView)
                                .And(ViewModel_ContainSummaries, true)

                        .WithScenario("forums each with existing topics each with existing posts")
                            .Given(ListWith_Forums, 1)
                                .And(EachForumContains_Topics, 2)
                                .And(EachTopicContains_Posts, 3)
                                .And(ForumRepositoryIsInitialized)
                            .When(IndexActionRequested)
                            .Then(ShouldReturnViewResult)
                                .And(ShouldRenderIndexView)
                                .And(ViewModel_ContainSummaries, true)
                .Execute();
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

            _repository.Stub(r => r.GetAll()).Return(_forums.ToArray());
        }

        private void IndexActionRequested()
        {
            _result = new ForumController(_repository).Index();
        }

        private void ShouldReturnViewResult()
        {
            Assert.IsNotNull(_result);
        }

        private void ShouldRenderIndexView()
        {
            Assert.That(_result.AssertViewRendered().ViewName == string.Empty);
        }

        private void ViewModel_ContainSummaries([BooleanParameterFormat("should", "should not")] bool contains)
        {
            Assert.IsNotNull(_result);
            Assert.IsNotNull(_result.ViewData);
            Assert.IsNotNull(_result.ViewData.Model);

            if (contains)
            {
                Assert.IsInstanceOf<ForumSummary[]>(_result.ViewData.Model);
                var model = _result.ViewData.Model as ForumSummary[];
                Assert.That(model.Length == _forums.Count);
            }
            else
            {
                Assert.IsNotInstanceOf<ForumSummary[]>(_result.ViewData.Model);
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

        private void Forum_WasCreatedOn_(int index, DateTime createdOn)
        {
            throw new NotImplementedException();
        }
    }
}