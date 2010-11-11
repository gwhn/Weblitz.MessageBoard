using NUnit.Framework;
using StoryQ;
using Weblitz.MessageBoard.Web.Models;
using Weblitz.MessageBoard.Web.Models.Mappers;

namespace Weblitz.MessageBoard.Tests.Mappers
{
    [TestFixture]
    public class ForumToSummaryMapperTest : MapperTestBase
    {
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
                            .When(ForumIsMappedToSummary)
                            .Then(SummaryIdShouldMatchForumId)
                                .And(SummaryNameShouldMatchForumName)
                                .And(SummaryTopicCountShouldEqual_, 0)
                                .And(SummaryPostCountShouldEqual_, 0)

                        .WithScenario("forum with topics each with no posts")
                            .Given(ForumWith_Topics, 3)
                            .When(ForumIsMappedToSummary)
                            .Then(SummaryIdShouldMatchForumId)
                                .And(SummaryNameShouldMatchForumName)
                                .And(SummaryTopicCountShouldEqual_, 3)
                                .And(SummaryPostCountShouldEqual_, 0)

                        .WithScenario("forum with topics each with posts")
                            .Given(ForumWith_Topics, 2)
                                .And(EachTopicContains_Posts, 3)
                            .When(ForumIsMappedToSummary)
                            .Then(SummaryIdShouldMatchForumId)
                                .And(SummaryNameShouldMatchForumName)
                                .And(SummaryTopicCountShouldEqual_, 2)
                                .And(SummaryPostCountShouldEqual_, 6)

                        .WithScenario("forum with topics each with posts each with child posts")
                            .Given(ForumWith_Topics, 1)
                                .And(EachTopicContains_Posts, 2)
                                .And(EachPostContains_Children, 3)
                            .When(ForumIsMappedToSummary)
                            .Then(SummaryIdShouldMatchForumId)
                                .And(SummaryNameShouldMatchForumName)
                                .And(SummaryTopicCountShouldEqual_, 1)
                                .And(SummaryPostCountShouldEqual_, 2)
                .Execute();
        }

        private void ForumIsMappedToSummary()
        {
            _summary = new ForumToSummaryMapper().Map(Forum);
        }

        private void SummaryIdShouldMatchForumId()
        {
            Assert.That(_summary.Id == Forum.Id);
        }

        private void SummaryNameShouldMatchForumName()
        {
            Assert.That(_summary.Name == Forum.Name);
        }

        private void SummaryTopicCountShouldEqual_(int count)
        {
            Assert.That(_summary.TopicCount == count);
        }

        private void SummaryPostCountShouldEqual_(int count)
        {
            Assert.That(_summary.PostCount == count);
        }
    }
}