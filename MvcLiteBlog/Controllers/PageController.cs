using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiteBlog.Common;
using MvcLiteBlog.Attributes;
using MvcLiteBlog.BlogEngine;
using MvcLiteBlog.Models;

namespace MvcLiteBlog.Controllers
{
    public class PageController : Controller
    {
        //
        // GET: /Page/

        public ActionResult Index(string id)
        {
            Page page = PageComp.Load(id);
            return View(page);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Manage()
        {
            var model = PageComp.GetPages();
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Compose(string id)
        {
            string fileId = id;
            Page page = new Page();
            if (!string.IsNullOrEmpty(fileId))
            {
                page = PageComp.Load(fileId);
            }

            var model = new ComposePageModel
            {
                FileId = page.FileId,
                Title = page.Title,
                Contents = page.Body
            };

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Delete(string id)
        {
            PageComp.Delete(id);
            TempData["Message"] = "页面已删除";
            return RedirectToAction("Manage");
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Save(ComposePageModel pageModel)
        {
            Page page = new Page
            {
                FileId = pageModel.FileId,
                Title = pageModel.Title,
                Body = pageModel.Contents
            };

            

            // Ajax call, return Json message
            if (string.IsNullOrEmpty(page.FileId))
            {
                // unique stuff.
                page.FileId = Guid.NewGuid().ToString();
            }

            page.Body = page.Body ?? string.Empty;

            PageComp.Save(page);

            DateTime now =
                LocalTime.GetCurrentTime(TimeZoneInfo.FindSystemTimeZoneById(SettingsComp.GetSettings().Timezone));
            
            StringWriter sw = new StringWriter();
            IView view = new RazorView(this.ControllerContext, "~/Views/Shared/AutoSaveControl.cshtml", null, false, null);
            this.ViewData.Model = now;
            ViewContext viewContext = new ViewContext(this.ControllerContext, view, this.ViewData, this.TempData, sw);
            view.Render(viewContext, sw);
            
            // PartialViewResult result = RenderViewToString this.PartialView("AutoSaveControl", now);
            return Json(new SavePageResultModel { FileId = page.FileId, Content = sw.ToString() });
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        [MultiButton(FormName = "Publish", FormValue = "发布")]
        public ActionResult Publish(ComposePageModel pageModel)
        {
            // return if invalid model
            if (!ModelState.IsValid)
                return View(pageModel);

            Page page = new Page
            {
                FileId = pageModel.FileId,
                Title = pageModel.Title,
                Body = pageModel.Contents
            };

            // Save the new page
            PageComp.Publish(page);

            TempData["Message"] = "页面已发布";
            return this.RedirectToAction("Manage");
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        [MultiButton(FormName = "Close", FormValue = "关闭")]
        public ActionResult Close(Page page)
        {
            return this.RedirectToAction("Manage");
        }

    }
}
