using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class PostToDeleteItemMapper : IMapper<Post, DeleteItem>
    {
        public DeleteItem Map(Post source)
        {
            return new DeleteItem {Id = source.Id, Description = source.Body};
        }
    }
}