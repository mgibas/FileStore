using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
namespace FileStore.Persistance.EntityFramework
{
  [ExcludeFromCodeCoverage]
  public static class FileStoreConfigurationExtensions
  {
    public static IFileStoreConfigurator UseEntityFramework(this IFileStoreConfigurator @this, string connectionStringName)
    {
      @this.UsePersistance(new EntityFrameworkPersistance(new FileDbContext(connectionStringName), Mapper.Engine));
      return @this;
    }

    public static IFileStoreConfigurator InitializeDatabase(this IFileStoreConfigurator @this)
    {
      Database.SetInitializer(new CreateDatabaseIfNotExists<FileDbContext>());
      return @this;
    }
  }
}
