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
            var topic = new Topic {Forum = _forumRepository.FindBy(forumId)};

            var input = new TopicToInputMapper(_forumRepository).Map(topic);

            return View(input);
        }

        //
        // POST: /Topic/Create

        [HttpPost]
        public ActionResult Create(TopicInput input)
        {
            if (ModelState.IsValid)
            {
                var topic = new InputToTopicMapper(_forumRepository).Map(input);

                _topicRepository.Save(topic);

                TempData["Message"] = string.Format("Topic {0} created successfully", topic.Title);

                return RedirectToAction("Details", new {topic.Id});
            }

            TempData["Message"] = "Failed to create topic";

            return RedirectToAction("Create", new {input.ForumId});
        }

        //
        // GET: /Topic/Edit/5432..7890

        public ActionResult Edit(Guid id)
        {
            var topic = _topicRepository.FindBy(id);

            var input = new TopicToInputMapper(_forumRepository).Map(topic);

            return View(input);
        }

        //
        // POST: /Topic/Edit/5432..9439

        [HttpPost]
        public ActionResult Edit(TopicInput input)
        {
            if (ModelState.IsValid)
            {
                var topic = new InputToTopicMapper(_forumRepository).Map(input);

                _topicRepository.Save(topic);

                TempData["Message"] = string.Format("Topic {0} updated successfully", topic.Title);

                return RedirectToAction("Details", new { topic.Id });
            }

            TempData["Message"] = "Failed to update topic";

            return View(input);
        }

        //
        // GET: /Topic/Delete/5

        public ActionResult Delete(Guid id)
        {
            var topic = _topicRepository.FindBy(id);

            var display = new TopicToDeleteItemMapper().Map(topic);

            display.CancelNavigation = new CancelNavigation("Details",
                                                            RouteData.Values["Controller"] as string,
                                                            new {id});

            return View(display);
        }

    }
}
