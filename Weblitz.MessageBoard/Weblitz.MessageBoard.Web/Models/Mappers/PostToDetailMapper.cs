using System;
using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class PostToDetailMapper : IMapper<Post, PostDetail>
    {
        public PostDetail Map(Post source)
        {
            throw new NotImplementedException();
        }
    }
}