using System;

namespace Weblitz.MessageBoard.Web.Models
{
    public class ForumDetail
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public TopicSummary[] Topics { get; set; }
    }
}