// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrlHelper.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The url helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Helpers
{
    using System;
    using System.Net;

    /// <summary>
    /// The url helper.
    /// </summary>
    public class UrlHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// The is valid url.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public static bool IsValidUrl(string url)
        {
            HttpWebResponse res;
            try
            {
                Uri uri = new Uri(url, UriKind.Absolute);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                res = (HttpWebResponse)req.GetResponse();
            }
            catch
            {
                return false;
            }

            switch (res.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Redirect:
                case HttpStatusCode.RedirectKeepVerb:
                case HttpStatusCode.RedirectMethod:
                    return true;
            }

            return false;
        }

        #endregion
    }
}