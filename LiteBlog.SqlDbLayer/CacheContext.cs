using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteBlog.Common;
using System.Web.Caching;
using LiteBlog.Common.Contracts;

namespace LiteBlog.SqlDbLayer
{
    public class CacheContext : ICacheContext
    {
        public CacheDependency GetDependency(CacheType type)
        {
            throw new NotImplementedException();
        }

        public CacheDependency GetDependency(CacheType type, string id)
        {
            throw new NotImplementedException();
        }
    }
}
