using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Core.Domain.Repositories;
using Weblitz.MessageBoard.Web.Models.Mappers;

namespace Weblitz.MessageBoard.Web.Controllers
{
    public class TopicController : Controller
    {
        private readonly IKeyedRepository<Topic, Guid> _repository;

        public TopicController(IKeyedRepository<Topic, Guid> repository)
        {
            _repository = repository;
        }

        //
        // GET: /Topic/Details/1234..4311

        public ActionResult Details(Guid id)
        {
            var topic = _repository.FindBy(id);

            if (topic == null)
            {
                TempData["Message"] = string.Format("No topic matches ID {0}", id);

                return RedirectToAction("Index", "Forum");
            }

            var detail = new TopicToDetailMapper().Map(topic);

            return View(detail);
        }

    }
}
