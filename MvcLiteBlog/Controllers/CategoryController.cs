// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryController.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The category controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using LiteBlog.Common;

    using MvcLiteBlog.BlogEngine;
    using MvcLiteBlog.Models;

    /// <summary>
    /// The category controller.
    /// </summary>
    public class CategoryController : Controller
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        [HttpGet]
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
        [Authorize]
        [HttpPost]
        public ActionResult Create(CatModel model)
        {
            if (this.ModelState.IsValid)
            {
                string uniqueCheck = CategoryComp.GetCategoryID(model.Name);
                if (uniqueCheck != string.Empty)
                {
                    this.ModelState.AddModelError("Name", "Category name should be unique");
                }

                uniqueCheck = CategoryComp.GetCategoryName(model.CatID);
                if (uniqueCheck != string.Empty)
                {
                    this.ModelState.AddModelError("CatID", "Category ID should be unique");
                }

                if (this.ModelState.IsValid)
                {
                    CategoryComp.Insert(model.CatID, model.Name);
                    this.TempData["Message"] = "Category is successfully added";
                    return this.RedirectToAction("Manage");
                }
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
            CategoryComp.Delete(id);
            this.TempData["Message"] = "Category is successfully deleted";
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
        [Authorize]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            CatModel model = new CatModel();
            model.CatID = id;

            Category cat = CategoryComp.GetCategory(id);

            // TempData["Category"] = cat;
            model.Name = cat.Name;

            this.ViewData.Model = model;
            return this.View();
        }

        /// <summary>
        /// The edit.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        [HttpPost]
        public ActionResult Edit(string id, CatModel model)
        {
            if (this.ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    Category oldCategory = CategoryComp.GetCategory(id);

                    // get old category
                    if (oldCategory.Name != model.Name)
                    {
                        if (!CategoryComp.IsUniqueName(model.Name))
                        {
                            this.ModelState.AddModelError("Name", "Category name should be unique");
                        }
                    }

                    if (id != model.CatID)
                    {
                        if (!CategoryComp.IsUniqueId(model.CatID))
                        {
                            this.ModelState.AddModelError("CatID", "Category ID should be unique");
                        }
                    }

                    if (this.ModelState.IsValid)
                    {
                        Category cat2 = new Category();
                        cat2.CatID = model.CatID;
                        cat2.Name = model.Name;
                        cat2.Count = oldCategory.Count;
                        cat2.Order = oldCategory.Order;
                        CategoryComp.Update(oldCategory.CatID, cat2);
                    }
                }
            }

            if (this.ModelState.IsValid)
            {
                this.TempData["Message"] = "Category is successfully updated";
                return this.RedirectToAction("Manage");
            }
            else
            {
                this.ViewData.Model = model;
                return this.View();
            }
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Index(string id)
        {
            string name = CategoryComp.GetCategoryName(id);
            if (name == string.Empty)
            {
                id = CategoryComp.GetDefaultCategory().CatID;
                name = CategoryComp.GetDefaultCategory().Name;
            }

            this.ViewData["CategoryName"] = name;
            this.ViewData.Model = BlogComp.GetPostsByCategory(id);
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
            List<Category> model = CategoryComp.GetCategories();
            this.ViewData.Model = model;
            return this.View();
        }

        /// <summary>
        /// The reorder.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpGet]
        [Authorize]
        public ActionResult Reorder()
        {
            List<Category> categories = CategoryComp.GetCategories();
            this.ViewData.Model = categories;
            return this.View();
        }

        /// <summary>
        /// The reorder.
        /// </summary>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Reorder(string order)
        {
            string[] catIDs = order.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<Category> categories = CategoryComp.GetCategories();

            int index = 1;
            foreach (string catID in catIDs)
            {
                foreach (Category cat in categories)
                {
                    if (cat.CatID == catID)
                    {
                        if (cat.Order != index)
                        {
                            cat.Order = index;
                            CategoryComp.Update(cat.CatID, cat);
                        }

                        break;
                    }
                }

                index++;
            }

            return this.RedirectToAction("Manage");
        }

        /// <summary>
        /// The widget.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Widget()
        {
            List<Category> categories = CategoryComp.GetCategories();
            this.ViewData.Model = categories;
            return this.PartialView("CategoryControl");
        }

        #endregion
    }
}