// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogComp.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The blog comp.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.BlogEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    using MvcLiteBlog.Helpers;

    /// <summary>
    /// The blog comp.
    /// </summary>
    public class BlogComp
    {
        #region Public Methods and Operators

        /// <summary>
        /// The change author.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <param name="author">
        /// The author.
        /// </param>
        public static void ChangeAuthor(string fileID, string author)
        {
            IBlogData data = ConfigHelper.DataContext.BlogData;
            data.ChangeAuthor(fileID, author);
        }

        /// <summary>
        /// The change category.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <param name="oldCatID">
        /// The old cat id.
        /// </param>
        /// <param name="newCatID">
        /// The new cat id.
        /// </param>
        public static void ChangeCategory(string fileID, string oldCatID, string newCatID)
        {
            IBlogData data = ConfigHelper.DataContext.BlogData;
            data.ChangeCategory(fileID, oldCatID, newCatID);
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="postInfo">
        /// The post info.
        /// </param>
        public static void Create(PostInfo postInfo)
        {
            IBlogData data = ConfigHelper.DataContext.BlogData;
            data.Create(postInfo);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        public static void Delete(string fileID)
        {
            IBlogData data = ConfigHelper.DataContext.BlogData;
            data.Delete(fileID);
        }

        /// <summary>
        /// The get default posts.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.PostInfo].
        /// </returns>
        public static List<PostInfo> GetDefaultPosts()
        {
            PostCollection postCol = GetBlogInfo();
            return postCol.Posts.Take<PostInfo>(SettingsComp.GetSettings().PostCount).ToList<PostInfo>();
        }

        /// <summary>
        /// The get post info.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <returns>
        /// The LiteBlog.Common.PostInfo.
        /// </returns>
        public static PostInfo GetPostInfo(string fileID)
        {
            List<PostInfo> posts = GetBlogInfo().Posts;
            var qry = from post in posts where post.FileID == fileID select post;

            if (qry.Count<PostInfo>() == 1)
            {
                return qry.First<PostInfo>();
            }

            return null;
        }

        /// <summary>
        /// The get posts.
        /// </summary>
        /// <param name="bRefresh">
        /// The b refresh.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.PostInfo].
        /// </returns>
        public static List<PostInfo> GetPosts(bool bRefresh = false)
        {
            return GetBlogInfo(bRefresh).Posts;
        }

        /// <summary>
        /// The get posts by author.
        /// </summary>
        /// <param name="author">
        /// The author.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.PostInfo].
        /// </returns>
        public static List<PostInfo> GetPostsByAuthor(string author)
        {
            PostCollection postCol = GetBlogInfo();
            List<PostInfo> posts = postCol.Posts;
            var qry = from postInfo in posts where postInfo.Author == author select postInfo;
            return qry.ToList<PostInfo>();
        }

        /// <summary>
        /// The get posts by category.
        /// </summary>
        /// <param name="catID">
        /// The cat id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.PostInfo].
        /// </returns>
        public static List<PostInfo> GetPostsByCategory(string catID)
        {
            List<PostInfo> list = new List<PostInfo>();
            PostCollection postCol = GetBlogInfo();
            if (postCol.CategoryMap.ContainsKey(catID))
            {
                list = postCol.CategoryMap[catID];
            }
            else
            {
                List<PostInfo> posts = postCol.Posts;
                foreach (PostInfo postInfo in posts)
                {
                    string[] catIDs = postInfo.CatID.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string id in catIDs)
                    {
                        if (id.ToLower() == catID.ToLower())
                        {
                            list.Add(postInfo);
                            break;
                        }
                    }
                }

                postCol.CategoryMap[catID] = list;
                CacheHelper.Put(CacheType.Blog, postCol);
            }

            return list;
        }

        /// <summary>
        /// The get posts by month.
        /// </summary>
        /// <param name="year">
        /// The year.
        /// </param>
        /// <param name="month">
        /// The month.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.PostInfo].
        /// </returns>
        public static List<PostInfo> GetPostsByMonth(int year, int month)
        {
            string monthID = PostInfo.GetMonthID(new DateTime(year, month, 1));

            List<PostInfo> list = new List<PostInfo>();
            PostCollection postCol = GetBlogInfo();
            if (postCol.CategoryMap.ContainsKey(monthID))
            {
                list = postCol.CategoryMap[monthID];
            }
            else
            {
                List<PostInfo> posts = postCol.Posts;
                var qry = from postInfo in posts where postInfo.MonthID == monthID select postInfo;
                list = qry.ToList<PostInfo>();
                postCol.CategoryMap[monthID] = list;
                CacheHelper.Put(CacheType.Blog, postCol);
            }

            return list;
        }

        /// <summary>
        /// The post exists.
        /// </summary>
        /// <param name="fileId">
        /// The file id.
        /// </param>
        /// <param name="fileId2">
        /// The file id 2.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public static bool PostExists(string fileId, out string fileId2)
        {
            fileId2 = null;
            var qry = from p in GetPosts() where p.FileID.ToLower() == fileId.ToLower() select p.FileID;

            if (qry.Count<string>() == 1)
            {
                fileId2 = qry.First<string>();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates a previously published post
        /// Cannot update time / author of publishing
        /// </summary>
        /// <param name="fileID">
        /// The file ID.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="catID">
        /// The cat ID.
        /// </param>
        public static void Update(string fileID, string title, string catID)
        {
            IBlogData data = ConfigHelper.DataContext.BlogData;
            data.Update(fileID, title, catID);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get blog info.
        /// </summary>
        /// <param name="bRefresh">
        /// The b refresh.
        /// </param>
        /// <returns>
        /// The LiteBlog.Common.PostCollection.
        /// </returns>
        private static PostCollection GetBlogInfo(bool bRefresh = false)
        {
            PostCollection postCol = CacheHelper.Get<PostCollection>(CacheType.Blog);
            if (postCol == null || bRefresh)
            {
                IBlogData data = ConfigHelper.DataContext.BlogData;
                List<PostInfo> posts = data.GetBlogItems();
                postCol = new PostCollection();
                postCol.Posts = posts;
                CacheHelper.Put(CacheType.Blog, postCol);
            }

            return postCol;
        }

        #endregion
    }
}