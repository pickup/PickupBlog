using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using LiteBlog.Common;

namespace LiteBlog.SqlDbLayer
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext()
            : base() { }

        public BlogDbContext(string name)
            : base(name) { }

        public DbSet<ArchiveMonth> ArchiveSet { get; set; }
        public DbSet<Category> CategorySet { get; set; }
        public DbSet<PostInfo> PostInfoSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //归档表
            modelBuilder.Entity<ArchiveMonth>().ToTable("Archive");

            //分类表
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Category>().Property(i => i.CatID).HasColumnName("Id");
            modelBuilder.Entity<Category>().Property(i => i.Order).HasColumnName("OrderNumber");

            //博文概要表
            modelBuilder.Entity<PostInfo>().ToTable("Blog");
            modelBuilder.Entity<PostInfo>().Property(i => i.FileID).HasColumnName("Id");
            modelBuilder.Entity<PostInfo>().Property(i => i.CatID).HasColumnName("CategoryId");
            modelBuilder.Entity<PostInfo>().Property(i => i.Time).HasColumnName("UpdateTime");

            base.OnModelCreating(modelBuilder);
        }
    }
}
