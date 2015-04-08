using AutoMapper;
namespace FileStore.Persistance.EntityFramework
{
    public static class FileStoreConfigurationExtensions
    {
        public static FileStoreConfiguration UseEntityFramework(this FileStoreConfiguration @this, string connectionStringName)
        {
            FileDbContext.ConnectionStringName = connectionStringName;

            @this.AddComponentForRegistration(typeof(IPersistance), typeof(EntityFrameworkPersistance));
            @this.AddComponentForRegistration(typeof(IFileDbContext), typeof(FileDbContext));
            @this.AddComponentForRegistration(typeof(IMappingEngine), () => Mapper.Engine);

            return @this;
        }
    }
}
