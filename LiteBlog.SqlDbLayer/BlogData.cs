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
