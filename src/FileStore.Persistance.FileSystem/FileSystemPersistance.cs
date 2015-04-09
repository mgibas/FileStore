using System;
using System.IO.Abstractions;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileStore.Persistance.FileSystem
{
    public class FileSystemPersistance : IPersistance
    {
        public FileSystemPersistance(IFileSystem fileSystem, string storeDirectory)
        {
            this.storeDirectory = storeDirectory;
            this.fileSystem = fileSystem;
        }

        private string storeDirectory;
        private IFileSystem fileSystem;

        public void Persist(Guid id, StoreFile fileToStore, int version)
        {
            var fileName = id + "_" + version + "." + fileToStore.Extension;
            using (var stream = fileSystem.File.Create(storeDirectory + fileName))
            {
                stream.Write(fileToStore.Data, 0, fileToStore.Data.Length);
            }
        }

        public StoreFile ReadFile(Guid id, int version)
        {
            var avaibleFiles = fileSystem.Directory.GetFiles(storeDirectory, string.Format("{0}_{1}.*", id, version));
            if (!avaibleFiles.Any()) return null;

            var data = fileSystem.File.ReadAllBytes(avaibleFiles.First());
            return new StoreFile { Data = data, Extension = avaibleFiles.First().Split('.').Last() };
        }

        public bool FileExists(Guid id)
        {
            return fileSystem.Directory.GetFiles(storeDirectory, string.Format("{0}_*.*", id))
                .Any();
        }

        public int GetFileLatestVersion(Guid id)
        {
            var avaibleFiles = fileSystem.Directory.GetFiles(storeDirectory, string.Format("{0}_*.*", id));
            var latestFileName = avaibleFiles.OrderByDescending(f => f).First();
            var version = Regex.Match(latestFileName, @"[.]*_([0-9]*)\.[.]*").Groups[1].Value;
            return int.Parse(version);
        }
    }
}
