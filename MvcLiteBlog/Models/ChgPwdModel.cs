// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChgPwdModel.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The login model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The chg pwd model.
    /// </summary>
    public class ChgPwdModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        [Required(ErrorMessage = "请输入正确的新密码")]
        [RegularExpression("^[a-zA-z0-9_.]{3,15}$", ErrorMessage = "请输入正确的新密码（3至15位字母或数字）")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the old password.
        /// </summary>
        [Required(ErrorMessage = "请输入正确的原密码")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the repeat password.
        /// </summary>
        [Required(ErrorMessage = "请重复输入新密码")]
        public string RepeatPassword { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Required(ErrorMessage = "请输入正确的用户名")]
        public string UserName { get; set; }

        #endregion
    }
}