using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class TopicToSummaryMapper : IMapper<Topic, TopicSummary>
    {
        public TopicSummary Map(Topic source)
        {
            if (source == null) return null;

            return new TopicSummary
                       {
                           Id = source.Id,
                           Title = source.Title,
                           Sticky = source.Sticky,
                           Closed = source.Closed,
                           PostCount = source.Posts.Length
                       };
        }
    }
}