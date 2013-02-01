using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteBlog.Common;
using LiteBlog.Common.Contracts;

namespace LiteBlog.SqlDbLayer
{
    public class DataContext : IDataContext
    {
        public IArchiveData ArchiveData
        {
            get { throw new NotImplementedException(); }
        }

        public IBlogData BlogData
        {
            get { throw new NotImplementedException(); }
        }

        public ICategoryData CategoryData
        {
            get { throw new NotImplementedException(); }
        }

        public ICommentData CommentData
        {
            get { throw new NotImplementedException(); }
        }

        public IDraftData DraftData
        {
            get { throw new NotImplementedException(); }
        }

        public IDraftPostData DraftPostData
        {
            get { throw new NotImplementedException(); }
        }

        public IPageData PageData
        {
            get { throw new NotImplementedException(); }
        }

        public IPostData PostData
        {
            get { throw new NotImplementedException(); }
        }

        public IServiceData ServiceData
        {
            get { throw new NotImplementedException(); }
        }

        public ISettingsData SettingsData
        {
            get { throw new NotImplementedException(); }
        }

        public IStatData StatData
        {
            get { throw new NotImplementedException(); }
        }
    }
}
