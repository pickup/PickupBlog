// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageAppModel.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The manage app model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// The manage app model.
    /// </summary>
    public class ManageAppModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the blog name.
        /// </summary>
        [Required(ErrorMessage = "请输入正确的博客名称")]
        public string BlogName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether comment moderation.
        /// </summary>
        public bool CommentModeration { get; set; }

        /// <summary>
        /// Gets the moderation list.
        /// </summary>
        public List<SelectListItem> ModerationList
        {
            get
            {
                List<SelectListItem> list = new List<SelectListItem>();

                SelectListItem yes = new SelectListItem();
                SelectListItem no = new SelectListItem();

                yes.Value = "True";
                yes.Text = "是";

                no.Value = "False";
                no.Text = "否";

                if (this.CommentModeration)
                {
                    yes.Selected = true;
                }
                else
                {
                    no.Selected = true;
                }

                list.Add(yes);
                list.Add(no);

                return list;
            }
        }

        /// <summary>
        /// Gets or sets the post count.
        /// </summary>
        public int PostCount { get; set; }

        /// <summary>
        /// Gets the post count list.
        /// </summary>
        public List<SelectListItem> PostCountList
        {
            get
            {
                List<SelectListItem> list = new List<SelectListItem>();
                for (int i = 1; i <= 20; i++)
                {
                    SelectListItem item = new SelectListItem();
                    item.Value = i.ToString();
                    item.Text = i.ToString();
                    if (i == this.PostCount)
                    {
                        item.Selected = true;
                    }

                    list.Add(item);
                }

                return list;
            }
        }

        /// <summary>
        /// Gets the time zone list.
        /// </summary>
        public List<SelectListItem> TimeZoneList
        {
            get
            {
                List<SelectListItem> list = new List<SelectListItem>();
                foreach (TimeZoneInfo tzInfo in TimeZoneInfo.GetSystemTimeZones())
                {
                    SelectListItem item = new SelectListItem();
                    item.Selected = false;
                    item.Text = tzInfo.DisplayName;
                    item.Value = tzInfo.Id;
                    if (item.Value.ToLower() == this.Timezone.ToLower())
                    {
                        item.Selected = true;
                    }

                    list.Add(item);
                }

                return list;
            }
        }

        /// <summary>
        /// Gets or sets the timezone.
        /// </summary>
        public string Timezone { get; set; }

        #endregion
    }
}