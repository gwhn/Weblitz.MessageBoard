using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Weblitz.MessageBoard.Core.Domain.Repositories;
using Weblitz.MessageBoard.Web.Models;
using Weblitz.MessageBoard.Web.Models.Mappers;

namespace Weblitz.MessageBoard.Web.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForumRepository _repository;

        public ForumController(IForumRepository repository)
        {
            _repository = repository;
        }

        //
        // GET: /Forum/

        public ViewResult Index()
        {
            var forums = _repository.GetAll();

            var summaries = new List<ForumSummary>(forums.Length);

            summaries.AddRange(forums.Select(forum => new ForumToSummaryMapper().Map(forum)));

            return View(summaries.ToArray());
        }

    }
}
