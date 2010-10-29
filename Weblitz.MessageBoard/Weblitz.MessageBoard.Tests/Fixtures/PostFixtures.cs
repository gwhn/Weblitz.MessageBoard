using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Tests.Fixtures
{
    public static class PostFixtures
    {
        public static Post PostWithNoChildren
        {
            get
            {
                return new Post
                           {
                               Body = "Body of post with no children",
                               Flagged = true
                           };
            }
        }
    }
}