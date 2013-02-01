using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteBlog.Common;
using LiteBlog.Common.Contracts;

namespace LiteBlog.SqlDbLayer
{
    public class ServiceData : IServiceData
    {
        public List<ServiceItem> Load()
        {
            throw new NotImplementedException();
        }

        public void Save(List<ServiceItem> svcItems)
        {
            throw new NotImplementedException();
        }
    }
}
