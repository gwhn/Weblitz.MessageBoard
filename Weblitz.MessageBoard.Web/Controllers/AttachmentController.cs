using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Core.Domain.Repositories;
using Weblitz.MessageBoard.Web.Models;
using Weblitz.MessageBoard.Web.Models.Mappers;

namespace Weblitz.MessageBoard.Web.Controllers
{
    public class AttachmentController : Controller
    {
        private readonly IKeyedRepository<Attachment, Guid> _attachmentRepository;
        private readonly IKeyedRepository<Entry, Guid> _entryRepository;

        private const string AttachmentsPath = "~/App_Data/Attachments";

        public AttachmentController(IKeyedRepository<Attachment, Guid> attachmentRepository,
                                    IKeyedRepository<Entry, Guid> entryRepository)
        {
            _attachmentRepository = attachmentRepository;
            _entryRepository = entryRepository;
        }

        //
        // GET: /Attachment/Download/4312..7897

        public FilePathResult Download(Guid id)
        {
            var attachment = _attachmentRepository.FindBy(id);

            var path = Path.Combine(Server.MapPath(AttachmentsPath), attachment.Id.ToString());

            return File(path, attachment.ContentType, attachment.FileName);
        }

        //
        // GET: /Attachment/Create

        public ViewResult Create(Guid entryId)
        {
            var attachment = new Attachment {Entry = _entryRepository.FindBy(entryId)};

            return View(attachment);
        }

        //
        // POST: /Attachment/Create

        [HttpPost]
        public ActionResult Create(Guid entryId, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);

                    var entry = _entryRepository.FindBy(entryId);

                    var attachment = new Attachment
                                         {
                                             Entry = entry,
                                             FileName = fileName,
                                             ContentLength = file.ContentLength,
                                             ContentType = file.ContentType
                                         };

                    _attachmentRepository.Save(attachment);

                    var path = Path.Combine(Server.MapPath(AttachmentsPath), attachment.Id.ToString());

                    file.SaveAs(path);

                    TempData["Message"] = string.Format("Attachment {0} created successfully", attachment.FileName);

                    var controller = "Topic";

                    if (entry.Actual is Post)
                    {
                        controller = "Post";
                    }

                    return RedirectToAction("Details", controller, new {attachment.Entry.Id});
                }
            }

            TempData["Message"] = "Failed to create attachment";

            return RedirectToAction("Create", new {entryId});
        }

        //
        // GET: /Attachment/Delete/5876..8090

        public ViewResult Delete(Guid id)
        {
            var attachment = _attachmentRepository.FindBy(id);

            var display = new AttachmentToDeleteItemMapper().Map(attachment);

            var controller = "Topic";

            if (attachment.Entry.Actual is Post)
            {
                controller = "Post";
            }

            display.CancelNavigation = new CancelNavigation("Details", controller, new {attachment.Entry.Id});

            return View(display);
        }

        //
        // POST: /Attachment/Delete/5432..0989

        [HttpPost, ActionName("Delete")]
        public ActionResult Destroy(Guid id)
        {
            var attachment = _attachmentRepository.FindBy(id);

            var path = Path.Combine(Server.MapPath(AttachmentsPath), attachment.Id.ToString());

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _attachmentRepository.Delete(attachment);

            TempData["Message"] = string.Format("Attachment {0} deleted successfully", attachment.FileName);

            var controller = "Topic";

            if (attachment.Entry.Actual is Post)
            {
                controller = "Post";
            }

            return RedirectToAction("Details", controller, new {attachment.Entry.Id});
        }
    }
}