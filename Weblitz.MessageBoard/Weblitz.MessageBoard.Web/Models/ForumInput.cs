using System;
using System.ComponentModel.DataAnnotations;

namespace Weblitz.MessageBoard.Web.Models
{
    public class ForumInput
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Required, StringLength(256)]
        public string Name { get; set; }
    }
}