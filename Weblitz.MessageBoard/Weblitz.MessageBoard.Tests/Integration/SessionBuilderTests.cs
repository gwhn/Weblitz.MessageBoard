using NUnit.Framework;
using Weblitz.MessageBoard.Infrastructure.NHibernate.Builders;

namespace Weblitz.MessageBoard.Tests.Integration
{
	[TestFixture]
    public class SessionBuilderTests : IntegrationTestBase
	{
		[Test]
		public void ShouldCreateNewSession()
		{
            // Arrange
			var builder = new SessionBuilder();

            // Act
			var session = builder.Construct();

            // Assert
            Assert.IsNotNull(session);
            Assert.That(session.IsOpen);
		}

		[Test]
		public void ShouldReuseSameInstance()
		{
            // Arrange
            var builder = new SessionBuilder();
            var session1 = builder.Construct();
            Assert.IsNotNull(session1);
            
            // Act
            var session2 = builder.Construct();

            // Assert
		    Assert.That(ReferenceEquals(session1, session2));
		}

		[Test]
		public void ShouldCreateNewSessionWhenPreviousSessionClosed()
		{
            // Arrange
            var builder = new SessionBuilder();
            var session1 = builder.Construct();
            session1.Close();

            // Act
            var session2 = builder.Construct();

            // Assert
			Assert.That(session2.IsOpen);
		}

		[Test]
        public void ShouldCreateNewSessionWhenPreviousSessionDisposed()
		{
            // Arrange
            var builder = new SessionBuilder();
            var session1 = builder.Construct();
            session1.Dispose();

            // Act
            var session2 = builder.Construct();

            // Assert
			Assert.That(session2.IsOpen);
		}
	}
}