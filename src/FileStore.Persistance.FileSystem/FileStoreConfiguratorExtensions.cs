using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FileStore.Persistance.FileSystem
{
    [ExcludeFromCodeCoverage]
    public static class FileStoreConfiguratorExtensions
    {
        public static IFileStoreConfigurator UseFileSystem(this IFileStoreConfigurator @this, string storeDirectory)
        {
            if (string.IsNullOrEmpty(storeDirectory))
                throw new ArgumentNullException("storeDirectory", @"In order to save files using file system Please provide valid directory path.");
            if (!storeDirectory.EndsWith("\\"))
                storeDirectory += "\\";

            if (!Directory.Exists(storeDirectory))
                Directory.CreateDirectory(storeDirectory);

            @this.UsePersistance(new FileSystemPersistance(new System.IO.Abstractions.FileSystem(), storeDirectory));
            return @this;
        }
    }
}
