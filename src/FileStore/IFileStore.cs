using System;

namespace FileStore
{
    public interface IFileStore
    {
        void StoreFile(Guid id, StoreFile file);
        StoreFile OpenFile(Guid id);
        StoreFile OpenFile(Guid id, int version);
    }
}
