using System;
using System.IO.Abstractions;

namespace FileStore.Persistance.FileSystem
{
  public class FileSystemPersistance : IPersistance
  {
    public FileSystemPersistance(IFileSystem fileSystem)
    {
      this.fileSystem = fileSystem;
    }

    private IFileSystem fileSystem;

    public void Persist(Guid id, StoreFile fileToStore, int version)
    {
      var fileName = id + "_" + version + "." + fileToStore.Extension;
      using (var stream = fileSystem.File.Create(fileName))
      {
        stream.Write(fileToStore.Data, 0, fileToStore.Data.Length);
      }
    }

    public StoreFile ReadFile(Guid id, int version)
    {
      throw new System.NotImplementedException();
    }

    public bool FileExists(Guid guid)
    {
      throw new System.NotImplementedException();
    }

    public int GetFileLatestVersion(Guid guid)
    {
      throw new System.NotImplementedException();
    }
  }
}
