namespace FileStore
{
  public interface IFileStoreConfigurator
  {
    IFileStoreConfigurator UsePersistance(IPersistance persistance);
  }
}