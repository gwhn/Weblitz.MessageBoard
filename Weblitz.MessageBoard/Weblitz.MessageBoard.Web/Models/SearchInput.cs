using System.ComponentModel.DataAnnotations;

namespace Weblitz.MessageBoard.Web.Models
{
    public class SearchInput
    {
        [Required]
        public string Query { get; set; }
    }
}