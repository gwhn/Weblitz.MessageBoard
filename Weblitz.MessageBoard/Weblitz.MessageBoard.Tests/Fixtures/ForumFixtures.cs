using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Tests.Fixtures
{
    public static class ForumFixtures
    {
        public static Forum ForumWithNoTopics
        {
            get
            {
                return new Forum
                           {
                               Name = "First forum with no topics"
                           };
            }
        }
    }
}