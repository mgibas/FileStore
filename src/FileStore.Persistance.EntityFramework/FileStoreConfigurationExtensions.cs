using AutoMapper;
namespace FileStore.Persistance.EntityFramework
{
    public static class FileStoreConfigurationExtensions
    {
        public static FileStoreConfiguration UseEntityFramework(this FileStoreConfiguration @this, string connectionStringName)
        {
            FileDbContext.ConnectionStringName = connectionStringName;

            @this.ComponentsForRegistration.Add(typeof(IPersistance), typeof(EntityFrameworkPersistance));
            @this.ComponentsForRegistration.Add(typeof(IFileDbContext), typeof(FileDbContext));
            @this.LambdaComponentsForRegistration.Add(typeof(IMappingEngine), () => Mapper.Engine);

            return @this;
        }
    }
}
