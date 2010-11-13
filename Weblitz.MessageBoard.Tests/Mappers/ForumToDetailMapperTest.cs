using NUnit.Framework;
using StoryQ;
using Weblitz.MessageBoard.Web.Models;
using Weblitz.MessageBoard.Web.Models.Mappers;

namespace Weblitz.MessageBoard.Tests.Mappers
{
    public class ForumToDetailMapperTest : MapperTestBase
    {
        private ForumDetail _detail;

        [Test]
        public void ForumToDetailMapping()
        {
            new Story("forum to detail mapping")
                .InOrderTo("check forum to detail property mappings")
                .AsA("developer")
                .IWant("to transform forum domain model to detail view model")

                        .WithScenario("forum with no topics")
                            .Given(ForumWith_Topics, 0)
                            .When(ForumIsMappedToDetail)
                            .Then(DetailIdShouldMatchForumId)
                                .And(DetailNameShouldMatchForumName)
                                .And(DetailTopicsShouldContain_Summaries, 0)

                        .WithScenario("forum with topics each with no posts")
                            .Given(ForumWith_Topics, 3)
                            .When(ForumIsMappedToDetail)
                            .Then(DetailIdShouldMatchForumId)
                                .And(DetailNameShouldMatchForumName)
                                .And(DetailTopicsShouldContain_Summaries, 3)
                .Execute();
        }

        private void ForumIsMappedToDetail()
        {
            _detail = new ForumToDetailMapper().Map(Forum);
        }

        private void DetailIdShouldMatchForumId()
        {
            Assert.That(_detail.Id == Forum.Id);
        }

        private void DetailNameShouldMatchForumName()
        {
            Assert.That(_detail.Name == Forum.Name);
        }

        private void DetailTopicsShouldContain_Summaries(int count)
        {
            Assert.That(_detail.Topics.Length == count);
        }
    }
}