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
        [Required(ErrorMessage = "Please enter a valid new password")]
        [RegularExpression("^[a-zA-z0-9_.]{3,15}$", ErrorMessage = "Please enter a valid password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the old password.
        /// </summary>
        [Required(ErrorMessage = "Please enter a valid password")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the repeat password.
        /// </summary>
        [Required(ErrorMessage = "Please type the new password again in repeat password")]
        public string RepeatPassword { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Required(ErrorMessage = "Please enter a valid user name")]
        public string UserName { get; set; }

        #endregion
    }
}