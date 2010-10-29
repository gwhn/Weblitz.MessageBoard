using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Tests.Fixtures
{
    public static class TopicFixtures
    {
        public static Topic TopicWithNoPostsAndNoAttachments
        {
            get
            {
                return new Topic
                           {
                               Title = "Title of topic with no posts or attachments",
                               Body = "Body of topic with no posts or attachments",
                               Closed = true,
                               Sticky = true,
                           };
            }
        }
    }
}