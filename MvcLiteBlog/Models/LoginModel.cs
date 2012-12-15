// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginModel.cs" company="LiteBlog">
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
    /// The login model.
    /// </summary>
    public class LoginModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginModel"/> class.
        /// </summary>
        public LoginModel()
        {
            this.UserName = string.Empty;
            this.Password = string.Empty;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required(ErrorMessage = "其输入正确的密码")]
        //[Display(Name="密码")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Required(ErrorMessage = "请输入正确的用户名")]
        //[Display(Name = "用户名")]
        public string UserName { get; set; }

        #endregion
    }
}