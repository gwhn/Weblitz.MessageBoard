using System.Linq;
using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappings
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
                summary.PostCount += posts.Length;
            }

            return summary;
        }
    }
}