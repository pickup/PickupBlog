// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorComp.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The engine exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.BlogEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Security;
    using LiteBlog.Common;

    using MvcLiteBlog.Helpers;

    /// <summary>
    /// The author comp.
    /// </summary>
    public class AuthorComp
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="author">
        /// The author.
        /// </param>
        /// <returns>
        /// The LiteBlog.BlogEngine.EngineException.
        /// </returns>
        public static EngineException Create(Author author)
        {
            MembershipUser user = Membership.GetUser(author.ID);
            if (user != null)
            {
                EngineException ex = new EngineException("Author with the same user name already exists");
                return ex;
            }

            try
            {
                user = Membership.CreateUser(author.ID, ConfigHelper.DefaultPassword, author.Email);
                if (user != null)
                {
                    ProfileComp.SetDisplayName(author.ID, author.Name);
                }
            }
            catch (Exception inner)
            {
                EngineException ex = new EngineException("Creating a new user failed", inner);
                return ex;
            }

            return null;
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="authorID">
        /// The author id.
        /// </param>
        /// <returns>
        /// The LiteBlog.BlogEngine.EngineException.
        /// </returns>
        public static EngineException Delete(string authorID)
        {
            try
            {
                if (Membership.GetAllUsers().Count < 2)
                {
                    EngineException ex = new EngineException("The primary author cannot be deleted");
                    return ex;
                }

                Membership.DeleteUser(authorID);

                List<PostInfo> posts = BlogComp.GetPostsByAuthor(authorID);
                string defaultAuthor = AuthorComp.GetDefaultAuthor().ID;

                foreach (PostInfo postInfo in posts)
                {
                    PostComp.ChangeAuthor(postInfo.FileID, defaultAuthor);
                    BlogComp.ChangeAuthor(postInfo.FileID, defaultAuthor);
                }
            }
            catch (Exception inner)
            {
                EngineException ex = new EngineException("Author could not be deleted", inner);
                return ex;
            }

            return null;
        }

        /// <summary>
        /// The get author.
        /// </summary>
        /// <param name="authorID">
        /// The author id.
        /// </param>
        /// <returns>
        /// The LiteBlog.Common.Author.
        /// </returns>
        public static Author GetAuthor(string authorID)
        {
            var qry = from auth in GetAuthors() where auth.ID == authorID select auth;

            return qry.FirstOrDefault<Author>();
        }

        /// <summary>
        /// The get author name.
        /// </summary>
        /// <param name="authorID">
        /// The author id.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetAuthorName(string authorID)
        {
            return authorID == string.Empty ? string.Empty : GetAuthor(authorID).Name;
        }

        /// <summary>
        /// The get authors.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.Author].
        /// </returns>
        public static List<Author> GetAuthors()
        {
            List<Author> authors = new List<Author>();
            foreach (MembershipUser user in Membership.GetAllUsers())
            {
                Author author = new Author();
                author.ID = user.UserName;
                author.Email = user.Email;
                author.Locked = user.IsLockedOut;
                author.Name = ProfileComp.GetDisplayName(author.ID);
                authors.Add(author);
            }

            return authors;
        }

        /// <summary>
        /// The get default author.
        /// </summary>
        /// <returns>
        /// The LiteBlog.Common.Author.
        /// </returns>
        public static Author GetDefaultAuthor()
        {
            return GetAuthors().First<Author>();
        }

        /// <summary>
        /// The unlock.
        /// </summary>
        /// <param name="authorID">
        /// The author id.
        /// </param>
        /// <returns>
        /// The LiteBlog.BlogEngine.EngineException.
        /// </returns>
        public static EngineException Unlock(string authorID)
        {
            try
            {
                MembershipUser user = Membership.GetUser(authorID);
                user.UnlockUser();
            }
            catch (Exception inner)
            {
                EngineException ex = new EngineException("Author could not be unlocked", inner);
                return ex;
            }

            return null;
        }

        /// <summary>
        /// ID should be the same for update to work
        /// </summary>
        /// <param name="oldID">
        /// The old ID.
        /// </param>
        /// <param name="author">
        /// The author
        /// </param>
        /// <returns>
        /// The LiteBlog.BlogEngine.EngineException.
        /// </returns>
        public static EngineException Update(string oldID, Author author)
        {
            if (author.ID != oldID)
            {
                // change the author ID (lot of checks)
                MembershipUser user = Membership.GetUser(author.ID);
                if (user != null)
                {
                    EngineException ex = new EngineException("Another author has the same username");
                    return ex;
                }
                else
                {
                    // Get password
                    MembershipUser oldUser = Membership.GetUser(oldID);
                    string password = oldUser.GetPassword();
                    try
                    {
                        user = Membership.CreateUser(author.ID, password, author.Email);
                        Membership.DeleteUser(oldID);

                        if (user != null)
                        {
                            foreach (PostInfo postInfo in BlogComp.GetPostsByAuthor(oldID))
                            {
                                BlogComp.ChangeAuthor(postInfo.FileID, user.UserName);
                                PostComp.ChangeAuthor(postInfo.FileID, user.UserName);
                            }
                        }
                    }
                    catch (Exception inner)
                    {
                        EngineException ex = new EngineException("Updating the author details failed", inner);
                        return ex;
                    }
                }
            }
            else
            {
                try
                {
                    MembershipUser user = Membership.GetUser(author.ID);
                    if (user != null)
                    {
                        user.Email = author.Email;
                        Membership.UpdateUser(user);
                    }
                }
                catch (Exception inner)
                {
                    EngineException ex = new EngineException("Updating the author details failed", inner);
                    return ex;
                }
            }

            ProfileComp.SetDisplayName(author.ID, author.Name);

            return null;
        }

        #endregion
    }
}