using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteBlog.Common;
using LiteBlog.Common.Contracts;

namespace LiteBlog.SqlDbLayer
{
    public class PostData : IPostData
    {
        public void Delete(string fileID)
        {
            throw new NotImplementedException();
        }

        public void DeleteComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public void InsertComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public Post Load(string fileID)
        {
            throw new NotImplementedException();
        }

        public void Save(Post post)
        {
            throw new NotImplementedException();
        }

        public void UpdateComment(Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}
