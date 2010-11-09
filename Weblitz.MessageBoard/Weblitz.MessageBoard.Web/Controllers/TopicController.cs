using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Core.Domain.Repositories;
using Weblitz.MessageBoard.Web.Models;
using Weblitz.MessageBoard.Web.Models.Mappers;

namespace Weblitz.MessageBoard.Web.Controllers
{
    public class TopicController : Controller
    {
        private readonly IKeyedRepository<Topic, Guid> _topicRepository;
        private readonly IKeyedRepository<Forum, Guid> _forumRepository;

        public TopicController(IKeyedRepository<Topic, Guid> topicRepository, IKeyedRepository<Forum, Guid> forumRepository)
        {
            _topicRepository = topicRepository;
            _forumRepository = forumRepository;
        }

        //
        // GET: /Topic/Details/1234..4311

        public ActionResult Details(Guid id)
        {
            var topic = _topicRepository.FindBy(id);

            if (topic == null)
            {
                TempData["Message"] = string.Format("No topic matches ID {0}", id);

                return RedirectToAction("Index", "Forum");
            }

            var detail = new TopicToDetailMapper().Map(topic);

            return View(detail);
        }

        //
        // GET: /Topic/Create

        public ViewResult Create(Guid forumId)
        {
            var input = new TopicInput
                            {
                                Forums = new SelectList(_forumRepository.All().OrderBy(f => f.Name).ToList(), "Id", "Name")
                            };

            return View(input);
        }

    }
}
