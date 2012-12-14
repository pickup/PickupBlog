// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComposePostModel.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The compose post model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MvcLiteBlog.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using LiteBlog.Common;

    using MvcLiteBlog.BlogEngine;

    /// <summary>
    /// The compose post model.
    /// </summary>
    public class ComposePostModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the cat id.
        /// </summary>
        public List<string> CatID { get; set; }

        /// <summary>
        /// Gets the category list.
        /// </summary>
        public List<SelectListItem> CategoryList
        {
            get
            {
                List<SelectListItem> list = new List<SelectListItem>();

                foreach (Category cat in CategoryComp.GetCategories())
                {
                    SelectListItem item = new SelectListItem();
                    item.Selected = false;
                    item.Text = cat.Name;
                    item.Value = cat.CatID;
                    if (this.CatID != null)
                    {
                        foreach (string categoryID in this.CatID)
                        {
                            if (categoryID == item.Value)
                            {
                                item.Selected = true;
                            }
                        }
                    }

                    list.Add(item);
                }

                return list;
            }
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the draft id.
        /// </summary>
        public string DraftID { get; set; }

        /// <summary>
        /// Gets or sets the post.
        /// </summary>
        public DraftPost Post { get; set; }

        /// <summary>
        /// Gets or sets the publish date.
        /// </summary>
        public string PublishDate { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [Required(ErrorMessage = "Please enter a title for the post")]
        [RegularExpression(@"[\w-\?!:,\. ]+", ErrorMessage = "No special characters allowed in the title")]
        public string Title { get; set; }

        #endregion
    }
}