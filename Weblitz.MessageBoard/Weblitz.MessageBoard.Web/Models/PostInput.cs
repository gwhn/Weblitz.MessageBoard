using System;
using System.ComponentModel;
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

        [Required, StringLength(256)]
        public string Name { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Body { get; set; }
    }
}