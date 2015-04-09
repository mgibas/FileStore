using FakeItEasy;
using System.IO.Abstractions;

namespace FileStore.Persistance.FileSystem.Tests.Unit
{
    public class FileSystemPersistanceTests
    {
        private IFileSystem fileSystem;
        private FileSystemPersistance persistance;

        public FileSystemPersistanceTests()
        {
            fileSystem = A.Fake<IFileSystem>();
            persistance = new FileSystemPersistance(fileSystem);
        }
    }
}
