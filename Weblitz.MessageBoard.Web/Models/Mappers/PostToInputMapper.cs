using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class PostToInputMapper : IMapper<Post, PostInput>
    {
        public PostInput Map(Post source)
        {
            var input = new PostInput
                            {
                                Id = source.Id,
                                TopicId = source.Topic.Id,
                                Body = source.Body
                            };

            if (source.Parent != null)
            {
                input.ParentId = source.Parent.Id;
            }

            return input;
        }
    }
}