using System;
using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Core.Domain.Repositories;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class InputToTopicMapper : IMapper<TopicInput, Topic>
    {
        private readonly IKeyedRepository<Forum, Guid> _forumRepository;
        private readonly IKeyedRepository<Topic, Guid> _topicRepository;

        public InputToTopicMapper(IKeyedRepository<Topic, Guid> topicRepository, IKeyedRepository<Forum, Guid> forumRepository)
        {
            _forumRepository = forumRepository;
            _topicRepository = topicRepository;
        }

        public Topic Map(TopicInput source)
        {
            var topic = _topicRepository.FindBy(source.Id) ?? new Topic();

            if (topic.Forum == null || topic.Forum.Id != source.ForumId)
            {
                topic.Forum = _forumRepository.FindBy(source.ForumId);
            }

            topic.Title = source.Title;
            topic.Body = source.Body;
            topic.Closed = source.Closed;
            topic.Sticky = source.Sticky;

            return topic;
        }
    }
}