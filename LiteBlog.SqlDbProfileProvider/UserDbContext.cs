using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace LiteBlog.SqlDbProfileProvider
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public short Locked { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime? LastActivityTime { get; set; }
        public DateTime? LastPasswordTime { get; set; }
        public DateTime? LastLockedTime { get; set; }
    }

    public class UserDbContext : DbContext
    {
        public UserDbContext()
            : base() { }

        public UserDbContext(string name)
            : base(name) { }

        public DbSet<User> UserSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("UserInfo");
            modelBuilder.Entity<User>().HasKey(u => u.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
