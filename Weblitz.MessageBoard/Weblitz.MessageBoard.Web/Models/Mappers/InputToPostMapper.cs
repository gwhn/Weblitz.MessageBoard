using System;
using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Core.Domain.Repositories;

namespace Weblitz.MessageBoard.Web.Models.Mappers
{
    public class InputToPostMapper : IMapper<PostInput, Post>
    {
        private readonly IKeyedRepository<Post, Guid> _postRepository;
        private readonly IKeyedRepository<Topic, Guid> _topicRepository;

        public InputToPostMapper(IKeyedRepository<Post, Guid> postRepository, IKeyedRepository<Topic, Guid> topicRepository)
        {
            _postRepository = postRepository;
            _topicRepository = topicRepository;
        }

        public Post Map(PostInput source)
        {
            var post = new Post
                           {
                               Body = source.Body,
                               Topic = _topicRepository.FindBy(source.TopicId)
                           };

            if (source.ParentId.HasValue)
            {
                post.Parent = _postRepository.FindBy(source.ParentId.Value);
            }
 
            return post;
        }
    }
}