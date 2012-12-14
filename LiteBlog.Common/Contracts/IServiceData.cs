// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IServiceData.cs" company="LiteBlog">
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
    /// The ServiceData interface.
    /// </summary>
    public interface IServiceData
    {
        #region Public Methods and Operators

        /// <summary>
        /// The load.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.ServiceItem].
        /// </returns>
        List<ServiceItem> Load();

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="svcItems">
        /// The svc items.
        /// </param>
        void Save(List<ServiceItem> svcItems);

        #endregion
    }
}