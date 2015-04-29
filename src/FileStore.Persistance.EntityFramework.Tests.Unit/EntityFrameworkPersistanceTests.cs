using AutoMapper;
using EntityFramework.FakeItEasy;
using FakeItEasy;
using System;
using System.Collections.Generic;
using Xunit;

namespace FileStore.Persistance.EntityFramework.Tests.Unit
{
  public class EntityFrameworkPersistanceTests
  {
    private IFileDbContext context;
    private IMappingEngine mapper;
    private EntityFrameworkPersistance persistance;

    public EntityFrameworkPersistanceTests()
    {
      mapper = A.Fake<IMappingEngine>();
      context = A.Fake<IFileDbContext>();
      A.CallTo(() => context.StoredFiles).Returns(Aef.FakeDbSet<StoredFile>());
      persistance = new EntityFrameworkPersistance(context, mapper);
    }

    [Fact]
    public void Persist_SavesChanges()
    {
      persistance.Persist(Guid.NewGuid(), new StoreFile(), 0);

      A.CallTo(() => context.SaveChanges()).MustHaveHappened();
    }

    [Fact]
    public void Persist_ValidData_AddsNewFileWithProvidedId()
    {
      var fileId = Guid.NewGuid();
      StoredFile storedFile = null;
      A.CallTo(() => context.StoredFiles.Add(A<StoredFile>._)).Invokes((StoredFile added) => { storedFile = added; });

      persistance.Persist(fileId, new StoreFile(), 0);

      Assert.Equal(fileId, storedFile.FileId);
    }

    [Fact]
    public void Persist_ValidData_AddsNewFileWithProvidedVersion()
    {
      StoredFile storedFile = null;
      A.CallTo(() => context.StoredFiles.Add(A<StoredFile>._)).Invokes((StoredFile added) => { storedFile = added; });

      persistance.Persist(Guid.NewGuid(), new StoreFile(), 5);

      Assert.Equal(5, storedFile.Version);
    }

    [Fact]
    public void Persist_ValidData_AddsNewFileWithProvidedStoreFileData()
    {
      var storeFile = new StoreFile { Data = new byte[] { 1, 2, 3 }, Extension = "txt" };
      StoredFile storedFile = null;
      A.CallTo(() => context.StoredFiles.Add(A<StoredFile>._)).Invokes((StoredFile added) => { storedFile = added; });

      persistance.Persist(Guid.NewGuid(), storeFile, 0);

      Assert.Equal(storeFile.Data, storedFile.Data);
      Assert.Equal(storeFile.Extension, storedFile.Extension);
    }

    [Fact]
    public void ReadFile_MultipleFiles_ReturnsFileWithProvidedGuid()
    {
      var currentFiles = new List<StoredFile>
            {
                new StoredFile{ FileId = Guid.NewGuid()},
                new StoredFile{ FileId = Guid.NewGuid()},
                new StoredFile{ FileId = Guid.NewGuid()},
                new StoredFile{ FileId = Guid.NewGuid()},
            };
      var expectedFile = new StoreFile();
      A.CallTo(() => context.StoredFiles).Returns(Aef.FakeDbSet(currentFiles));
      A.CallTo(() => mapper.DynamicMap<StoreFile>(currentFiles[2])).Returns(expectedFile);

      var result = persistance.ReadFile(currentFiles[2].FileId, 0);

      Assert.Equal(expectedFile, result);
    }

    [Fact]
    public void ReadFile_MultipleVersion_ReturnsFileWithProvidedVersion()
    {
      var id = Guid.NewGuid();
      var currentFiles = new List<StoredFile>
            {
                new StoredFile{ FileId = id, Version=0},
                new StoredFile{ FileId = id, Version=1},
                new StoredFile{ FileId = id, Version=2}
            };
      var expectedFile = new StoreFile();
      A.CallTo(() => context.StoredFiles).Returns(Aef.FakeDbSet(currentFiles));
      A.CallTo(() => mapper.DynamicMap<StoreFile>(currentFiles[1])).Returns(expectedFile);

      var result = persistance.ReadFile(id, 1);

      Assert.Equal(expectedFile, result);
    }

    [Fact]
    public void ReadFile_NoFileWithSpecifiedVersion_ReturnsNull()
    {
      var id = Guid.NewGuid();
      var currentFiles = new List<StoredFile>
            {
                new StoredFile{ FileId = id, Version=0},
                new StoredFile{ FileId = id, Version=1},
                new StoredFile{ FileId = id, Version=2}
            };
      A.CallTo(() => context.StoredFiles).Returns(Aef.FakeDbSet(currentFiles));

      var result = persistance.ReadFile(id, 3);

      Assert.Null(result);
    }

    [Fact]
    public void FileExists_Exists_ReturnsTrue()
    {
      var currentFiles = new List<StoredFile>
            {
                new StoredFile{ FileId = Guid.NewGuid()},
                new StoredFile{ FileId = Guid.NewGuid()}
            };
      A.CallTo(() => context.StoredFiles).Returns(Aef.FakeDbSet(currentFiles));

      var result = persistance.FileExists(currentFiles[1].FileId);

      Assert.True(result);
    }

    [Fact]
    public void FileExists_DoesNotExist_ReturnsFalse()
    {
      var currentFiles = new List<StoredFile>
            {
                new StoredFile{ FileId = Guid.NewGuid()},
                new StoredFile{ FileId = Guid.NewGuid()}
            };
      A.CallTo(() => context.StoredFiles).Returns(Aef.FakeDbSet(currentFiles));

      var result = persistance.FileExists(Guid.NewGuid());

      Assert.False(result);
    }

    [Fact]
    public void GetFileLatestVersion_MultipleFilesAndVersion_ReturnsLatestVersionOfSpecifiedFile()
    {
      var id = Guid.NewGuid();
      var currentFiles = new List<StoredFile>
            {
                new StoredFile{ FileId = id, Version = 0},
                new StoredFile{ FileId = id, Version = 1},
                new StoredFile{ FileId = id, Version = 2},
                new StoredFile{ FileId = Guid.NewGuid(), Version = 78},
            };
      A.CallTo(() => context.StoredFiles).Returns(Aef.FakeDbSet(currentFiles));

      var result = persistance.GetFileLatestVersion(id);

      Assert.Equal(2, result);
    }
  }
}
