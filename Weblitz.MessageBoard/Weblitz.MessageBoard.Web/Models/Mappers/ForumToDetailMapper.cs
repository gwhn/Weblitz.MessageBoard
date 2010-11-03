using System.Collections.Generic;
using System.Linq;
using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class ForumToDetailMapper : IMapper<Forum, ForumDetail>
    {
        public ForumDetail Map(Forum source)
        {
            if (source == null) return null;

            var detail = new ForumDetail
                             {
                                 Id = source.Id,
                                 Name = source.Name
                             };

            var topics = source.Topics;

            var summaries = new List<TopicSummary>(topics.Length);

            summaries.AddRange(topics.Select(topic => new TopicToSummaryMapper().Map(topic)));

            detail.Topics = summaries.ToArray();

            return detail;
        }
    }
}