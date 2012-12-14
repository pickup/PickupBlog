// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISettingsData.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The cache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common.Contracts
{
    /// <summary>
    /// The SettingsData interface.
    /// </summary>
    public interface ISettingsData
    {
        #region Public Methods and Operators

        /// <summary>
        /// The load.
        /// </summary>
        /// <returns>
        /// The LiteBlog.Common.Settings.
        /// </returns>
        Settings Load();

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        void Save(Settings app);

        #endregion
    }
}