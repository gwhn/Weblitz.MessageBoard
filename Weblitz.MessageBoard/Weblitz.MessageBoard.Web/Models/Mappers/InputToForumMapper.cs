using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class InputToForumMapper : IMapper<ForumInput, Forum>
    {
        public Forum Map(ForumInput source)
        {
            return new Forum {Name = source.Name};
        }
    }
}