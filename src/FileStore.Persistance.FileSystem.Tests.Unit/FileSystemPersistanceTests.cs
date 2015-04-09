using System;
using System.IO;
using System.Text.RegularExpressions;
using FakeItEasy;
using System.IO.Abstractions;
using Xunit;

namespace FileStore.Persistance.FileSystem.Tests.Unit
{
    public class FileSystemPersistanceTests
    {
        private string configuredDirectory;
        private IFileSystem fileSystem;
        private FileSystemPersistance persistance;

        public FileSystemPersistanceTests()
        {
            configuredDirectory = @"C:\SomeDirectory\";
            fileSystem = A.Fake<IFileSystem>();
            persistance = new FileSystemPersistance(fileSystem, configuredDirectory);
        }

        [Fact]
        public void Persist_ValidData_CreatesNewFileInProvidedDirectory()
        {
            persistance.Persist(Guid.NewGuid(), new StoreFile { Data = new byte[0] }, 0);

            A.CallTo(() => fileSystem.File.Create(A<string>.That.Matches(n => n.StartsWith(configuredDirectory)))).MustHaveHappened();
        }

        [Fact]
        public void Persist_ValidData_CreatesNewFileWithNameContainingProvidedId()
        {
            var fileId = Guid.NewGuid();
            var storeFile = new StoreFile { Data = new byte[0] };
            var expectedMatch = fileId + @"[_][.0-9]*[\.][.]*";

            persistance.Persist(fileId, storeFile, 0);

            A.CallTo(() => fileSystem.File.Create(A<string>.That.Matches(n => Regex.IsMatch(n, expectedMatch)))).MustHaveHappened();
        }

        [Fact]
        public void Persist_ValidData_CreatesNewFileWithNameContainingVersion()
        {
            var version = 44;
            var storeFile = new StoreFile { Data = new byte[0] };
            var expectedMatch = @"[.]*[_]" + version + @"[\.][.]*";

            persistance.Persist(Guid.NewGuid(), storeFile, version);

            A.CallTo(() => fileSystem.File.Create(A<string>.That.Matches(n => Regex.IsMatch(n, expectedMatch)))).MustHaveHappened();
        }

        [Fact]
        public void Persist_ValidData_CreatesNewFileWithNameContainingExtension()
        {
            var storeFile = new StoreFile { Extension = "txt", Data = new byte[0] };
            var expectedMatch = @"[.]*[_][.0-9]*[\.]" + storeFile.Extension;

            persistance.Persist(Guid.NewGuid(), storeFile, 0);

            A.CallTo(() => fileSystem.File.Create(A<string>.That.Matches(n => Regex.IsMatch(n, expectedMatch)))).MustHaveHappened();
        }

        [Fact]
        public void Persist_ValidData_WriteAllDataToCreatedFile()
        {
            var stream = A.Fake<Stream>();
            var storeFile = new StoreFile { Data = new byte[] { 1, 2, 3, 4, 5 } };
            A.CallTo(() => fileSystem.File.Create(A<string>._)).Returns(stream);

            persistance.Persist(Guid.NewGuid(), storeFile, 0);

            A.CallTo(() => stream.Write(storeFile.Data, 0, storeFile.Data.Length)).MustHaveHappened();
        }

        [Fact]
        public void Persist_ValidData_DisposeFileStream()
        {
            var stream = A.Fake<Stream>(cfg => cfg.Implements(typeof(IDisposable)));
            var storeFile = new StoreFile { Data = new byte[0] };
            A.CallTo(() => fileSystem.File.Create(A<string>._)).Returns(stream);

            persistance.Persist(Guid.NewGuid(), storeFile, 0);

            A.CallTo(() => (stream as IDisposable).Dispose()).MustHaveHappened();
        }

        [Fact]
        public void ReadFile_SearchForFileUsingStoreDirectoryFileIdAndVersion()
        {
            var id = Guid.NewGuid();
            var version = 6;
            var expectedSearchFileName = string.Format("{0}_{1}.*", id, version);

            var result = persistance.ReadFile(id, version);

            A.CallTo(() => fileSystem.Directory.GetFiles(configuredDirectory, expectedSearchFileName)).MustHaveHappened();
        }

        [Fact]
        public void ReadFile_FileDoesNotExists_ReturnsNull()
        {
            A.CallTo(() => fileSystem.Directory.GetFiles(A<string>._, A<string>._)).Returns(new string[0]);

            var result = persistance.ReadFile(Guid.NewGuid(), 0);

            Assert.Null(result);
        }

        [Fact]
        public void ReadFile_FileExists_ReturnsFileDataReadFromFoundFile()
        {
            var fileData = new byte[] { 1, 2, 3, 4 };
            A.CallTo(() => fileSystem.Directory.GetFiles(A<string>._, A<string>._))
                .Returns(new[] { "SomeFileToRead" });
            A.CallTo(() => fileSystem.File.ReadAllBytes("SomeFileToRead")).Returns(fileData);

            var result = persistance.ReadFile(Guid.NewGuid(), 0);

            Assert.Equal(fileData, result.Data);
        }

        [Fact]
        public void ReadFile_FileExists_ReturnsFileExtensionReadFromFoundFile()
        {
            A.CallTo(() => fileSystem.Directory.GetFiles(A<string>._, A<string>._))
                .Returns(new[] { "SomeFileToRead.zip" });

            var result = persistance.ReadFile(Guid.NewGuid(), 0);

            Assert.Equal("zip", result.Extension);
        }

        [Fact]
        public void FileExists_Exists_ReturnsTrue()
        {
            var id = Guid.NewGuid();
            A.CallTo(() => fileSystem.Directory.GetFiles(configuredDirectory, string.Format("{0}_*.*", id)))
                .Returns(new[] { "SomeFileToRead.zip" });

            var result = persistance.FileExists(id);

            Assert.True(result);
        }

        [Fact]
        public void FileExists_DoesNotExist_ReturnsFalse()
        {
            var id = Guid.NewGuid();
            A.CallTo(() => fileSystem.Directory.GetFiles(configuredDirectory, string.Format("{0}_*.*", id)))
                .Returns(new string[0]);

            var result = persistance.FileExists(id);

            Assert.False(result);
        }

        [Fact]
        public void GetFileLatestVersion_MultipleVersion_ReturnsLatestVersionOfSpecifiedFile()
        {
            var id = Guid.NewGuid();
            A.CallTo(() => fileSystem.Directory.GetFiles(configuredDirectory, string.Format("{0}_*.*", id)))
               .Returns(new[]{
               "somefile_3.txt",
               "somefile_6.txt",
               "somefile_5.txt",
               "somefile_4.txt",
               });

            var result = persistance.GetFileLatestVersion(id);

            Assert.Equal(6, result);
        }
    }
}
