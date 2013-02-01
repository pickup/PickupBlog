using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using LiteBlog.Common;
using LiteBlog.Common.Contracts;

namespace LiteBlog.SqlDbLayer
{
    public class CategoryData : ICategoryData
    {
        private BlogDbContext dbContext;

        public CategoryData()
        {
            dbContext = new BlogDbContext();
        }

        public void ChangeCount(string catID, int number)
        {
            try
            {
                var category = dbContext.CategorySet.FirstOrDefault(i => i.CatID == catID);
                category.Count = category.Count + number;
                this.dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Log("更新分类下文章数错误。", ex);
                throw new ApplicationException("更新分类下文章数错误。", ex);
            }
        }

        public void Delete(string catID)
        {
            try
            {
                var category = dbContext.CategorySet.FirstOrDefault(i => i.CatID == catID);
                dbContext.CategorySet.Remove(category);
                this.dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Log("删除归档错误。", ex);
                throw new ApplicationException("删除归档错误。", ex);
            }
        }

        public List<Category> GetCategories()
        {
            try
            {
                return dbContext.CategorySet.ToList();
            }
            catch (Exception ex)
            {
                Logger.Log("获取全部分类错误。", ex);
                throw new ApplicationException("获取全部分类错误。", ex);
            }
        }

        public void Insert(Category category)
        {
            var c = dbContext.CategorySet.FirstOrDefault(i => i.CatID == category.CatID);
            if (c == null)
            {
                try
                {
                    dbContext.CategorySet.Add(category);
                    this.dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.Log("创建分类错误。", ex);
                    throw new ApplicationException("创建分类错误。", ex);
                }
            }
            else
            {
                Logger.Log("分类已存在。");
                throw new ApplicationException("分类已存在。");
            }
        }

        public void Update(string oldID, Category category)
        {
            var c = dbContext.CategorySet.FirstOrDefault(i => i.CatID == oldID);
            if (c == null)
            {
                try
                {
                    dbContext.CategorySet.Add(category);
                    this.dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.Log("创建分类错误。", ex);
                    throw new ApplicationException("创建分类错误。", ex);
                }
            }
            else
            {
                try
                {
                    dbContext.Entry(category).State = EntityState.Modified;
                    this.dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.Log("更新分类错误。", ex);
                    throw new ApplicationException("更新分类错误。", ex);
                }
            }
        }
    }
}
