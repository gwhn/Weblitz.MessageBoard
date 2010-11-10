using System.Collections.Generic;
using System.Linq;
using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class PostToDetailMapper : IMapper<Post, PostDetail>
    {
        public PostDetail Map(Post source)
        {
            return Map(source, 0);
        }

        public PostDetail Map(Post source, int level)
        {
            if (source == null) return null;

            var detail = new PostDetail
                             {
                                 Id = source.Id,
                                 Author = source.AuditInfo.CreatedBy,
                                 Body = source.Body,
                                 ForumName = source.Topic.Forum.Name,
                                 TopicId = source.Topic.Id,
                                 TopicTitle = source.Topic.Title,
                                 Level = level++
                             };

            if (source.AuditInfo.CreatedOn.HasValue)
            {
                detail.PublishedDate = source.AuditInfo.CreatedOn.Value.ToString("dd/MM/yyyy hh:mm tt");
            }

            var children = source.Children;

            var childDetails = new List<PostDetail>(children.Length);

            childDetails.AddRange(children.Select(child => new PostToDetailMapper().Map(child, level)));

            detail.Children = childDetails.ToArray();

            return detail;
        }
    }
}