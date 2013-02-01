using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteBlog.Common;
using LiteBlog.Common.Contracts;

namespace LiteBlog.SqlDbLayer
{
    public class CommentData : ICommentData
    {
        public void Approve(string commentID)
        {
            throw new NotImplementedException();
        }

        public void Delete(string commentID)
        {
            throw new NotImplementedException();
        }

        public List<Comment> GetComments()
        {
            throw new NotImplementedException();
        }

        public string Insert(Comment comment)
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void Update(Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}
