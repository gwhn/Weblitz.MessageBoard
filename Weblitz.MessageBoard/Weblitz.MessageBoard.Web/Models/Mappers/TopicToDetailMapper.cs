using System.Collections.Generic;
using System.Linq;
using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class TopicToDetailMapper : IMapper<Topic, TopicDetail>
    {
        public TopicDetail Map(Topic source)
        {
            if (source == null) return null;

            var detail = new TopicDetail
                             {
                                 Id = source.Id,
                                 Author = source.AuditInfo.CreatedBy,
                                 Body = source.Body,
                                 Forum = source.Forum.Name,
                                 Title = source.Title,
                                 NewPost = new PostInput {TopicId = source.Id}
                             };

            if (source.AuditInfo.CreatedOn.HasValue)
            {
                detail.PublishedDate = source.AuditInfo.CreatedOn.Value.ToString("dd/MM/YYYY hh:mm tt");                
            }

            var posts = source.Posts;

            var postDetails = new List<PostDetail>(posts.Length);

            postDetails.AddRange(posts.Select(post => new PostToDetailMapper().Map(post)));

            detail.Posts = postDetails.ToArray();

            return detail;
        }
    }
}