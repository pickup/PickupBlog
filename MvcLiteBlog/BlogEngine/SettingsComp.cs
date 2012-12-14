// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsComp.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The settings comp.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.BlogEngine
{
    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    using MvcLiteBlog.Helpers;

    /// <summary>
    /// The settings comp.
    /// </summary>
    public class SettingsComp
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get settings.
        /// </summary>
        /// <returns>
        /// The LiteBlog.Common.Settings.
        /// </returns>
        public static Settings GetSettings()
        {
            Settings app = CacheHelper.Get<Settings>(CacheType.Settings);
            if (app == null)
            {
                ISettingsData data = ConfigHelper.DataContext.SettingsData;
                app = data.Load();
                CacheHelper.Put(CacheType.Settings, app);
            }

            return app;
        }

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        public static void Save(Settings settings)
        {
            ConfigHelper.DataContext.SettingsData.Save(settings);
        }

        #endregion
    }
}