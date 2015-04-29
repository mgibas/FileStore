using FileStore.Exceptions;
using System;

namespace FileStore
{
  public class UnversionedFileStore : IFileStore
  {
    public UnversionedFileStore(IPersistance persistance)
    {
      this.persistance = persistance;
    }

    private IPersistance persistance;

    public void StoreFile(Guid id, StoreFile file)
    {
      persistance.Persist(id, file, 0);
    }

    public StoreFile OpenFile(Guid id)
    {
      var result = persistance.ReadFile(id, 0);
      if (result == null)
        throw new FileDoesNotExistException(id);
      return result;
    }
  }
}
