// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorController.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The author controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using LiteBlog.Common;
    using MvcLiteBlog.BlogEngine;
    using MvcLiteBlog.Helpers;
    using MvcLiteBlog.Models;

    /// <summary>
    /// The author controller.
    /// </summary>
    public class AuthorController : Controller
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return this.View();
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Create(AuthorModel model)
        {
            if (this.ModelState.IsValid)
            {
                // Edit Author
                Author author = new Author();
                author.ID = model.Name;
                author.Name = model.DisplayName;
                author.Email = model.Email;

                EngineException ex = AuthorComp.Create(author);
                if (ex != null)
                {
                    this.ModelState.AddModelError("Create", ex);
                }
            }

            if (this.ModelState.IsValid)
            {
                this.TempData["Message"] = "用户添加成功";
                return this.RedirectToAction("Manage");
            }

            this.ViewData.Model = model;
            return this.View();
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult Delete(string id)
        {
            EngineException ex = AuthorComp.Delete(id);
            if (ex != null)
            {
                this.TempData["Message"] = ex.Message;
            }
            else
            {
                this.TempData["Message"] = "用户已被删除";
            }

            return this.RedirectToAction("Manage");
        }

        /// <summary>
        /// The edit.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Edit(string id)
        {
            AuthorModel model = new AuthorModel();

            if (!string.IsNullOrEmpty(id))
            {
                Author author = AuthorComp.GetAuthor(id);
                if (author != null)
                {
                    this.TempData["AuthorName"] = author.ID;
                    model.Name = author.ID;
                    model.DisplayName = author.Name;
                    model.Email = author.Email;
                }
            }

            this.ViewData.Model = model;
            return this.View();
        }

        /// <summary>
        /// The edit.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Edit(AuthorModel model)
        {
            if (this.ModelState.IsValid)
            {
                // Edit Author
                string oldName = this.TempData["AuthorName"].ToString();
                Author author = new Author();
                author.ID = model.Name;
                author.Name = model.DisplayName;
                author.Email = model.Email;

                EngineException ex = AuthorComp.Update(oldName, author);
                if (ex != null)
                {
                    this.ModelState.AddModelError("Edit", ex);
                }
            }

            if (this.ModelState.IsValid)
            {
                this.TempData["Message"] = "用户信息已更新";
                return this.RedirectToAction("Manage");
            }

            this.ViewData.Model = model;
            return this.View();
        }

        /// <summary>
        /// The manage.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        [HttpGet]
        public ActionResult Manage()
        {
            List<Author> model;
            model = AuthorComp.GetAuthors();
            this.ViewData.Model = model;
            return this.View();
        }

        /// <summary>
        /// The unlock.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult Unlock(string id)
        {
            AuthorComp.Unlock(id);
            this.TempData["Message"] = "用户已解除锁定";
            return this.RedirectToAction("Manage");
        }

        #endregion
    }
}