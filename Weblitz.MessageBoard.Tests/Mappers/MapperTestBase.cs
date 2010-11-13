using System.Linq;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Tests.Fixtures;

namespace Weblitz.MessageBoard.Tests.Mappers
{
    public class MapperTestBase : TestBase
    {
        protected Forum Forum;

        protected void ForumWith_Topics(int count)
        {
            Forum = ForumFixtures.ForumWithNoTopics(1);

            for (var i = 0; i < count; i++)
            {
                Forum.Add(TopicFixtures.TopicWithNoPostsAndNoAttachments(i));
            }
        }

        protected void EachTopicContains_Posts(int count)
        {
            foreach (var topic in Forum.Topics)
            {
                for (var i = 0; i < count; i++)
                {
                    topic.Add(PostFixtures.RootPostWithNoChildren(i));
                }
            }
        }

        protected void EachPostContains_Children(int count)
        {
            foreach (var post in Forum.Topics.SelectMany(topic => topic.Posts))
            {
                for (var i = 0; i < count; i++)
                {
                    post.Add(PostFixtures.BranchPostWithNoChildren(i));
                }
            }
        }
    }
}