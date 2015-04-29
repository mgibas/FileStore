using System;

namespace FileStore
{
  public interface IVersionedFileStore : IFileStore
  {
    StoreFile OpenFile(Guid id, int version);
  }
}
