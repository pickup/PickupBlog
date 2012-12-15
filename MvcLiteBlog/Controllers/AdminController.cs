// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdminController.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The admin controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using LiteBlog.Common;

    using MvcLiteBlog.BlogEngine;
    using MvcLiteBlog.Helpers;
    using MvcLiteBlog.Models;

    /// <summary>
    /// The admin controller.
    /// </summary>
    public class AdminController : Controller
    {
        // GET: /Admin/
        #region Public Methods and Operators

        /// <summary>
        /// The application.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        [HttpGet]
        public ActionResult Application()
        {
            ManageAppModel model = new ManageAppModel();
            Settings app = SettingsComp.GetSettings();
            model.BlogName = app.Name;
            model.PostCount = app.PostCount;
            model.CommentModeration = app.CommentModeration;
            model.Timezone = app.Timezone;
            this.ViewData.Model = model;
            return this.View();
        }

        /// <summary>
        /// The application.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Application(ManageAppModel model)
        {
            if (this.ModelState.IsValid)
            {
                Settings app = SettingsComp.GetSettings();
                app.Name = model.BlogName;
                app.PostCount = model.PostCount;
                app.CommentModeration = model.CommentModeration;
                app.Timezone = model.Timezone;
                SettingsComp.Save(app);

                this.ViewData["SuccessMessage"] = "配置信息已更新";
            }

            this.ViewData.Model = model;
            return this.View();
        }

        /// <summary>
        /// The chg pwd.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ChgPwd()
        {
            return this.View();
        }

        /// <summary>
        /// The chg pwd.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChgPwd(ChgPwdModel model)
        {
            this.ViewData.Model = model;

            if (this.ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.OldPassword))
                {
                    if (model.NewPassword == model.RepeatPassword)
                    {
                        MembershipUser user = Membership.GetUser(model.UserName);
                        if (user != null && user.ChangePassword(model.OldPassword, model.NewPassword))
                        {
                            FormsAuthentication.RedirectFromLoginPage(model.UserName, true);
                        }
                        else
                        {
                            this.ViewData.ModelState.AddModelError("ChangePassword", "密码不能修改");
                        }
                    }
                    else
                    {
                        this.ViewData.ModelState.AddModelError(
                            "RepeatPassword", "两次输入的新密码不一致");
                    }

                    // redirect only if validation is successful as well as change password is successful
                }
                else
                {
                    this.ViewData.ModelState.AddModelError("InvalidLogin", "用户名或密码不正确");
                }
            }

            return this.View();
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Index()
        {
            if (Membership.GetAllUsers().Count == 0)
            {
                Membership.CreateUser("Admin", ConfigHelper.DefaultPassword, "Admin@LiteBlog.com");
            }

            if (!string.IsNullOrEmpty(this.Request.QueryString["ReturnUrl"]))
            {
                this.ViewData["ReturnUrl"] = this.Request.QueryString["ReturnUrl"];
            }

            return this.View();
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(LoginModel model)
        {
            this.ViewData.Model = model;
            if (this.ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    bool enabled = AnonymousIdentificationModule.Enabled;
                    FormsAuthentication.RedirectFromLoginPage(model.UserName, true);
                    return new EmptyResult();
                }
                else
                {
                    this.ViewData.ModelState.AddModelError("Login", "用户名或密码不正确");
                }
            }

            if (!string.IsNullOrEmpty(this.Request.Form["ReturnUrl"]))
            {
                this.ViewData["ReturnUrl"] = this.Request.Form["ReturnUrl"];
            }

            return this.View("Index");
        }

        /// <summary>
        /// The logout.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return this.RedirectToAction("Index");
        }

        #endregion
    }
}