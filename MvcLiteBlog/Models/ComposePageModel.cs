using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcLiteBlog.Models
{
    public class ComposePageModel
    {
        public string FileId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [Required(ErrorMessage = "Please enter a title for the page")]
        [RegularExpression(@"[\w-\?!:,\. ]+", ErrorMessage = "No special characters allowed in the title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter text for the page body")]
        public string Contents { get; set; }
    }
}