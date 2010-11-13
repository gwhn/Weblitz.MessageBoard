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
    public class PostController : Controller
    {
        private readonly IKeyedRepository<Post, Guid> _postRepository;
        private readonly IKeyedRepository<Topic, Guid> _topicRepository;

        public PostController(IKeyedRepository<Post, Guid> postRepository, IKeyedRepository<Topic, Guid> topicRepository)
        {
            _postRepository = postRepository;
            _topicRepository = topicRepository;
        }

        //
        // GET: /Post/Details/1234..4311

        public ActionResult Details(Guid id)
        {
            var post = _postRepository.FindBy(id);

            if (post == null)
            {
                TempData["Message"] = string.Format("No post matches ID {0}", id);

                return RedirectToAction("Index", "Forum");
            }

            var detail = new PostToDetailMapper().Map(post);

            return View(detail);
        }

        //
        // GET: /Post/Create?TopicId=1234..7890
        // GET: /Post/Create?TopicId=1234..7890&ParentId=4312..0987

        public ViewResult Create(Guid topicId, Guid? parentId)
        {
            var topic = _topicRepository.FindBy(topicId);

            var post = new Post {Topic = topic};

            if (parentId.HasValue)
            {
                post.Parent = _postRepository.FindBy(parentId.Value);
            }

            var input = new PostToInputMapper().Map(post);

            return View(input);
        }

        //
        // POST: /Post/Create

        [HttpPost]
        public ActionResult Create(PostInput input)
        {
            if (ModelState.IsValid)
            {
                var post = new InputToPostMapper(_postRepository, _topicRepository).Map(input);

                _postRepository.Save(post);

                TempData["Message"] = string.Format("Post {0} created successfully", post.Id);

                return RedirectToAction("Details", new {post.Id});
            }

            TempData["Message"] = "Failed to create post";

            return RedirectToAction("Create", new {input.TopicId});
        }

        //
        // GET: /Post/Edit/5432..7890

        public ActionResult Edit(Guid id)
        {
            var post = _postRepository.FindBy(id);

            var input = new PostToInputMapper().Map(post);

            return View(input);
        }

        //
        // POST: /Post/Edit/5432..9439

        [HttpPost]
        public ActionResult Edit(PostInput input)
        {
            if (ModelState.IsValid)
            {
                var post = new InputToPostMapper(_postRepository, _topicRepository).Map(input);

                _postRepository.Save(post);

                TempData["Message"] = string.Format("Post {0} updated successfully", post.Id);

                return RedirectToAction("Details", new {post.Id});
            }

            TempData["Message"] = "Failed to update post";

            return View(input);
        }

        //
        // GET: /Post/Delete/5876..8090

        public ActionResult Delete(Guid id)
        {
            var post = _postRepository.FindBy(id);

            var display = new PostToDeleteItemMapper().Map(post);

            display.CancelNavigation = new CancelNavigation("Details",
                                                            RouteData.Values["Controller"] as string,
                                                            new {id});

            return View(display);
        }

        //
        // POST: /Post/Delete/5432..0989

        [HttpPost, ActionName("Delete")]
        public ActionResult Destroy(Guid id)
        {
            var post = _postRepository.FindBy(id);

            _postRepository.Delete(post);

            TempData["Message"] = string.Format("Post {0} deleted successfully", post.Id);

            return RedirectToAction("Details", "Topic", new {post.Topic.Id});
        }
    }
}