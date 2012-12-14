// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBlogData.cs" company="LiteBlog">
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
    /// The BlogData interface.
    /// </summary>
    public interface IBlogData
    {
        #region Public Methods and Operators

        /// <summary>
        /// The change author.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <param name="author">
        /// The author.
        /// </param>
        void ChangeAuthor(string fileID, string author);

        /// <summary>
        /// The change category.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <param name="oldCatID">
        /// The old cat id.
        /// </param>
        /// <param name="newCatID">
        /// The new cat id.
        /// </param>
        void ChangeCategory(string fileID, string oldCatID, string newCatID);

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="postInfo">
        /// The post info.
        /// </param>
        void Create(PostInfo postInfo);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        void Delete(string fileID);

        /// <summary>
        /// The get blog items.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.PostInfo].
        /// </returns>
        List<PostInfo> GetBlogItems();

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="catID">
        /// The cat id.
        /// </param>
        void Update(string fileID, string title, string catID);

        #endregion
    }
}