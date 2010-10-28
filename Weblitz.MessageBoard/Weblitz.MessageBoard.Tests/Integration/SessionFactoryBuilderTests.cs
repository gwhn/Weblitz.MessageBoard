using NUnit.Framework;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Infrastructure.NHibernate;

namespace Weblitz.MessageBoard.Tests.Integration
{
    [TestFixture]
    public class SessionFactoryBuilderTests : TestBase
    {
        [Test]
        public void ShouldCreateNHibernateConfiguration()
        {
            // Arrange
            var builder = new SessionFactoryBuilder();
            
            // Act
            var factory = builder.Construct();

            // Assert
            Assert.IsNotNull(factory);

            var type = typeof (Forum);
            var forum = factory.GetClassMetadata(type);
            Assert.That(forum.EntityName == type.Name);

            type = typeof (Entry);
            var entry = factory.GetClassMetadata(type);
            Assert.That(entry.EntityName == type.Name);

            type = typeof (Topic);
            var topic = factory.GetClassMetadata(type);
            Assert.That(topic.EntityName == type.Name);

            type = typeof (Post);
            var post = factory.GetClassMetadata(type);
            Assert.That(post.EntityName == type.Name);

            type = typeof (Attachment);
            var attachment = factory.GetClassMetadata(type);
            Assert.That(attachment.EntityName == type.Name);
        }

        [Test]
        public void ShouldStoreSessionFactoryInSingleton()
        {
            // Act
            var builder = new SessionFactoryBuilder();

            // Arrange
            var factory1 = builder.Construct();
            var factory2 = builder.Construct();

            // Assert
            Assert.IsNotNull(factory1);
            Assert.IsNotNull(factory2);

            Assert.ReferenceEquals(factory1, factory2);
        }
    }
}