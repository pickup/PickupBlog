using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteBlog.Common.Contracts
{
    public interface IPageData
    {
        Page Load(string fileId);
        void Save(Page page);
        List<Page> GetPages();
        List<Page> GetPublishedPages();
        void Delete(string fileId);
        bool IsUniqueId(string fileId);
    }
}
