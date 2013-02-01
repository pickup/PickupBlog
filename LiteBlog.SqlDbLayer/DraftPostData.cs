using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteBlog.Common;
using LiteBlog.Common.Contracts;

namespace LiteBlog.SqlDbLayer
{
    public class DraftPostData : IDraftPostData
    {
        public DraftPost Create(string title, string author, DateTime time, string catID)
        {
            throw new NotImplementedException();
        }

        public DraftPost Create(string fileID)
        {
            throw new NotImplementedException();
        }

        public void Delete(string fileID)
        {
            throw new NotImplementedException();
        }

        public DraftPost Load(string fileID)
        {
            throw new NotImplementedException();
        }

        public string Publish(DraftPost post)
        {
            throw new NotImplementedException();
        }

        public void Save(DraftPost post)
        {
            throw new NotImplementedException();
        }
    }
}
