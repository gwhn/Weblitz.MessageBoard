using System;
using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Core.Domain.Repositories;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class InputToTopicMapper : IMapper<TopicInput, Topic>
    {
        private readonly IKeyedRepository<Forum, Guid> _repository;

        public InputToTopicMapper(IKeyedRepository<Forum, Guid> repository)
        {
            _repository = repository;
        }

        public Topic Map(TopicInput source)
        {
            return new Topic
                       {
                           Title = source.Title,
                           Body = source.Body,
                           Closed = source.Closed,
                           Sticky = source.Sticky,
                           Forum = _repository.FindBy(source.ForumId)
                       };
        }
    }
}