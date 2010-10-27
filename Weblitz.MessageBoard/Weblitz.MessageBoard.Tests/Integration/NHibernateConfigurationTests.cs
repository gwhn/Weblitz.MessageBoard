using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Weblitz.MessageBoard.Tests.Integration
{
    [TestFixture]
    public class NHibernateConfigurationTests
    {
        [Test]
        public void ShouldGenerateDatabaseSchema()
        {
            // Arrange
            var config = new Configuration()
                .Configure()
                .AddAssembly("Weblitz.MessageBoard.Infrastructure");

            // Act
            new SchemaExport(config).Execute(true, true, false);

            // Assert
        }
    }
}