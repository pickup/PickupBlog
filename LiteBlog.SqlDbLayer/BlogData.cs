using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteBlog.Common;
using LiteBlog.Common.Contracts;

namespace LiteBlog.SqlDbLayer
{
    public class BlogData : IBlogData
    {
        private BlogDbContext dbContext;

        public BlogData()
        {
            dbContext = new BlogDbContext();
        }

        /// <summary>
        /// 改变作者
        /// </summary>
        /// <param name="fileID">内容ID</param>
        /// <param name="author">作者</param>
        public void ChangeAuthor(string fileID, string author)
        {
            throw new NotImplementedException();
        }

        public void ChangeCategory(string fileID, string oldCatID, string newCatID)
        {
            throw new NotImplementedException();
        }

        public void Create(PostInfo postInfo)
        {
            throw new NotImplementedException();
        }

        public void Delete(string fileID)
        {
            throw new NotImplementedException();
        }

        public List<PostInfo> GetBlogItems()
        {
            throw new NotImplementedException();
        }

        public void Update(string fileID, string title, string catID)
        {
            throw new NotImplementedException();
        }
    }
}
