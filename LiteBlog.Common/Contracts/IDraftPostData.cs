// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDraftPostData.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The cache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common.Contracts
{
    using System;

    /// <summary>
    /// The DraftPostData interface.
    /// </summary>
    public interface IDraftPostData
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="author">
        /// The author.
        /// </param>
        /// <param name="time">
        /// The time.
        /// </param>
        /// <param name="catID">
        /// The cat id.
        /// </param>
        /// <returns>
        /// The LiteBlog.Common.DraftPost.
        /// </returns>
        DraftPost Create(string title, string author, DateTime time, string catID);

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <returns>
        /// The LiteBlog.Common.DraftPost.
        /// </returns>
        DraftPost Create(string fileID);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        void Delete(string fileID);

        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <returns>
        /// The LiteBlog.Common.DraftPost.
        /// </returns>
        DraftPost Load(string fileID);

        /// <summary>
        /// The publish.
        /// </summary>
        /// <param name="post">
        /// The post.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        string Publish(DraftPost post);

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="post">
        /// The post.
        /// </param>
        void Save(DraftPost post);

        #endregion
    }
}