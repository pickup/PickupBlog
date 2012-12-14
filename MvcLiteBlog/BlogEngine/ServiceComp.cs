// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceComp.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The service comp.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.BlogEngine
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web;

    using LiteBlog.Common;

    using MvcLiteBlog.Helpers;

    /// <summary>
    /// The service comp.
    /// </summary>
    public class ServiceComp
    {
        #region Public Methods and Operators

        /// <summary>
        /// Initializes service run times from Service.xml
        /// and store in in the Application state object
        /// Need to run from asp.net worker thread???
        /// </summary>
        public static void Initialize()
        {
            List<ServiceItem> items = ConfigHelper.DataContext.ServiceData.Load();
            HttpContext.Current.Application["Service"] = items;
        }

        /// <summary>
        /// The run.
        /// </summary>
        public static void Run()
        {
            List<ServiceItem> items = (List<ServiceItem>)HttpContext.Current.Application["Service"];
            if (items != null)
            {
                foreach (ServiceItem item in items)
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(SettingsComp.GetSettings().Timezone);
                    if (item.NextUpdate < LocalTime.GetCurrentTime(tzi))
                    {
                        Type type = Type.GetType(item.Type);
                        MethodInfo mi = type.GetMethod(item.Method, BindingFlags.Public | BindingFlags.Static);
                        mi.Invoke(null, null);
                        item.LastUpdated = LocalTime.GetCurrentTime(tzi);
                    }
                }

                HttpContext.Current.Application["Service"] = items;
            }
        }

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="svcList">
        /// The svc list.
        /// </param>
        public static void Save(List<ServiceItem> svcList)
        {
            ConfigHelper.DataContext.ServiceData.Save(svcList);
        }

        #endregion
    }
}