using System;

namespace Weblitz.MessageBoard.Web.Models
{
    public class TopicSummary
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int PostCount { get; set; }

        public bool Sticky { get; set; }

        public bool Closed { get; set; }
    }
}