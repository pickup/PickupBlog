// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategoryData.cs" company="LiteBlog">
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
    /// The CategoryData interface.
    /// </summary>
    public interface ICategoryData
    {
        #region Public Methods and Operators

        /// <summary>
        /// The change count.
        /// </summary>
        /// <param name="catID">
        /// The cat id.
        /// </param>
        /// <param name="number">
        /// The number.
        /// </param>
        void ChangeCount(string catID, int number);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="catID">
        /// The cat id.
        /// </param>
        void Delete(string catID);

        /// <summary>
        /// The get categories.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.Category].
        /// </returns>
        List<Category> GetCategories();

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        void Insert(Category category);

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="oldID">
        /// The old id.
        /// </param>
        /// <param name="category">
        /// The category.
        /// </param>
        void Update(string oldID, Category category);

        #endregion
    }
}