using System.Data.Entity;

namespace FileStore.Persistance.EntityFramework
{
    public class FileDbContext : DbContext, IFileDbContext
    {
        public FileDbContext()
            : base(ConnectionStringName)
        { }

        public static string ConnectionStringName;

        public DbSet<StoredFile> StoredFiles { get; set; }
    }

}
