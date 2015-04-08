using System;
using Autofac;
using FileStore.Persistance.EntityFramework;

namespace FileStore.Samples
{
  class Program
  {
    static void Main(string[] args)
    {
      var containerBuilder = new ContainerBuilder();
      var fileStoreBuilder = FileStoreBuilder.Config(cfg =>
      {
        cfg.UseEntityFramework("some.connection.string.name");
        cfg.InitializeDatabase();
      });

      containerBuilder.Register(c => fileStoreBuilder.Build());
      var container = containerBuilder.Build();

      var store = container.Resolve<IFileStore>();
      store.StoreFile(Guid.NewGuid(), new StoreFile { Data = new byte[] { 1, 2, 3 }, Extension = "txt" });
    }
  }
}
