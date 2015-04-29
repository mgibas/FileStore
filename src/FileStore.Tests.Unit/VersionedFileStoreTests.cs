using FakeItEasy;
using FileStore.Exceptions;
using System;
using Xunit;

namespace FileStore.Tests.Unit
{
    public class VersionedFileStoreTests
    {
        private IPersistance persistance;
        private VersionedFileStore storage;

        public VersionedFileStoreTests()
        {
            persistance = A.Fake<IPersistance>();
            storage = new VersionedFileStore(persistance);
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
        [InlineData(55, 56)]
        [InlineData(0, 1)]
        public void StoreFile_PreviousVersionsExists_StoresFileWithIncreasedVersion(int currentVersion, int expectedVersion)
        {
            var guid = Guid.NewGuid();
            var file = new StoreFile();
            A.CallTo(() => persistance.FileExists(guid)).Returns(true);
            A.CallTo(() => persistance.GetFileLatestVersion(guid)).Returns(currentVersion);

            storage.StoreFile(guid, file);

            A.CallTo(() => persistance.Persist(guid, file, expectedVersion)).MustHaveHappened();
        }

        [Fact]
        public void OpenFile_NoVersionSpecified_ReturnsFileLatestVersion()
        {
            var guid = Guid.NewGuid();
            var latestVersion = 56;
            var latestFile = new StoreFile();
            A.CallTo(() => persistance.GetFileLatestVersion(guid)).Returns(latestVersion);
            A.CallTo(() => persistance.ReadFile(guid, latestVersion)).Returns(latestFile);

            var result = storage.OpenFile(guid);

            Assert.Same(latestFile, result);
        }

        [Fact]
        public void OpenFile_VersionSpecified_ReturnsFileAtSpecifiedVersion()
        {
            var guid = Guid.NewGuid();
            var version = 12;
            var specificFile = new StoreFile();
            A.CallTo(() => persistance.ReadFile(guid, version)).Returns(specificFile);

            var result = storage.OpenFile(guid, version);

            Assert.Same(specificFile, result);
        }

        [Fact]
        public void OpenFile_NoFileAtSpecifiedVersion_ThrowsException()
        {
            var guid = Guid.NewGuid();
            A.CallTo(() => persistance.ReadFile(guid, 4)).Returns(null);

            Assert.Throws<FileAtSpecificVersionDoesNotExistException>(() => storage.OpenFile(guid, 4));
        }
    }
}
