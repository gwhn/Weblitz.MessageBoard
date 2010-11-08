using System;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Core.Domain.Repositories
{
    public interface IForumRepository : IKeyedRepository<Forum, Guid>
    {

    }
}