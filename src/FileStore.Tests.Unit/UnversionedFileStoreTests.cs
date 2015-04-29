using FakeItEasy;
using FileStore.Exceptions;
using System;
using Xunit;

namespace FileStore.Tests.Unit
{
  public class UnversionedFileStoreTests
  {
    private IPersistance persistance;
    private UnversionedFileStore storage;

    public UnversionedFileStoreTests()
    {
      persistance = A.Fake<IPersistance>();
      storage = new UnversionedFileStore(persistance);
    }

    [Fact]
    public void StoreFile_FirstTimeStoringThisFile_StoresProvidedFileAtVersion0()
    {
      var guid = Guid.NewGuid();
      var file = new StoreFile();
      A.CallTo(() => persistance.FileExists(guid)).Returns(false);

      storage.StoreFile(guid, file);

      A.CallTo(() => persistance.Persist(guid, file, 0)).MustHaveHappened();
    }

    [Theory]
    [InlineData(55)]
    [InlineData(10)]
    public void StoreFile_FirstVersionAlreadyExists_AlwaysStoresProvidedFileAtVersion0(int currentVersion)
    {
      var guid = Guid.NewGuid();
      var file = new StoreFile();
      A.CallTo(() => persistance.FileExists(guid)).Returns(true);

      storage.StoreFile(guid, file);

      A.CallTo(() => persistance.Persist(guid, file, 0)).MustHaveHappened();
    }

    [Fact]
    public void OpenFile_NoVersionSpecified_ReturnsFile0Version()
    {
      var guid = Guid.NewGuid();
      var latestFile = new StoreFile();
      A.CallTo(() => persistance.ReadFile(guid, 0)).Returns(latestFile);

      var result = storage.OpenFile(guid);

      Assert.Same(latestFile, result);
    }

    [Fact]
    public void OpenFile_NoFile_ThrowsException()
    {
      var guid = Guid.NewGuid();
      A.CallTo(() => persistance.ReadFile(guid, A<int>._)).Returns(null);

      Assert.Throws<FileDoesNotExistException>(() => storage.OpenFile(guid));
    }
  }
}
