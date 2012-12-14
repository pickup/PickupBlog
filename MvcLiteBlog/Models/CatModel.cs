// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CatModel.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The cat model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The cat model.
    /// </summary>
    public class CatModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the cat id.
        /// </summary>
        [Required(ErrorMessage = "Please enter a valid category id")]
        [RegularExpression(@"\w+", ErrorMessage = "Category id should be a simple word")]
        public string CatID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required(ErrorMessage = "Please enter a valid category name")]
        public string Name { get; set; }

        #endregion
    }
}