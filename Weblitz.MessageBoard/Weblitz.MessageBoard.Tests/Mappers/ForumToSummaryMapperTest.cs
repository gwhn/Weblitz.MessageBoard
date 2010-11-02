using System;
using System.Linq;
using NUnit.Framework;
using StoryQ;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Tests.Fixtures;
using Weblitz.MessageBoard.Web.Models;
using Weblitz.MessageBoard.Web.Models.Mappers;

namespace Weblitz.MessageBoard.Tests.Mappers
{
    [TestFixture]
    public class ForumToSummaryMapperTest : TestBase
    {
        private Forum _forum;
        private ForumSummary _summary;

        [Test]
        public void ForumToSummaryMapping()
        {
            new Story("forum to summary mapping")
                .InOrderTo("check forum to summary property mappings")
                .AsA("developer")
                .IWant("to transform forum domain model to summary view model")

                        .WithScenario("forum with no topics")
                            .Given(ForumWith_Topics, 0)
                            .When(ForumIsMapped)
                            .Then(SummaryIdShouldMatchForumId)
                                .And(SummaryNameShouldMatchForumName)
                                .And(SummaryTopicCountShouldEqual_, 0)
                                .And(SummaryPostCountShouldEqual_, 0)

                        .WithScenario("forum with topics each with no posts")
                            .Given(ForumWith_Topics, 3)
                            .When(ForumIsMapped)
                            .Then(SummaryIdShouldMatchForumId)
                                .And(SummaryNameShouldMatchForumName)
                                .And(SummaryTopicCountShouldEqual_, 3)
                                .And(SummaryPostCountShouldEqual_, 0)

                        .WithScenario("forum with topics each with posts")
                            .Given(ForumWith_Topics, 2)
                                .And(EachTopicContains_Posts, 3)
                            .When(ForumIsMapped)
                            .Then(SummaryIdShouldMatchForumId)
                                .And(SummaryNameShouldMatchForumName)
                                .And(SummaryTopicCountShouldEqual_, 2)
                                .And(SummaryPostCountShouldEqual_, 6)

                        .WithScenario("forum with topics each with posts each with child posts")
                            .Given(ForumWith_Topics, 1)
                                .And(EachTopicContains_Posts, 2)
                                .And(EachPostContains_Children, 3)
                            .When(ForumIsMapped)
                            .Then(SummaryIdShouldMatchForumId)
                                .And(SummaryNameShouldMatchForumName)
                                .And(SummaryTopicCountShouldEqual_, 1)
                                .And(SummaryPostCountShouldEqual_, 8)
                .Execute();
        }

        private void ForumWith_Topics(int count)
        {
            _forum = ForumFixtures.ForumWithNoTopics(1);

            for (var i = 0; i < count; i++)
            {
                _forum.Add(TopicFixtures.TopicWithNoPostsAndNoAttachments(i));
            }
        }

        private void ForumIsMapped()
        {
            _summary = new ForumToSummaryMapper().Map(_forum);
        }

        private void SummaryIdShouldMatchForumId()
        {
            Assert.That(_summary.Id == _forum.Id);
        }

        private void SummaryNameShouldMatchForumName()
        {
            Assert.That(_summary.Name == _forum.Name);
        }

        private void SummaryTopicCountShouldEqual_(int count)
        {
            Assert.That(_summary.TopicCount == count);
        }

        private void SummaryPostCountShouldEqual_(int count)
        {
            Assert.That(_summary.PostCount == count);
        }

        private void EachTopicContains_Posts(int count)
        {
            foreach (var topic in _forum.Topics)
            {
                for (var i = 0; i < count; i++)
                {
                    topic.Add(PostFixtures.RootPostWithNoChildren(i));
                }                
            }
        }

        private void EachPostContains_Children(int count)
        {
            foreach (var post in _forum.Topics.SelectMany(topic => topic.Posts))
            {
                for (var i = 0; i < count; i++)
                {
                    post.Add(PostFixtures.BranchPostWithNoChildren(i));
                }
            }
        }
    }
}