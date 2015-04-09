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
    private IFileSystem fileSystem;
    private FileSystemPersistance persistance;

    public FileSystemPersistanceTests()
    {
      fileSystem = A.Fake<IFileSystem>();
      persistance = new FileSystemPersistance(fileSystem);
    }

    //[Fact]
    //public void Persist_SavesChanges()
    //{
    //  persistance.Persist(Guid.NewGuid(), new StoreFile(), 0);

    //  A.CallTo(() => context.SaveChanges()).MustHaveHappened();
    //}

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

    //[Fact]
    //public void ReadFile_MultipleFiles_ReturnsFileWithProvidedGuid()
    //{
    //  var currentFiles = new List<StoredFile>
    //        {
    //            new StoredFile{ FileId = Guid.NewGuid()},
    //            new StoredFile{ FileId = Guid.NewGuid()},
    //            new StoredFile{ FileId = Guid.NewGuid()},
    //            new StoredFile{ FileId = Guid.NewGuid()},
    //        };
    //  var expectedFile = new StoreFile();
    //  A.CallTo(() => context.StoredFiles).Returns(Aef.FakeDbSet(currentFiles));
    //  A.CallTo(() => mapper.DynamicMap<StoreFile>(currentFiles[2])).Returns(expectedFile);

    //  var result = persistance.ReadFile(currentFiles[2].FileId, 0);

    //  Assert.Equal(expectedFile, result);
    //}

    //[Fact]
    //public void ReadFile_MultipleVersion_ReturnsFileWithProvidedVersion()
    //{
    //  var id = Guid.NewGuid();
    //  var currentFiles = new List<StoredFile>
    //        {
    //            new StoredFile{ FileId = id, Version=0},
    //            new StoredFile{ FileId = id, Version=1},
    //            new StoredFile{ FileId = id, Version=2}
    //        };
    //  var expectedFile = new StoreFile();
    //  A.CallTo(() => context.StoredFiles).Returns(Aef.FakeDbSet(currentFiles));
    //  A.CallTo(() => mapper.DynamicMap<StoreFile>(currentFiles[1])).Returns(expectedFile);

    //  var result = persistance.ReadFile(id, 1);

    //  Assert.Equal(expectedFile, result);
    //}

    //[Fact]
    //public void ReadFile_NoFileWithSpecifiedVersion_ReturnsNull()
    //{
    //  var id = Guid.NewGuid();
    //  var currentFiles = new List<StoredFile>
    //        {
    //            new StoredFile{ FileId = id, Version=0},
    //            new StoredFile{ FileId = id, Version=1},
    //            new StoredFile{ FileId = id, Version=2}
    //        };
    //  A.CallTo(() => context.StoredFiles).Returns(Aef.FakeDbSet(currentFiles));

    //  var result = persistance.ReadFile(id, 3);

    //  Assert.Null(result);
    //}

    //[Fact]
    //public void FileExists_Exists_ReturnsTrue()
    //{
    //  var currentFiles = new List<StoredFile>
    //        {
    //            new StoredFile{ FileId = Guid.NewGuid()},
    //            new StoredFile{ FileId = Guid.NewGuid()}
    //        };
    //  A.CallTo(() => context.StoredFiles).Returns(Aef.FakeDbSet(currentFiles));

    //  var result = persistance.FileExists(currentFiles[1].FileId);

    //  Assert.True(result);
    //}

    //[Fact]
    //public void FileExists_DoesNotExist_ReturnsFalse()
    //{
    //  var currentFiles = new List<StoredFile>
    //        {
    //            new StoredFile{ FileId = Guid.NewGuid()},
    //            new StoredFile{ FileId = Guid.NewGuid()}
    //        };
    //  A.CallTo(() => context.StoredFiles).Returns(Aef.FakeDbSet(currentFiles));

    //  var result = persistance.FileExists(Guid.NewGuid());

    //  Assert.False(result);
    //}

    //[Fact]
    //public void GetFileLatestVersion_MultipleFilesAndVersion_ReturnsLatestVersionOfSpecifiedFile()
    //{
    //  var id = Guid.NewGuid();
    //  var currentFiles = new List<StoredFile>
    //        {
    //            new StoredFile{ FileId = id, Version = 0},
    //            new StoredFile{ FileId = id, Version = 1},
    //            new StoredFile{ FileId = id, Version = 2},
    //            new StoredFile{ FileId = Guid.NewGuid(), Version = 78},
    //        };
    //  A.CallTo(() => context.StoredFiles).Returns(Aef.FakeDbSet(currentFiles));

    //  var result = persistance.GetFileLatestVersion(id);

    //  Assert.Equal(2, result);
    //}
  }
}
