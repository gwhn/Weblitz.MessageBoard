using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class TopicToDeleteItemMapper : IMapper<Topic, DeleteItem>
    {
        public DeleteItem Map(Topic source)
        {
            return new DeleteItem {Id = source.Id, Description = source.Title};
        }
    }
}