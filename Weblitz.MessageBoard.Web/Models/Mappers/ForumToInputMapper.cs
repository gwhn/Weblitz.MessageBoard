using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class ForumToInputMapper : IMapper<Forum, ForumInput>
    {
        public ForumInput Map(Forum source)
        {
            return new ForumInput
                       {
                           Id = source.Id,
                           Name = source.Name
                       };
        }
    }
}