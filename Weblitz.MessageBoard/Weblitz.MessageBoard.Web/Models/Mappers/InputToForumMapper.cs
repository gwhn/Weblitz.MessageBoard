using System;
using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Core.Domain.Repositories;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class InputToForumMapper : IMapper<ForumInput, Forum>
    {
        private readonly IKeyedRepository<Forum, Guid> _forumRepository;

        public InputToForumMapper(IKeyedRepository<Forum, Guid> forumRepository)
        {
            _forumRepository = forumRepository;
        }

        public Forum Map(ForumInput source)
        {
            var forum = _forumRepository.FindBy(source.Id) ?? new Forum();

            forum.Name = source.Name;

            return forum;
        }
    }
}