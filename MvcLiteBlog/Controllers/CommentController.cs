// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentController.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   Used for creating comments from user pages
//   Used for editing, deleting, approving, rejecting comments from admin pages
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LiteBlog.Common;
    using MvcLiteBlog.BlogEngine;
    using MvcLiteBlog.Helpers;
    using MvcLiteBlog.Models;

    /// <summary>
    /// Used for creating comments from user pages
    /// Used for editing, deleting, approving, rejecting comments from admin pages
    /// </summary>
    public class CommentController : Controller
    {
        #region Public Methods and Operators

        /// <summary>
        /// Approves or rejects a list of comments
        /// Redirects to the moderation list page
        /// </summary>
        /// <param name="coll">
        /// Form Collection
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult Approve(FormCollection coll)
        {
            bool approve = true;
            ValueProviderResult result = coll.GetValue("Approve");
            if (result == null)
            {
                approve = false;
            }

            string[] ids = coll.GetValues("Select");
            if (ids.Length > 0)
            {
                foreach (string id in ids)
                {
                    if (approve)
                    {
                        CommentComp.Approve(id);
                    }
                    else
                    {
                        CommentComp.Delete(id);
                    }
                }

                if (approve)
                {
                    this.TempData["Message"] = "评论审核通过";
                }
                else
                {
                    this.TempData["Message"] = "评论审核未通过";
                }
            }

            return this.RedirectToAction("Moderate");
        }

        /// <summary>
        /// A partial view displaying the comment form
        /// Users can use this form to enter comments
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult CommentFormControl()
        {
            CommenterProfile comProfile = ProfileComp.GetCommenterProfile();
            InsertCommentModel model = new InsertCommentModel() { Name = comProfile.Name, Url = comProfile.Url };
            if (this.User.Identity.IsAuthenticated)
            {
                model.IsAuthor = true;
            }

            this.ViewData.Model = model;
            return this.PartialView();
        }

        /// <summary>
        /// Creates a comment
        /// Always called using Ajax, Do not want to refresh the entire screen
        /// Shown to all user
        /// </summary>
        /// <param name="model">
        /// The model
        /// </param>
        /// <returns>
        /// error message
        /// </returns>
        [ValidateInput(false)]
        public ActionResult Create(InsertCommentModel model)
        {
            string message = string.Empty;
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(model.Url) && this.ModelState.IsValidField("Url"))
                {
                    // Check if URL is valid
                    if (!MvcLiteBlog.Helpers.UrlHelper.IsValidUrl(model.Url))
                    {
                        this.ModelState.AddModelError("Url", "请输入正确的Url地址");
                    }
                }

                if (this.ModelState.IsValid)
                {
                    CommentComp.Insert(model.FileID, model.Name, model.Url ?? string.Empty, model.CommentText, model.IsAuthor);
                    message = "评论已提交";
                    if (SettingsComp.GetSettings().CommentModeration)
                    {
                        message = "评论已提交，并等待管理员审核";
                    }

                    ProfileComp.SetCommenterProfile(model.Name, model.Url ?? string.Empty);
                    result = true;
                }
                else
                {
                    message = string.Empty;
                    foreach (string key in this.ModelState.Keys)
                    {
                        foreach (ModelError err in this.ModelState[key].Errors)
                        {
                            if (err.ErrorMessage != string.Empty)
                            {
                                message += err.ErrorMessage + "<br />";
                            }
                        }
                    }

                    result = false;
                }
            }
            catch
            {
                message = "评论ID不能修改";
            }

            return this.Json(new { Message = message, Result = result });
        }

        /// <summary>
        /// Delete the selected comments
        /// Redirects to comments list page
        /// </summary>
        /// <param name="page">
        /// Page Index starting with zero
        /// </param>
        /// <param name="coll">
        /// Form collection
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult Delete(int page, FormCollection coll)
        {
            string[] ids = coll.GetValues("Select");
            if (ids != null)
            {
                if (ids.Length > 0)
                {
                    foreach (string id in ids)
                    {
                        CommentComp.Delete(id);
                    }

                    this.TempData["Message"] = "评论已删除";
                }
            }

            if (page == 1)
            {
                return this.RedirectToAction("Manage");
            }
            else
            {
                return this.RedirectToAction("Manage", new { page = page });
            }
        }

        /// <summary>
        /// Gets a list of comments as JSON
        /// It is called from PostControl from the user pages
        /// </summary>
        /// <param name="id">
        /// Post ID
        /// </param>
        /// <returns>
        /// List of comments (as JSON)
        /// </returns>
        [HttpGet]
        public ActionResult Details(string id)
        {
            Post post = PostComp.Load(id);
            return this.Json(post.Comments);
        }

        /// <summary>
        /// Gets comment details for editing
        /// </summary>
        /// <param name="id">
        /// Comment ID
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            Comment comment = CommentComp.GetComment(id);
            this.ViewData.Model = comment;

            // Find out the page index
            int pageSize = 10;
            int rowIndex = CommentComp.GetComments().FindIndex(c => c.ID == comment.ID);
            int pageIndex = rowIndex / pageSize;
            this.ViewBag.PageIndex = pageIndex + 1;
            return this.View();
        }

        /// <summary>
        /// Updates comment based on ID
        /// Finds the current page index
        /// Redirects to comments list page at appropriate pageindex
        /// </summary>
        /// <param name="comment">
        /// The comment
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Comment comment)
        {
            if (string.IsNullOrEmpty(comment.Name))
            {
                this.ModelState.AddModelError("Name", "评论人不能为空");
            }

            if (string.IsNullOrEmpty(comment.Text))
            {
                this.ModelState.AddModelError("Text", "评论内容不能为空");
            }

            comment.Url = comment.Url ?? string.Empty;

            try
            {
                comment.Text = comment.Text.Replace("&nbsp;", " ");
                CommentComp.Update(comment);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("Update", "更新错误");
                Logger.Log("Update failed", ex);
            }

            if (this.ModelState.IsValid)
            {
                this.TempData["Message"] = "评论已更新";

                // Find out the page index
                int pageSize = 10;
                int rowIndex = CommentComp.GetComments().FindIndex(c => c.ID == comment.ID);
                int pageIndex = (rowIndex / pageSize) + 1;
                if (pageIndex == 1)
                {
                    return this.RedirectToAction("Manage");
                }
                else
                {
                    return this.RedirectToAction("Manage", new { page = pageIndex });
                }
            }

            this.ViewData.Model = comment;
            return this.View();
        }

        /// <summary>
        /// Gets list of comments
        /// Manages comments in a simple way
        /// Paging and Tabs are handled in QueryString
        /// ViewBag is used to store page-state data
        /// </summary>
        /// <param name="page">
        /// Page Index starting with 0
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Manage(int? page)
        {
            int pageSize = 10;
            int pageIndex = page ?? 1;
            int totalRecords = 0;

            List<Comment> comments = CommentComp.GetComments();
            totalRecords = comments.Count<Comment>();

            // There is a possibility that page index > total pages
            // when for eg the last of 61 records are deleted.
            int totalPages = totalRecords / pageSize;
            if (totalRecords % pageSize != 0)
            {
                totalPages++;
            }

            if (pageIndex > totalPages)
            {
                pageIndex = totalPages;
                if (pageIndex == 1)
                {
                    return this.RedirectToAction("Manage");
                }
                else
                {
                    return this.RedirectToAction("Manage", new { page = pageIndex });
                }
            }

            this.ViewBag.PageSize = pageSize;
            this.ViewBag.PageIndex = pageIndex;
            this.ViewBag.TotalRecords = totalRecords;

            // comments = comments.Skip<Comment>((pageIndex-1) * pageSize)
            // .Take<Comment>(pageSize)
            // .ToList<Comment>();
            return View(comments);
        }

        /// <summary>
        /// Comments moderation page
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult Moderate()
        {
            List<Comment> comments = CommentComp.GetUnapprovedComments();
            return View(comments);
        }

        /// <summary>
        /// A list of comments posted
        /// Shown to all users
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Recent()
        {
            this.ViewData.Model = CommentComp.GetComments();
            return this.View();
        }

        /// <summary>
        /// Top 3 comments posted
        /// Shown on comments tile on the home page
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult RecentCommentsTile()
        {
            this.ViewData.Model = CommentComp.GetComments().Take(10);
            return this.PartialView("RecentCommentsTile");
        }

        #endregion
    }
}