using System;

namespace Weblitz.MessageBoard.Web.Models
{
    public class DeleteItem
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public CancelNavigation CancelNavigation { get; set; }
    }
}