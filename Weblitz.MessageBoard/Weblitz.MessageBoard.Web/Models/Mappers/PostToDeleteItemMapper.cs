using System;
using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class PostToDeleteItemMapper : IMapper<Post, DeleteItem>
    {
        public DeleteItem Map(Post source)
        {
            throw new NotImplementedException();
        }
    }
}