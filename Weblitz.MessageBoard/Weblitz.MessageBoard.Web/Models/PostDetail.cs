using System;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models
{
    public class PostDetail
    {
        public Guid Id { get; set; }

        public string ForumName { get; set; }

        public Guid TopicId { get; set; }

        public string TopicTitle { get; set; }

        public string Body { get; set; }

        public string Author { get; set; }

        public string PublishedDate { get; set; }

        public PostDetail[] Children { get; set; }

        public int Level { get; set; }

        public Attachment[] Attachments { get; set; }
    }
}