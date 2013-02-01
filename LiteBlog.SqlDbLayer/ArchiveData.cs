using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteBlog.Common;
using LiteBlog.Common.Contracts;

namespace LiteBlog.SqlDbLayer
{
    public class ArchiveData : IArchiveData
    {
        private BlogDbContext dbContext;

        public ArchiveData()
        {
            dbContext = new BlogDbContext();
        }

        /// <summary>
        /// 更新归档中文章的数目
        /// </summary>
        /// <param name="archiveID">归档ID</param>
        /// <param name="number">变化的数量</param>
        public void ChangeCount(string archiveID, int number)
        {
            try
            {
                int id = int.Parse(archiveID);
                var archive = dbContext.ArchiveSet.FirstOrDefault(i => i.ID == id);
                archive.Count = archive.Count + number;
                this.dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Log("更新归档文章数错误。", ex);
                throw new ApplicationException("更新归档文章数错误。", ex);
            }
        }

        /// <summary>
        /// 新建一个归档
        /// </summary>
        /// <param name="month">表示归档的月份对象</param>
        public void Create(ArchiveMonth month)
        {
            var archive = dbContext.ArchiveSet.FirstOrDefault(i => i.ID == month.ID);
            if (archive == null)
            {
                try
                {
                    dbContext.ArchiveSet.Add(month);
                    this.dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.Log("创建归档错误。", ex);
                    throw new ApplicationException("创建归档错误。", ex);
                }
            }
            else
            {
                Logger.Log("归档已存在。");
                throw new ApplicationException("归档已存在。");
            }
        }

        /// <summary>
        /// 删除一个归档
        /// </summary>
        /// <param name="archiveID">归档ID</param>
        public void Delete(string archiveID)
        {
            try
            {
                int id = int.Parse(archiveID);
                var archive = dbContext.ArchiveSet.FirstOrDefault(i => i.ID == id);
                dbContext.ArchiveSet.Remove(archive);
                this.dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Log("删除归档错误。", ex);
                throw new ApplicationException("删除归档错误。", ex);
            }
        }

        /// <summary>
        /// 获取全部归档
        /// </summary>
        /// <returns>全部归档</returns>
        public List<ArchiveMonth> GetArchiveMonths()
        {
            try
            {
                return dbContext.ArchiveSet.ToList();
            }
            catch (Exception ex)
            {
                Logger.Log("获取全部归档错误。", ex);
                throw new ApplicationException("获取全部归档错误。", ex);
            }
        }
    }
}
