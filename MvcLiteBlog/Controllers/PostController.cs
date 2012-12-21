// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostController.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The post controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using LiteBlog.Common;

    using MvcLiteBlog.Attributes;
    using MvcLiteBlog.BlogEngine;
    using MvcLiteBlog.Models;

    /// <summary>
    /// The post controller.
    /// </summary>
    public class PostController : Controller
    {
        #region Public Methods and Operators

        /// <summary>
        /// Close the form without saving
        /// </summary>
        /// <param name="model">
        /// The Post
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        [MultiButton(FormName = "Close", FormValue = "关闭")]
        public ActionResult Close(ComposePostModel model)
        {
            this.Session.Remove("Post");
            return this.RedirectToAction("ManageDraft");
        }

        /// <summary>
        /// Compose action
        /// </summary>
        /// <param name="id">
        /// The id
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Compose(string id)
        {
            DraftPost post = new DraftPost();

            if (!string.IsNullOrEmpty(id))
            {
                post = DraftComp.Load(id);
                this.Session["Post"] = post;
            }
            else
            {
                if (this.Session["Post"] == null)
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(SettingsComp.GetSettings().Timezone);
                    post.Time = LocalTime.GetCurrentTime(tzi);
                    post.Type = PostType.New;
                    this.Session["Post"] = post;
                }
                else
                {
                    post = this.Session["Post"] as DraftPost;
                }
            }

            ComposePostModel model = new ComposePostModel();
            model.Title = post.Title;
            model.PublishDate = post.Time.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            List<string> catList = new List<string>();
            if (!string.IsNullOrEmpty(post.CatID))
            {
                string[] catIDs = post.CatID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string catID in catIDs)
                {
                    catList.Add(catID);
                }
            }

            model.CatID = catList;
            model.Content = post.Contents;
            model.DraftID = post.DraftID;
            model.Post = post;

            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            this.ViewData.Model = model;
            return this.View();
        }

        /// <summary>
        /// Is it obsolete??
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult Correct()
        {
            // string files = "Files: <br />";
            // foreach (PostInfo pi in BlogComp.GetPosts())
            // {
            // Post post = PostComp.GetPost(pi.FileID, true, false);
            // CodeBlock codeBlock = new CodeBlock(post.Contents);
            // if (codeBlock.Encode)
            // {
            // codeBlock.GetSnippets();
            // codeBlock.ReplaceSnippets();
            // post.Contents = codeBlock.HighlightedText;
            // PostComp.Save(post);
            // files += post.FileID + "<br /> ";
            // }
            // }

            // TempData["Files"] = files;
            ////Post post = PostComp.GetPost("Path-of-TreeNode", true);
            ////CodeBlock codeBlock = new CodeBlock(post.Contents, "code");
            ////codeBlock.GetSnippets();
            ////codeBlock.ReplaceSnippets();
            ////post.Contents = codeBlock.HighlightedText;
            ////PostComp.Save(post);
            return this.RedirectToAction("Manage");
        }

        /// <summary>
        /// Delete a post with a FileID
        /// </summary>
        /// <param name="id">
        /// File ID
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Delete(string id)
        {
            List<PostInfo> posts = BlogComp.GetPosts();
            int pageSize = 10;
            int index = posts.FindIndex(p => p.FileID == id);

            int pageIndex; // pagesize hardcoded sorry

            pageIndex = (index / pageSize) + 1;

            int totalPages = ((posts.Count - 1) / pageSize) + 1;
            if ((posts.Count - 1) % pageSize == 0)
            {
                totalPages = (posts.Count - 1) / pageSize;
            }

            if (pageIndex > totalPages)
            {
                pageIndex = totalPages;
            }

            PostInfo post = posts.Single(p => p.FileID == id);
            if (post != null)
            {
                PublisherComp.Unpublish(post);

                // BlogComp.Delete(id);
            }

            this.TempData["Message"] = "Post is successfully deleted";
            return this.RedirectToAction("Manage", new { page = pageIndex });
        }

        /// <summary>
        /// The delete attachment.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult DeleteAttachment(string id)
        {
            Post post = this.Session["Post"] as Post;
            List<Document> docs = new List<Document>();
            if (post != null && !string.IsNullOrEmpty(id))
            {
                string url = this.Url.Content(string.Format("~/Docs/{0}", id));
                DraftComp.RemoveDocument(url);
                Document remove = post.Documents.Single(d => d.Path == url);
                if (remove != null)
                {
                    post.Documents.Remove(remove);
                }

                docs = post.Documents;

                this.Session["Post"] = post;
            }

            return this.PartialView("AttachmentControl", docs);
        }

        /// <summary>
        /// Delete a draft with draft id
        /// </summary>
        /// <param name="id">
        /// Draft ID
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult DeleteDraft(string id)
        {
            DraftComp.Delete(id);
            this.TempData["Message2"] = "Post is successfully deleted";
            return this.RedirectToAction("ManageDraft");
        }

        /// <summary>
        /// Gets a comment
        /// Called from PostControl from user pages
        /// </summary>
        /// <param name="id">
        /// The id
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult GetComments(string id)
        {
            this.Response.Expires = 0;

            // Post post = PostComp.Load(id);
            Post post = PostComp.GetPost(id);

            this.ViewData.Model = post.Comments;
            return this.PartialView("CommentControl");
        }

        /// <summary>
        /// Permalink page
        /// Called from users
        /// </summary>
        /// <param name="id">
        /// File ID
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult Index(string id)
        {
            string id2;
            if (!BlogComp.PostExists(id, out id2))
            {
                throw new Exception("Sorry, the post could not be found. The post may be deleted or the URL is wrong");
            }

            if (id != id2)
            {
                return this.RedirectToAction("Index", new { id = id2 });
            }

            Post post = PostComp.GetPost(id);

            StatComp.IncrementPosts(id);
            if (post == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            this.ViewData.Model = post;
            return this.View();
        }

        /// <summary>
        /// Gets a list of published posts with paging enabled
        /// Gets a list of draft posts
        /// </summary>
        /// <param name="page">
        /// pageIndex starting from 0
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Manage(int? page)
        {
            int pageSize = 20;
            int pageIndex = page ?? 0;
            int totalRecords = 0;

            List<PostInfo> posts = BlogComp.GetPosts(true);
            totalRecords = posts.Count<PostInfo>();

            //// deleting the last post of 61 records can 
            //// lead to pageIndex > totalPages
            // int totalPages = totalRecords / pageSize;
            // if (totalRecords % pageSize != 0)
            // totalPages++;
            // if (pageIndex > (totalPages - 1))
            // pageIndex = totalPages - 1;
            this.ViewBag.PageSize = pageSize;
            this.ViewBag.PageIndex = pageIndex;
            this.ViewBag.TotalRecords = totalRecords;

            // posts = posts.Skip<PostInfo>(pageIndex * pageSize)
            // .Take<PostInfo>(pageSize)
            // .ToList<PostInfo>();
            return View(posts);
        }

        /// <summary>
        /// Gets a list of draft posts
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult ManageDraft()
        {
            List<PostInfo> posts = DraftComp.GetDrafts();
            return View(posts);
        }

        /// <summary>
        /// Post Control
        /// </summary>
        /// <param name="id">
        /// The id
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult PostControl(string id)
        {
            Post post = PostComp.GetPost(id);

            if (string.IsNullOrEmpty(post.Author))
            {
                post.Author = AuthorComp.GetDefaultAuthor().Name;
            }

            this.ViewData.Model = post;
            return this.PartialView("PostControl");
        }

        /// <summary>
        /// Preview post
        /// </summary>
        /// <param name="model">
        /// The Post
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        [MultiButton(FormName = "Preview", FormValue = "预览")]
        public ActionResult Preview(ComposePostModel model)
        {
            this.UpdatePost(model);
            DraftPost post = model.Post;
            post.CategoriesText = PostComp.GetCategories(post.CatID);

            // get current user
            string author = this.User.Identity.Name;
            post.TimeText = PostComp.GetTime(post.Time, author);
            return View("Preview", model.Post);
        }

        /// <summary>
        /// Publish a post
        /// Private method because it is called internally?
        /// </summary>
        /// <param name="model">
        /// The Post
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        [MultiButton(FormName = "Publish", FormValue = "发布")]
        public ActionResult Publish(ComposePostModel model)
        {
            this.UpdatePost(model);

            if (this.ModelState.IsValid)
            {
                model.Post = this.SavePost(model.Post);
                PublisherComp.Publish(model.Post);
                this.Session.Remove("Post");
                this.TempData["Message"] = "发布成功";
                return this.RedirectToAction("Manage");
            }

            this.ViewData.Model = model;
            return this.View();
        }

        /// <summary>
        /// Saves a post
        /// Internal method
        /// </summary>
        /// <param name="model">
        /// The Post
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Save(ComposePostModel model)
        {
            this.UpdatePost(model);

            // if (ModelState.IsValid)
            // {
            // SavePost(model.Post);
            // }

            // ViewData.Model = model;
            // return View();
            this.SavePost(model.Post);
            DateTime now =
                LocalTime.GetCurrentTime(TimeZoneInfo.FindSystemTimeZoneById(SettingsComp.GetSettings().Timezone));
            return this.PartialView("AutoSaveControl", now);
        }

        /// <summary>
        /// Uploads a document
        /// </summary>
        /// <param name="model">
        /// The Post
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        [MultiButton(FormName = "Upload", FormValue = "上传")]
        public ActionResult Upload(ComposePostModel model)
        {
            this.UpdatePost(model);

            for (int i = 0; i < this.Request.Files.Count; i++)
            {
                HttpPostedFileBase file = this.Request.Files[i];
                if (file.ContentLength > 0)
                {
                    string docPath = this.Server.MapPath(this.Url.Content("~/Docs/"));
                    string fileName = DraftComp.StoreDocument(file, docPath);
                    Document doc = new Document();
                    doc.Path = this.Url.Content(string.Format("~/Docs/{0}", fileName));
                    model.Post.Documents.Add(doc);
                }
            }

            this.Session["Post"] = model.Post;
            return this.RedirectToAction("Compose", new { id = string.Empty });
        }

        #endregion

        #region Methods

        /// <summary>
        /// The save post.
        /// </summary>
        /// <param name="post">
        /// The post.
        /// </param>
        /// <returns>
        /// The LiteBlog.Common.DraftPost.
        /// </returns>
        private DraftPost SavePost(DraftPost post)
        {
            if (string.IsNullOrEmpty(post.DraftID))
            {
                if (post.Type == PostType.New)
                {
                    post.DraftID = DraftComp.Create(post.Title, post.Time, post.CatID);
                }
                else
                {
                    post.DraftID = DraftComp.Create(post.FileID);
                }
            }

            // post.Contents = post.Contents.Replace("&nbsp;", " ");
            DraftComp.Save(post);

            this.Session["Post"] = post;

            return post;
        }

        /// <summary>
        /// Strange method, got to look into it
        /// </summary>
        /// <param name="model">
        /// The model
        /// </param>
        private void UpdatePost(ComposePostModel model)
        {
            DraftPost post = new DraftPost();
            if (this.Session["Post"] != null)
            {
                post = this.Session["Post"] as DraftPost;
            }

            model.Title = model.Title ?? string.Empty;
            post.Title = model.Title;
            post.CatID = string.Empty;
            if (model.CatID != null)
            {
                foreach (string catID in model.CatID)
                {
                    post.CatID += catID + ",";
                }

                post.CatID = post.CatID.Substring(0, post.CatID.Length - 1);
            }

            if (post.Type == PostType.New)
            {
                try
                {
                    if (!string.IsNullOrEmpty(model.PublishDate))
                    {
                        post.Time = DateTime.ParseExact(model.PublishDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(SettingsComp.GetSettings().Timezone);
                        DateTime now = LocalTime.GetCurrentTime(tzi);
                        post.Time = post.Time.AddHours(now.Hour);
                        post.Time = post.Time.AddMinutes(now.Minute);
                    }
                }
                catch
                {
                }
            }

            // To avoid exception in syntax highlighter
            post.Contents = model.Content ?? string.Empty;
            post.Author = this.User.Identity.Name;
            model.Post = post;

            this.Session["Post"] = post;
        }

        #endregion
    }
}