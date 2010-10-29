using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Tests.Fixtures
{
    public static class ForumFixtures
    {
        public static Forum ForumWithNoTopics
        {
            get
            {
                return new Forum {Name = "Name of forum with no topics"};
            }
        }
    }
}