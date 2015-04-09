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
            throw new System.NotImplementedException();
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
