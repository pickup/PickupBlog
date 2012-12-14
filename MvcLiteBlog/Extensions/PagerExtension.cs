// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagerExtension.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The pager extension.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Extensions
{
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    /// <summary>
    /// The pager extension.
    /// </summary>
    public static class PagerExtension
    {
        #region Public Methods and Operators

        /// <summary>
        /// The pager.
        /// </summary>
        /// <param name="helper">
        /// The helper.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <param name="pageIndex">
        /// The page index.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="totalRecords">
        /// The total records.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string Pager(this HtmlHelper helper, string action, int pageIndex, int pageSize, int totalRecords)
        {
            string html = string.Empty;
            int pageCount = (totalRecords % pageSize == 0) ? (totalRecords / pageSize) : (totalRecords / pageSize) + 1;
            for (int idx = 1; idx <= pageCount; idx++)
            {
                if (idx - 1 == pageIndex)
                {
                    html += idx.ToString() + " ";
                }
                else
                {
                    if (idx == 1)
                    {
                        html += helper.ActionLink(idx.ToString(), action);
                    }
                    else
                    {
                        html += helper.ActionLink(idx.ToString(), action, new { id = idx - 1, page = idx - 1 });
                    }

                    html += " ";
                }
            }

            return html;
        }

        #endregion
    }
}