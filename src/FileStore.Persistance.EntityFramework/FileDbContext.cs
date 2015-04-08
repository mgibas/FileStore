using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;

namespace FileStore.Persistance.EntityFramework
{
  [ExcludeFromCodeCoverage]
  public class FileDbContext : DbContext, IFileDbContext
  {
    public FileDbContext(string connectionStringName)
      : base(connectionStringName)
    { }

    public DbSet<StoredFile> StoredFiles { get; set; }
  }
}
