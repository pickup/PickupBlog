// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PublisherComp.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   Helper component to PostComp for publishing
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.BlogEngine
{
    using System;
    using System.Collections.Generic;
    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    using MvcLiteBlog.Helpers;

    /// <summary>
    /// Helper component to PostComp for publishing
    /// </summary>
    public class PublisherComp
    {
        #region Public Methods and Operators

        /// <summary>
        /// The publish.
        /// </summary>
        /// <param name="post">
        /// The post.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public static bool Publish(DraftPost post)
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(SettingsComp.GetSettings().Timezone);
            if (post.Time > LocalTime.GetCurrentTime(tzi))
            {
                ConfigHelper.DataContext.DraftData.AddSchedule(post.DraftID, post.Time);
                return false;
            }

            IDraftPostData postData = ConfigHelper.DataContext.DraftPostData;
            post.FileID = postData.Publish(post);

            UpdatePost(post);
            UpdateCategory(post.OldCatID, post.CatID);
            if (post.Type == PostType.New)
            {
                UpdateMonth(post.Time);
            }

            IDraftData draftData = ConfigHelper.DataContext.DraftData;
            draftData.Delete(post.DraftID);
            return true;
        }

        /// <summary>
        /// The publish scheduled.
        /// </summary>
        public static void PublishScheduled()
        {
            List<PostInfo> drafts = ConfigHelper.DataContext.DraftData.GetScheduledDraftItems();
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(SettingsComp.GetSettings().Timezone);
            foreach (PostInfo postInfo in drafts)
            {
                if (postInfo.Time < LocalTime.GetCurrentTime(tzi))
                {
                    DraftPost post = ConfigHelper.DataContext.DraftPostData.Load(postInfo.FileID);
                    Publish(post);
                }
            }
        }

        /// <summary>
        /// The unpublish.
        /// </summary>
        /// <param name="postInfo">
        /// The post info.
        /// </param>
        public static void Unpublish(PostInfo postInfo)
        {
            BlogComp.Delete(postInfo.FileID);
            if (postInfo.CatID != string.Empty)
            {
                string[] catIDs = postInfo.CatID.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string catID in catIDs)
                {
                    CategoryComp.Decrement(catID);
                }
            }

            ArchiveComp.Decrement(postInfo.Time.Month, postInfo.Time.Year);

            // Remove file 
            Post post2 = PostComp.Load(postInfo.FileID);
            foreach (Comment comment in post2.Comments)
            {
                CommentComp.Delete(comment.ID);
            }

            PostComp.Delete(postInfo.FileID);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The update category.
        /// </summary>
        /// <param name="oldCatID">
        /// The old cat id.
        /// </param>
        /// <param name="newCatID">
        /// The new cat id.
        /// </param>
        private static void UpdateCategory(string oldCatID, string newCatID)
        {
            if (oldCatID != newCatID)
            {
                string[] oldIDs = oldCatID.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                string[] newIDs = newCatID.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string oldID in oldIDs)
                {
                    CategoryComp.Decrement(oldID);
                }

                foreach (string newID in newIDs)
                {
                    CategoryComp.Increment(newID);
                }
            }
        }

        /// <summary>
        /// The update month.
        /// </summary>
        /// <param name="publishDate">
        /// The publish date.
        /// </param>
        private static void UpdateMonth(DateTime publishDate)
        {
            ArchiveComp.Increment(publishDate.Month, publishDate.Year);
        }

        /// <summary>
        /// The update post.
        /// </summary>
        /// <param name="post">
        /// The post.
        /// </param>
        private static void UpdatePost(DraftPost post)
        {
            if (post.Type == PostType.New)
            {
                PostInfo postInfo = new PostInfo();
                postInfo.FileID = post.FileID;
                postInfo.Author = post.Author;
                postInfo.Title = post.Title;
                postInfo.CatID = post.CatID;
                postInfo.Time = post.Time;
                BlogComp.Create(postInfo);
            }
            else
            {
                BlogComp.Update(post.FileID, post.Title, post.CatID);
            }
        }

        #endregion
    }
}