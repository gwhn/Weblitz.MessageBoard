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
            if (source == null) return null;

            var summary = new ForumSummary
                              {
                                  Id = source.Id,
                                  Name = source.Name
                              };

            var topics = source.Topics;

            summary.TopicCount = topics.Length;

            foreach (var posts in topics.Select(topic => topic.Posts))
            {
                summary.PostCount += Post.Count(posts);
            }

            return summary;
        }
    }
}