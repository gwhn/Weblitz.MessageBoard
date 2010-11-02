using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Tests.Fixtures
{
    public static class ForumFixtures
    {
        public static Forum ForumWithNoTopics(int index)
        {
            return new Forum {Name = string.Format("Name of forum {0} with no topics", index)};
        }
    }
}