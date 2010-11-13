using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Tests.Fixtures
{
    public static class TopicFixtures
    {
        public static Topic TopicWithNoPostsAndNoAttachments(object index)
        {
            return new Topic
                       {
                           Title = string.Format("Title of topic {0} with no posts or attachments", index),
                           Body = string.Format("Body of topic {0} with no posts or attachments", index),
                           Closed = true,
                           Sticky = true,
                       };
        }
    }
}