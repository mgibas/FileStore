using AutoMapper;
using System;
using System.Linq;

namespace FileStore.Persistance.EntityFramework
{
  public class EntityFrameworkPersistance : IPersistance
  {
    public EntityFrameworkPersistance(IFileDbContext context, IMappingEngine mapper)
    {
      this.mapper = mapper;
      this.context = context;
    }

    private IMappingEngine mapper;
    private IFileDbContext context;

    public void Persist(Guid id, StoreFile fileToStore, int version)
    {
      var file = new StoredFile
      {
        FileId = id,
        Data = fileToStore.Data,
        Extension = fileToStore.Extension,
        Version = version
      };
      context.StoredFiles.Add(file);

      context.SaveChanges();
    }

    public StoreFile ReadFile(Guid id, int version)
    {
      var file = context.StoredFiles.SingleOrDefault(f => f.FileId == id && f.Version == version);
      if (file == null) return null;

      return mapper.DynamicMap<StoreFile>(file);
    }

    public bool FileExists(Guid id)
    {
      return context.StoredFiles.Any(f => f.FileId == id);
    }

    public int GetFileLatestVersion(Guid id)
    {
      return context.StoredFiles
          .Where(f => f.FileId == id)
          .Max(f => f.Version);
    }
  }
}
