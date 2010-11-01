using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Tests.Fixtures
{
    public static class PostFixtures
    {
        public static Post RootPostWithNoChildren(int index)
        {
            return new Post
                       {
                           Body = string.Format("Body of root post {0} with no children", index),
                           Flagged = true
                       };
        }

        public static Post BranchPostWithNoChildren(int index)
        {
            return new Post
                       {
                           Body = string.Format("Body of branch post {0} with no children", index),
                           Flagged = false
                       };
        }
    }
}