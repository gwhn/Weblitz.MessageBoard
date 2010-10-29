using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Tests.Fixtures
{
    public static class PostFixtures
    {
        public static Post RootPostWithNoChildren
        {
            get
            {
                return new Post
                           {
                               Body = "Body of root post with no children",
                               Flagged = true
                           };
            }
        }

        public static Post BranchPostWithNoChildren
        {
            get
            {
                return new Post
                           {
                               Body = "Body of branch post with no children",
                               Flagged = true
                           };
            }
        }
    }
}