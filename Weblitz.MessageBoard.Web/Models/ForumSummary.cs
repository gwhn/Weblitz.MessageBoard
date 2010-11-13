using System;

namespace Weblitz.MessageBoard.Web.Models
{
    public class ForumSummary
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int TopicCount { get; set; }

        public int PostCount { get; set; }        
    }
}