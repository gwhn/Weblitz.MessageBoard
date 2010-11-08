using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Core.Domain.Repositories;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate
{
    public class ForumRepository : Repository<Forum>, IForumRepository
    {
        
    }
}