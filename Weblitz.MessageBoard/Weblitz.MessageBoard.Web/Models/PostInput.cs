using System;
using System.ComponentModel.DataAnnotations;

namespace Weblitz.MessageBoard.Web.Models
{
    public class PostInput
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [ScaffoldColumn(false)]
        public Guid TopicId { get; set; }

        [ScaffoldColumn(false)]
        public Guid? ParentId { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Body { get; set; }
    }
}