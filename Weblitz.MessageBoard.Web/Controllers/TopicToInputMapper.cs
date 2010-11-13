using System;
using System.Linq;
using System.Web.Mvc;
using Weblitz.MessageBoard.Core.Domain;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Core.Domain.Repositories;
using Weblitz.MessageBoard.Web.Models;

namespace Weblitz.MessageBoard.Web.Controllers
{
    public class TopicToInputMapper : IMapper<Topic, TopicInput>
    {
        private readonly IKeyedRepository<Forum, Guid> _repository;

        public TopicToInputMapper(IKeyedRepository<Forum, Guid> repository)
        {
            _repository = repository;
        }

        public TopicInput Map(Topic source)
        {
            return new TopicInput
                       {
                           Body = source.Body,
                           Sticky = source.Sticky,
                           Title = source.Title,
                           Closed = source.Closed,
                           ForumId = source.Forum.Id,
                           Forums = new SelectList(_repository.All().OrderBy(f => f.Name).ToArray(), "Id", "Name")
                       };
        }
    }
}