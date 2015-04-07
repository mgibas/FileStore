using Xunit;
namespace FileStore.Tests.Unit
{
    public class FileStoreConfigurationTests
    {
        [Fact]
        public void Init_ReturnsDefaultInstanceOfConfigurator()
        {
            var configurator = FileStoreConfiguration.Init();

            Assert.NotNull(configurator);
        }
    }
}
