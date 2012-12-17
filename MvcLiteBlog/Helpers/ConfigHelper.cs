// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigHelper.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The config helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Helpers
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Configuration;
    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    /// <summary>
    /// The config helper.
    /// </summary>
    public class ConfigHelper
    {
        #region Public Properties

        /// <summary>
        /// Gets the cache context.
        /// </summary>
        public static ICacheContext CacheContext
        {
            get
            {
                ICacheContext context = CacheHelper.Get<ICacheContext>(CacheType.CacheContext);
                if (context == null)
                {
                    // string className = ConfigurationManager.AppSettings["CacheContext"];
                    // Type type = Type.GetType(className);
                    // context = (ICacheContext)Activator.CreateInstance(type);
                    using (UnityContainer container = new UnityContainer())
                    {
                        UnityConfigurationSection section =
                            (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                        if (section != null)
                        {
                            section.Configure(container, "main");
                            context = container.Resolve<ICacheContext>();
                        }
                    }

                    // The .ToString() is done to avoid looping
                    CacheHelper.Put(CacheType.CacheContext.ToString(), context);
                }

                return context;
            }
        }

        /// <summary>
        /// Gets the data context.
        /// </summary>
        public static IDataContext DataContext
        {
            get
            {
                IDataContext context = null;
                if (HttpContext.Current != null)
                {
                    context = CacheHelper.Get<IDataContext>(CacheType.DataContext);
                }

                if (context == null)
                {
                    // string className = ConfigurationManager.AppSettings["DataContext"];
                    // Type type = Type.GetType(className);
                    string dataPath = ConfigHelper.DataPath;
                    //if (string.IsNullOrEmpty(dataPath) || !System.IO.Directory.Exists(dataPath))
                    //{
                    //    if (HttpContext.Current != null)
                    //    {
                    //        dataPath = HttpContext.Current.Server.MapPath("~/App_Data/");
                    //        ConfigHelper.DataPath = dataPath;
                    //    }
                    //}

                    // context = (IDataContext)Activator.CreateInstance(type, dataPath);
                    using (UnityContainer container = new UnityContainer())
                    {
                        UnityConfigurationSection section =
                            (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                        if (section != null)
                        {
                            section.Configure(container, "main");
                            context = container.Resolve<IDataContext>(new ParameterOverride("path", dataPath));
                        }
                    }

                    if (HttpContext.Current != null)
                    {
                        CacheHelper.Put(CacheType.DataContext, context);
                    }
                }

                return context;
            }
        }

        /// <summary>
        /// Gets or sets the data path.
        /// </summary>
        public static string DataPath
        {
            get
            {
                return ConfigurationManager.AppSettings["DataPath"];
            }

            set
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                config.AppSettings.Settings["DataPath"].Value = value.ToString();
                config.Save();
            }
        }

        /// <summary>
        /// Gets the date format.
        /// </summary>
        public static string DateFormat
        {
            get
            {
                return ConfigurationManager.AppSettings["DateFormat"];
            }
        }

        /// <summary>
        /// Gets the default password.
        /// </summary>
        public static string DefaultPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["DefaultPassword"];
            }
        }

        #endregion
    }
}