// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InsertCommentModel.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The insert comment model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using LiteBlog.Common;

    /// <summary>
    /// The insert comment model.
    /// </summary>
    public class InsertCommentModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        [Required(ErrorMessage = "Please enter your comment")]
        public string CommentText { get; set; }

        /// <summary>
        /// Gets or sets the file id.
        /// </summary>
        [Required(ErrorMessage = "There was some error. Please refresh the page!")]
        public string FileID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is author.
        /// </summary>
        [DefaultValue(false)]
        public bool IsAuthor { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        [RegularExpression(
            @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?", 
            ErrorMessage = "Please enter a valid web URL")]
        public string Url { get; set; }

        #endregion
    }
}