using System;
using System.Diagnostics.CodeAnalysis;

namespace FileStore
{
  [ExcludeFromCodeCoverage]
  public class FileStoreBuilder : IFileStoreConfigurator, IFileStoreBuilder
  {
    public FileStoreBuilder(Action<IFileStoreConfigurator> configuration)
    {
      this.configuration = configuration;
    }

    private Action<IFileStoreConfigurator> configuration;
    private IPersistance persistance;

    public static IFileStoreBuilder Config(Action<IFileStoreConfigurator> configuration)
    {
      return new FileStoreBuilder(configuration);
    }

    public IFileStoreConfigurator UsePersistance(IPersistance persistance)
    {
      this.persistance = persistance;
      return this;
    }

    public IFileStore BuildUnversioned()
    {
      configuration(this);
      if (persistance == null)
        throw new NullReferenceException(@"Persistance object is not configured. 
                    Please configure FileStore with selected persistance that is located in external package, ie. FileSoter.Persistance.EntityFramework.");

      return new UnversionedFileStore(persistance);
    }

    public IVersionedFileStore BuildVersioned()
    {
      configuration(this);
      if (persistance == null)
        throw new NullReferenceException(@"Persistance object is not configured. 
                    Please configure FileStore with selected persistance that is located in external package, ie. FileSoter.Persistance.EntityFramework.");

      return new VersionedFileStore(persistance);
    }
  }
}
