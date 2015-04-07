using System;

namespace FileStore
{
    public class VersionedFileStore : IFileStore
    {
        public VersionedFileStore(IPersistance persistance)
        {
            this.persistance = persistance;
        }

        private IPersistance persistance;

        public void StoreFile(Guid id, StoreFile file)
        {
            var version = 0;
            if (persistance.FileExists(id))
                version = persistance.GetFileLatestVersion(id) + 1;

            persistance.Persist(id, file, version);
        }

        public StoreFile OpenFile(Guid id)
        {
            return OpenFile(id, persistance.GetFileLatestVersion(id));
        }

        public StoreFile OpenFile(Guid id, int version)
        {
            var result = persistance.ReadFile(id, version);
            if (result == null)
                throw new FileAtSpecificVersionDoesNotExistException();
            return result;
        }
    }
}
