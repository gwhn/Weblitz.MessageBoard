namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Attachment : Entity
    {
        public virtual Entry Entry { get; set; }

        public virtual string FileName { get; set; }

        public virtual string ContentType { get; set; }

        public virtual int ContentLength { get; set; }

        public virtual bool Equals(Attachment other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other.FileName, FileName) && Equals(other.ContentType, ContentType) &&
                   other.ContentLength == ContentLength;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as Attachment);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ (FileName != null ? FileName.GetHashCode() : 0);
                result = (result*397) ^ (ContentType != null ? ContentType.GetHashCode() : 0);
                result = (result*397) ^ ContentLength;
                return result;
            }
        }
    }
}