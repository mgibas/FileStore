using System;

namespace FileStore.Exceptions
{
    public class FileAtSpecificVersionDoesNotExistException : Exception
    {
        public FileAtSpecificVersionDoesNotExistException(Guid id, int version)
            : base(string.Format("Can not locate file '{0}' version '{1}' in File Store. Please verify file id and version", id, version))
        { }
    }
}
