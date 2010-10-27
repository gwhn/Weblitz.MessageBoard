namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Topic : Entry
    {
        public virtual string Title { get; set; }

        public virtual Forum Forum { get; set; }

        public virtual Post[] Posts { get; set; }
    }
}