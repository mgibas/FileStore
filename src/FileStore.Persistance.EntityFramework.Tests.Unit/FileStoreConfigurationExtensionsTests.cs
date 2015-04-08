using AutoMapper;
using System;
using Xunit;

namespace FileStore.Persistance.EntityFramework.Tests.Unit
{
    public class FileStoreConfigurationExtensionsTests
    {
        private FileStoreConfiguration configuration;

        public FileStoreConfigurationExtensionsTests()
        {
            configuration = new FileStoreConfiguration();
        }

        [Fact]
        public void UseEntityFramework_ReturnsExtendedInstance()
        {
            Assert.Same(configuration, configuration.UseEntityFramework("as"));
        }

        [Fact]
        public void UseEntityFramework_ConnectionStringProvided_SetsFileDbContectConnectionStringName()
        {
            var connectionString = "some";

            configuration.UseEntityFramework(connectionString);

            Assert.Equal(connectionString, FileDbContext.ConnectionStringName);
        }

        [Theory]
        [InlineData(typeof(IFileDbContext), typeof(FileDbContext))]
        public void UseEntityFramework_AddsNecesaryCommponentsForRegistration(Type service, Type implementation)
        {
            configuration.UseEntityFramework("asdsa");

            Assert.True(configuration.GetComponentsForRegistration().ContainsKey(service));
            Assert.Equal(implementation, configuration.GetComponentsForRegistration()[service]);
        }

        [Fact]
        public void UseEntityFramework_AddsAutomapperDelegateCommponentForRegistration()
        {
            configuration.UseEntityFramework("asdsa");

            Assert.True(configuration.GetDelegateComponentsForRegistration().ContainsKey(typeof(IMappingEngine)));
            Assert.Equal(Mapper.Engine, configuration.GetDelegateComponentsForRegistration()[typeof(IMappingEngine)].Invoke());
        }
    }
}
