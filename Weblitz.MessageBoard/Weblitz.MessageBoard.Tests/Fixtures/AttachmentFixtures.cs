using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Tests.Fixtures
{
    public static class AttachmentFixtures
    {
        public static Attachment Attachment
        {
            get
            {
                return new Attachment
                           {
                               FileName = "path/to/test/file.name",
                               ContentType = "some/type",
                               ContentLength = 1234
                           };
            }
        }
    }
}