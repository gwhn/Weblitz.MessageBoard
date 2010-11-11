using System;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Web.Models
{
    public class TopicDetail
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string ForumName { get; set; }

        public string Author { get; set; }

        public string PublishedDate { get; set; }

        public PostDetail[] Posts { get; set; }

        public Attachment[] Attachments { get; set; }
    }
}