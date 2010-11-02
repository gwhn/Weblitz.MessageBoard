using System.Collections.Generic;
using System.Linq;
using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class ForumToSummaryMapper : IMapper<Forum, ForumSummary>
    {
        public ForumSummary Map(Forum source)
        {
            var summary = new ForumSummary
                              {
                                  Id = source.Id,
                                  Name = source.Name
                              };

            var topics = source.Topics;

            summary.TopicCount = topics.Length;

            foreach (var posts in topics.Select(topic => topic.Posts))
            {
                summary.PostCount += Count(posts);
            }

            return summary;
        }

        private static int Count(ICollection<Post> posts)
        {
            return posts.Count + posts.Where(post => post.Children.Length > 0).Sum(post => Count(post.Children));
        }
    }
}