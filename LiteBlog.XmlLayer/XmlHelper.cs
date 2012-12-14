// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlHelper.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The xml helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.XmlLayer
{
    using System;
    using System.Xml;
    using System.Xml.Linq;

    using LiteBlog.Common;

    /// <summary>
    /// The xml helper.
    /// </summary>
    public class XmlHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="root">
        /// The root.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <exception cref="ApplicationException">
        /// Application exception
        /// </exception>
        public static void Save(XElement root, string path)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                XmlWriter writer = XmlWriter.Create(path, settings);
                root.Save(writer);
                writer.Close();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                string msg = string.Format("Error in saving file: {0}", path);
                throw new ApplicationException(msg, ex);
            }
        }

        #endregion
    }
}