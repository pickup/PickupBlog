// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryComp.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The category comp.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.BlogEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    using MvcLiteBlog.Helpers;

    /// <summary>
    /// The category comp.
    /// </summary>
    public class CategoryComp
    {
        #region Public Methods and Operators

        /// <summary>
        /// The decrement.
        /// </summary>
        /// <param name="catID">
        /// The cat id.
        /// </param>
        public static void Decrement(string catID)
        {
            if (!string.IsNullOrEmpty(catID) && catID != "0")
            {
                ICategoryData data = ConfigHelper.DataContext.CategoryData;
                data.ChangeCount(catID, -1);
            }
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="catID">
        /// The cat id.
        /// </param>
        public static void Delete(string catID)
        {
            foreach (PostInfo post in BlogComp.GetPosts())
            {
                BlogComp.ChangeCategory(post.FileID, catID, string.Empty);
                PostComp.ChangeCategory(post.FileID, catID, string.Empty);
            }

            ConfigHelper.DataContext.CategoryData.Delete(catID);
        }

        /// <summary>
        /// The get categories.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.Category].
        /// </returns>
        public static List<Category> GetCategories()
        {
            List<Category> categories = CacheHelper.Get<List<Category>>(CacheType.Categories);
            if (categories == null)
            {
                ICategoryData data = ConfigHelper.DataContext.CategoryData;
                categories = data.GetCategories();
                var qry = from cat in categories orderby cat.Order ascending select cat;
                categories = qry.ToList<Category>();
                CacheHelper.Put(CacheType.Categories, categories);
            }

            return categories;
        }

        /// <summary>
        /// The get category.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The LiteBlog.Common.Category.
        /// </returns>
        public static Category GetCategory(string id)
        {
            var qry = from elem in CategoryComp.GetCategories() where elem.CatID == id select elem;

            Category cat = qry.FirstOrDefault<Category>();
            return cat;
        }

        /// <summary>
        /// The get category id.
        /// </summary>
        /// <param name="categoryName">
        /// The category name.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetCategoryID(string categoryName)
        {
            List<Category> catList = GetCategories();
            foreach (Category cat in catList)
            {
                if (cat.Name.ToLower() == categoryName.ToLower())
                {
                    return cat.CatID;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// The get category name.
        /// </summary>
        /// <param name="categoryID">
        /// The category id.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetCategoryName(string categoryID)
        {
            List<Category> catList = GetCategories();
            var categories = from cat in GetCategories() where cat.CatID.ToLower() == categoryID.ToLower() select cat;
            if (categories.Count<Category>() == 1)
            {
                return categories.First<Category>().Name;
            }

            return string.Empty;
        }

        /// <summary>
        /// The get default category.
        /// </summary>
        /// <returns>
        /// The LiteBlog.Common.Category.
        /// </returns>
        public static Category GetDefaultCategory()
        {
            return GetCategories().FirstOrDefault<Category>();
        }

        /// <summary>
        /// The increment.
        /// </summary>
        /// <param name="catID">
        /// The cat id.
        /// </param>
        public static void Increment(string catID)
        {
            if (!string.IsNullOrEmpty(catID) && catID != "0")
            {
                ICategoryData data = ConfigHelper.DataContext.CategoryData;
                data.ChangeCount(catID, 1);
            }
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="catID">
        /// The cat id.
        /// </param>
        /// <param name="catName">
        /// The cat name.
        /// </param>
        public static void Insert(string catID, string catName)
        {
            Category category = new Category();
            category.CatID = catID;
            category.Name = catName;
            category.Count = 0;
            category.Order = 0;

            ConfigHelper.DataContext.CategoryData.Insert(category);
        }

        /// <summary>
        /// The is unique id.
        /// </summary>
        /// <param name="newId">
        /// The new id.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public static bool IsUniqueId(string newId)
        {
            var qry = from category in CategoryComp.GetCategories() where category.CatID == newId select category;

            if (qry.Count<Category>() > 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The is unique name.
        /// </summary>
        /// <param name="newName">
        /// The new name.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public static bool IsUniqueName(string newName)
        {
            var qry = from category in CategoryComp.GetCategories() where category.Name == newName select category;

            if (qry.Count<Category>() > 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="oldCatID">
        /// The old cat id.
        /// </param>
        /// <param name="category">
        /// The category.
        /// </param>
        public static void Update(string oldCatID, Category category)
        {
            if (oldCatID != category.CatID)
            {
                foreach (PostInfo post in BlogComp.GetPosts())
                {
                    BlogComp.ChangeCategory(post.FileID, oldCatID, category.CatID);
                    PostComp.ChangeCategory(post.FileID, oldCatID, category.CatID);
                }
            }

            ConfigHelper.DataContext.CategoryData.Update(oldCatID, category);
        }

        #endregion
    }
}