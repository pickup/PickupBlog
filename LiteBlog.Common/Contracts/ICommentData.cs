// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommentData.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The cache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common.Contracts
{
    using System.Collections.Generic;

    /// <summary>
    /// The CommentData interface.
    /// </summary>
    public interface ICommentData
    {
        #region Public Methods and Operators

        /// <summary>
        /// The approve.
        /// </summary>
        /// <param name="commentID">
        /// The comment id.
        /// </param>
        void Approve(string commentID);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="commentID">
        /// The comment id.
        /// </param>
        void Delete(string commentID);

        /// <summary>
        /// The get comments.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.Comment].
        /// </returns>
        List<Comment> GetComments();

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        string Insert(Comment comment);

        /// <summary>
        /// The refresh.
        /// </summary>
        void Refresh();

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="comment">
        /// The comment.
        /// </param>
        void Update(Comment comment);

        #endregion
    }
}