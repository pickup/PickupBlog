// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDraftData.cs" company="LiteBlog">
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

    /// <summary>
    /// The DraftData interface.
    /// </summary>
    public interface IDraftData
    {
        #region Public Methods and Operators

        /// <summary>
        /// The add schedule.
        /// </summary>
        /// <param name="draftID">
        /// The draft id.
        /// </param>
        /// <param name="publishDate">
        /// The publish date.
        /// </param>
        void AddSchedule(string draftID, DateTime publishDate);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="draftID">
        /// The draft id.
        /// </param>
        void Delete(string draftID);

        /// <summary>
        /// The get draft items.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.PostInfo].
        /// </returns>
        List<PostInfo> GetDraftItems();

        /// <summary>
        /// The get scheduled draft items.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.PostInfo].
        /// </returns>
        List<PostInfo> GetScheduledDraftItems();

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="draftID">
        /// The draft id.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        void Insert(string draftID, string title);

        #endregion
    }
}