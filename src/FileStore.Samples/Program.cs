using System;
using Autofac;
using FileStore.Persistance.EntityFramework;
using FileStore.Persistance.FileSystem;

namespace FileStore.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();

            //var fileStoreBuilder = FileStoreBuilder.Config(cfg =>
            //{
            //  cfg.UseEntityFramework("some.connection.string.name");
            //  cfg.InitializeDatabase();
            //});

            var fileStoreBuilder = FileStoreBuilder.Config(cfg =>
            {
                cfg.UseFileSystem("C:\\fileStore");
            });

            containerBuilder.Register(c => fileStoreBuilder.Build());
            var container = containerBuilder.Build();

            var id = Guid.NewGuid();
            var store = container.Resolve<IFileStore>();
            store.StoreFile(id, new StoreFile { Data = new byte[] { 1, 2, 3 }, Extension = "txt" });
            store.StoreFile(id, new StoreFile { Data = new byte[] { 1, 2, 3, 4, 5, 6 }, Extension = "txt" });

            var newest = store.OpenFile(id);
            var old = store.OpenFile(id, 0);
        }
    }
}
