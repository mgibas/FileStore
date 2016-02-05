using System.Data.Entity.Infrastructure;

namespace FileStore.Persistance.EntityFramework.Migrations
{
    public class MigrationsContextFactory : IDbContextFactory<FileDbContext>
    {
        public FileDbContext Create()
        {
            return new FileDbContext(FileStoreConfiguratorExtensions.ConnectionString);
        }
    }
}
