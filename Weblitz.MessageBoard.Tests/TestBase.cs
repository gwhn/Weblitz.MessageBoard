using Rhino.Mocks;

namespace Weblitz.MessageBoard.Tests
{
    public abstract class TestBase
    {
        protected static T Mock<T>(params object[] args) where T : class
        {
            return MockRepository.GenerateMock<T>(args);
        }

        protected static T Stub<T>(params object[] args) where T : class
        {
            return MockRepository.GenerateStub<T>(args);
        }
    }
    
}