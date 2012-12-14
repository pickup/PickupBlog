// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Category.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The category.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common
{
    /// <summary>
    /// The category.
    /// </summary>
    public class Category
    {
        #region Fields

        /// <summary>
        /// The _cat id.
        /// </summary>
        private string _catID = string.Empty;

        /// <summary>
        /// The _count.
        /// </summary>
        private int _count = 0;

        /// <summary>
        /// The _name.
        /// </summary>
        private string _name = string.Empty;

        /// <summary>
        /// The _order.
        /// </summary>
        private int _order = 0;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the cat id.
        /// </summary>
        public string CatID
        {
            get
            {
                return this._catID;
            }

            set
            {
                this._catID = value;
            }
        }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        public int Count
        {
            get
            {
                return this._count;
            }

            set
            {
                this._count = value;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }

            set
            {
                this._name = value;
            }
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        public int Order
        {
            get
            {
                return this._order;
            }

            set
            {
                this._order = value;
            }
        }

        #endregion
    }
}