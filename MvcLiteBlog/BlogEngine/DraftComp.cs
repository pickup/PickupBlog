// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DraftComp.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The draft comp.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.BlogEngine
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    using MvcLiteBlog.Helpers;

    /// <summary>
    /// The draft comp.
    /// </summary>
    public class DraftComp
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="time">
        /// The time.
        /// </param>
        /// <param name="catID">
        /// The cat id.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string Create(string title, DateTime time, string catID)
        {
            IDraftPostData data = ConfigHelper.DataContext.DraftPostData;
            DraftPost post = data.Create(title, HttpContext.Current.User.Identity.Name, time, catID);
            return post.DraftID;
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string Create(string fileID)
        {
            IDraftPostData data = ConfigHelper.DataContext.DraftPostData;
            DraftPost post;
            if (!DraftPost.IsDraft(fileID))
            {
                post = data.Create(fileID);
                return post.DraftID;
            }

            return string.Empty;
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="draftID">
        /// The draft id.
        /// </param>
        public static void Delete(string draftID)
        {
            IDraftPostData data = ConfigHelper.DataContext.DraftPostData;
            data.Delete(draftID);
            ConfigHelper.DataContext.DraftData.Delete(draftID);
        }

        /// <summary>
        /// The get drafts.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.PostInfo].
        /// </returns>
        public static List<PostInfo> GetDrafts()
        {
            return ConfigHelper.DataContext.DraftData.GetDraftItems();
        }

        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="draftID">
        /// The draft id.
        /// </param>
        /// <returns>
        /// The LiteBlog.Common.DraftPost.
        /// </returns>
        public static DraftPost Load(string draftID)
        {
            IDraftPostData data = ConfigHelper.DataContext.DraftPostData;
            DraftPost post;
            if (DraftPost.IsDraft(draftID))
            {
                post = data.Load(draftID);
            }
            else
            {
                Post post2 = PostComp.Load(draftID);
                post = new DraftPost(post2);

                // post.GetCode();
                // post.RestoreCode();
            }

            //// for earlier versions!
            // post.GetCode();
            // post.RestoreCode();
            return post;
        }

        /// <summary>
        /// The remove document.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        public static void RemoveDocument(string path)
        {
            string fullPath = HttpContext.Current.Server.MapPath(path);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="post">
        /// The post.
        /// </param>
        public static void Save(DraftPost post)
        {
            IDraftPostData data = ConfigHelper.DataContext.DraftPostData;
            data.Save(post);
        }

        /// <summary>
        /// The store document.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="docPath">
        /// The doc path.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string StoreDocument(HttpPostedFileBase file, string docPath)
        {
            string fileName = Path.GetFileName(file.FileName);
            int extnPos = fileName.LastIndexOf('.');
            string fileID = fileName.Substring(0, extnPos);
            string extn = fileName.Substring(extnPos, fileName.Length - extnPos);
            string path = string.Format("{0}{1}", docPath, fileName);

            if (File.Exists(path))
            {
                int index = 1;
                fileName = string.Format("{0}-{1}{2}", fileID, index, extn);
                path = string.Format("{0}{1}", docPath, fileName);
                while (File.Exists(path))
                {
                    index++;
                    fileName = string.Format("{0}-{1}{2}", fileID, index, extn);
                    path = string.Format("{0}{1}", docPath, fileName);
                }
            }

            file.SaveAs(path);
            return fileName;
        }

        #endregion
    }
}