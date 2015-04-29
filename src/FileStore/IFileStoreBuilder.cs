namespace FileStore
{
  public interface IFileStoreBuilder
  {
    IVersionedFileStore BuildVersioned();
    IFileStore BuildUnversioned();
  }
}