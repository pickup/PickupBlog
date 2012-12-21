using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace LiteBlog.SqlDbProfileProvider
{
    public class UserDbContext : DbContext
    {
        public UserDbContext()
            : base() { }

        public UserDbContext(string name)
            : base(name) { }


    }
}
