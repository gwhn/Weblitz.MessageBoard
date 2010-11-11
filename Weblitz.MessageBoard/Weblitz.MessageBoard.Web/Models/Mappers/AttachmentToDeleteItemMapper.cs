using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class AttachmentToDeleteItemMapper : IMapper<Attachment, DeleteItem>
    {
        public DeleteItem Map(Attachment source)
        {
            return new DeleteItem{Id = source.Id, Description = source.FileName};
        }
    }
}