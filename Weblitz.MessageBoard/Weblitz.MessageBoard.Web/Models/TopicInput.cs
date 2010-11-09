using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Weblitz.MessageBoard.Web.Models
{
    public class TopicInput
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Required, StringLength(256)]
        public string Title { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [DisplayName("Sticky?")]
        public bool Sticky { get; set; }

        [DisplayName("Closed?")]
        public bool Closed { get; set; }

        [ScaffoldColumn(false), DisplayName("Forum")]
        public Guid ForumId { get; set; }

        [ScaffoldColumn(false)]
        public IEnumerable<SelectListItem> Forums { get; set; }
    }
}