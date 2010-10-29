namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Attachment : Entity
    {
        public virtual Entry Entry { get; set; }

        public virtual string FileName { get; set; }

        public virtual string ContentType { get; set; }

        public virtual int ContentLength { get; set; }
    }
}