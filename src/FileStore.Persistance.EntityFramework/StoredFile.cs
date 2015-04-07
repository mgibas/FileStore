using System;

namespace FileStore.Persistance.EntityFramework
{
    public class StoredFile
    {
        public long Id { get; set; }
        public Guid FileId { get; set; }
        public int Version { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
    }
}
