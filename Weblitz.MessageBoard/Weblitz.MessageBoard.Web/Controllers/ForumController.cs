using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Core.Domain.Repositories;
using Weblitz.MessageBoard.Web.Models;
using Weblitz.MessageBoard.Web.Models.Mappers;

namespace Weblitz.MessageBoard.Web.Controllers
{
    public class ForumController : Controller
    {
        private readonly IKeyedRepository<Forum, Guid> _repository;

        public ForumController(IKeyedRepository<Forum, Guid> repository)
        {
            _repository = repository;
        }

        //
        // GET: /Forum/

        public ViewResult Index()
        {
            var forums = _repository.All().ToArray();

            var summaries = new List<ForumSummary>(forums.Length);

            summaries.AddRange(forums.Select(forum => new ForumToSummaryMapper().Map(forum)));

            return View(summaries.ToArray());
        }

        //
        // GET: /Forum/Details/5945..4340

        public ActionResult Details(Guid id)
        {
            var forum = _repository.FindBy(id);

            if (forum == null)
            {
                TempData["Message"] = string.Format("No forum matches ID {0}", id);

                return RedirectToAction("Index");
            }

            var details = new ForumToDetailMapper().Map(forum);

            return View(details);
        }

        //
        // GET: /Forum/Create

        public ViewResult Create()
        {
            var input = new ForumInput();

            return View(input);
        }

        //
        // POST: /Forum/Create

        [HttpPost]
        public ActionResult Create(ForumInput input)
        {
            if (ModelState.IsValid)
            {
                var forum = new InputToForumMapper().Map(input);

                _repository.Save(forum);

                TempData["Message"] = string.Format("Forum {0} created successfully", forum.Name);

                return RedirectToAction("Details", new {forum.Id});
            }

            TempData["Message"] = "Failed to create forum";

            return View(new ForumInput());
        }

        //
        // GET: /Forum/Edit/5432..8558

        public ViewResult Edit(Guid id)
        {
            var forum = _repository.FindBy(id);

            var input = new ForumToInputMapper().Map(forum);

            return View(input);
        }

        //
        // POST: /Forum/Edit

        [HttpPost]
        public ActionResult Edit(ForumInput input)
        {
            if (ModelState.IsValid)
            {
                var forum = new InputToForumMapper().Map(input);

                _repository.Save(forum);

                TempData["Message"] = string.Format("Forum {0} updated successfully", forum.Name);
            
                return RedirectToAction("Details", new {forum.Id});
            }

            TempData["Message"] = "Failed to update forum";

            _repository.FindBy(input.Id);

            return View(input);
        }

        //
        // GET: /Forum/Delete/4314..9804

        public ViewResult Delete(Guid id)
        {
            var forum = _repository.FindBy(id);

            var display = new ForumToDeleteItemMapper().Map(forum);

            display.CancelNavigation = new CancelNavigation("Details",
                                                            RouteData.Values["Controller"] as string,
                                                            new {id});

            return View(display);
        }

        //
        // POST: /Forum/Delete

        [HttpPost, ActionName("Delete")]
        public ActionResult Destroy(Guid id)
        {
            var forum = _repository.FindBy(id);

            _repository.Delete(forum);

            TempData["Message"] = "Forum {0} deleted successfully";

            return RedirectToAction("Index");
        }

    }
}
