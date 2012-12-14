// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStatData.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The cache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common.Contracts
{
    /// <summary>
    /// The StatData interface.
    /// </summary>
    public interface IStatData
    {
        #region Public Methods and Operators

        /// <summary>
        /// The load.
        /// </summary>
        /// <returns>
        /// The LiteBlog.Common.Stat.
        /// </returns>
        Stat Load();

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="stat">
        /// The stat.
        /// </param>
        void Save(Stat stat);

        #endregion
    }
}