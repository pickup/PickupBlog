using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteBlog.Common;
using LiteBlog.Common.Contracts;

namespace LiteBlog.SqlDbLayer
{
    public class PageData : IPageData
    {
        public Page Load(string fileId)
        {
            throw new NotImplementedException();
        }

        public void Save(Page page)
        {
            throw new NotImplementedException();
        }

        public List<Page> GetPages()
        {
            throw new NotImplementedException();
        }

        public List<Page> GetPublishedPages()
        {
            throw new NotImplementedException();
        }

        public void Delete(string fileId)
        {
            throw new NotImplementedException();
        }

        public bool IsUniqueId(string fileId)
        {
            throw new NotImplementedException();
        }
    }
}
