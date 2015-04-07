using System.Data.Entity;

namespace FileStore.Persistance.EntityFramework
{
    public interface IFileDbContext
    {
        DbSet<StoredFile> StoredFiles { get; }

        int SaveChanges();
    }
}
