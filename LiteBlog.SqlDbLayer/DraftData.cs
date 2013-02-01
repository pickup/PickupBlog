using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteBlog.Common;
using LiteBlog.Common.Contracts;

namespace LiteBlog.SqlDbLayer
{
    public class DraftData : IDraftData
    {
        public void AddSchedule(string draftID, DateTime publishDate)
        {
            throw new NotImplementedException();
        }

        public void Delete(string draftID)
        {
            throw new NotImplementedException();
        }

        public List<PostInfo> GetDraftItems()
        {
            throw new NotImplementedException();
        }

        public List<PostInfo> GetScheduledDraftItems()
        {
            throw new NotImplementedException();
        }

        public void Insert(string draftID, string title)
        {
            throw new NotImplementedException();
        }
    }
}
