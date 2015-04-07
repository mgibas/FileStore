using System;

namespace FileStore
{
    public interface IPersistance
    {
        void Persist(Guid id, StoreFile fileToStore, int version);
        StoreFile ReadFile(Guid id, int version);

        bool FileExists(Guid guid);
        int GetFileLatestVersion(Guid guid);
    }
}
