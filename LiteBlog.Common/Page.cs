using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteBlog.Common
{
    public class Page
    {
        public string FileId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool Published { get; set; }
    }
}
