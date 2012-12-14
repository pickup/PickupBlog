// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The logger.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;

    /// <summary>
    /// The logger.
    /// </summary>
    public class Logger
    {
        #region Static Fields

        /// <summary>
        /// The lock obj.
        /// </summary>
        private static object lockObj = new object();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Log(string message)
        {
            Log(message, null);
        }

        /// <summary>
        /// The log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="ex">
        /// The ex.
        /// </param>
        public static void Log(string message, Exception ex)
        {
            lock (lockObj)
            {
                string path = HttpContext.Current.Server.MapPath("~/App_Data/Log.txt");

                // FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                // Appends to the end of the file
                StreamWriter sw = new StreamWriter(path, true);

                sw.Write(DateTime.Now.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) + " ");
                sw.WriteLine(message);
                if (ex != null)
                {
                    sw.WriteLine(ex.Message);
                    sw.WriteLine(ex.Source);
                    sw.WriteLine(ex.StackTrace);
                }

                sw.Flush();
                sw.Close();
            }
        }

        #endregion
    }
}