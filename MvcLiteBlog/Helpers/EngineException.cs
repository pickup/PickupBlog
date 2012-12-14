// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EngineException.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The engine exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Security;

    using LiteBlog.Common;

    /// <summary>
    /// The engine exception.
    /// </summary>
    [Serializable]
    public class EngineException : ApplicationException
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineException"/> class.
        /// </summary>
        public EngineException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineException"/> class.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        public EngineException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineException"/> class.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        /// <param name="ex">
        /// The ex.
        /// </param>
        public EngineException(string msg, Exception ex)
            : base(msg, ex)
        {
        }

        #endregion
    }
}