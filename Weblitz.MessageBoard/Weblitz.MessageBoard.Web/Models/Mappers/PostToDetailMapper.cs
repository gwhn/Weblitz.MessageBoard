using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class PostToDetailMapper : IMapper<Post, PostDetail>
    {
        public PostDetail Map(Post source)
        {
            if (source == null) return null;

            var detail = new PostDetail
                             {
                                 Id = source.Id,
                                 Author = source.AuditInfo.CreatedBy,
                                 Body = source.Body,
                                 ForumName = source.Topic.Forum.Name,
                                 TopicTitle = source.Topic.Title
                             };

            if (source.AuditInfo.CreatedOn.HasValue)
            {
                detail.PublishedDate = source.AuditInfo.CreatedOn.Value.ToString("dd/MM/YYYY hh:mm tt");
            }

            return detail;
        }
    }
}