// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorModel.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The author model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The author model.
    /// </summary>
    public class AuthorModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        [Required(ErrorMessage = "请输入正确的昵称")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Required(ErrorMessage = "请输人正确的电子油箱地址")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", 
            ErrorMessage = "Please enter the email in the right format")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required(ErrorMessage = "请输人正确的用户名")]
        public string Name { get; set; }

        #endregion
    }
}