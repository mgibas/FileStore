using System;

namespace FileStore.Exceptions
{
  public class FileDoesNotExistException : Exception
  {
    public FileDoesNotExistException(Guid id)
      : base(string.Format("Can not locate file '{0}' in File Store. Please verify file id", id))
    { }
  }
}
