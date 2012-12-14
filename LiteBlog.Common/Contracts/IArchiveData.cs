// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IArchiveData.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The cache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Web.Caching;

    /// <summary>
    /// The ArchiveData interface.
    /// </summary>
    public interface IArchiveData
    {
        #region Public Methods and Operators

        /// <summary>
        /// The change count.
        /// </summary>
        /// <param name="archiveID">
        /// The archive id.
        /// </param>
        /// <param name="number">
        /// The number.
        /// </param>
        void ChangeCount(string archiveID, int number);

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="month">
        /// The month.
        /// </param>
        void Create(ArchiveMonth month);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="archiveID">
        /// The archive id.
        /// </param>
        void Delete(string archiveID);

        /// <summary>
        /// The get archive months.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.ArchiveMonth].
        /// </returns>
        List<ArchiveMonth> GetArchiveMonths();

        #endregion
    }
}