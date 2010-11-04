using System;
using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class ForumToDeleteItemMapper : IMapper<Forum, DeleteItem>
    {
        public DeleteItem Map(Forum source)
        {
            return new DeleteItem {Id = source.Id, Description = source.Name};
        }
    }
}