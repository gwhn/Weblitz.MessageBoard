using System;

namespace Weblitz.MessageBoard.Web.Models
{
    public class PostDetail
    {
        public Guid Id { get; set; }

        public string Body { get; set; }

        public string Author { get; set; }

        public string PublishedDate { get; set; }
    }
}