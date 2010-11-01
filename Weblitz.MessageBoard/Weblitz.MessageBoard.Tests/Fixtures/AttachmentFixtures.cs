using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Tests.Fixtures
{
    public static class AttachmentFixtures
    {
        public static Attachment Attachment(int index)
        {
            return new Attachment
                        {
                            FileName = string.Format("path/to/test/file{0}.name", index),
                            ContentType = "some/type",
                            ContentLength = 1234
                        };
        }
    }
}