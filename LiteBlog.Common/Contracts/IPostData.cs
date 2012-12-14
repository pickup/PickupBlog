// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPostData.cs" company="LiteBlog">
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
    /// The PostData interface.
    /// </summary>
    public interface IPostData
    {
        #region Public Methods and Operators

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        void Delete(string fileID);

        /// <summary>
        /// The delete comment.
        /// </summary>
        /// <param name="comment">
        /// The comment.
        /// </param>
        void DeleteComment(Comment comment);

        /// <summary>
        /// The insert comment.
        /// </summary>
        /// <param name="comment">
        /// The comment.
        /// </param>
        void InsertComment(Comment comment);

        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <returns>
        /// The LiteBlog.Common.Post.
        /// </returns>
        Post Load(string fileID);

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="post">
        /// The post.
        /// </param>
        void Save(Post post);

        /// <summary>
        /// The update comment.
        /// </summary>
        /// <param name="comment">
        /// The comment.
        /// </param>
        void UpdateComment(Comment comment);

        #endregion
    }
}