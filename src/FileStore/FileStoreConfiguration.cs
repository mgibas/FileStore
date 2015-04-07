using System;
using System.Collections.Generic;
namespace FileStore
{
    public class FileStoreConfiguration
    {
        public FileStoreConfiguration()
        {
            ComponentsForRegistration = new Dictionary<Type, Type>();
            LambdaComponentsForRegistration = new Dictionary<Type, Func<object>>();
        }

        public static FileStoreConfiguration Init()
        {
            var configuration = new FileStoreConfiguration();
            configuration.ComponentsForRegistration.Add(typeof(IFileStore), typeof(VersionedFileStore));
            return configuration;
        }

        public Dictionary<Type, Type> ComponentsForRegistration { get; private set; }
        public Dictionary<Type, Func<object>> LambdaComponentsForRegistration { get; private set; }
    }
}
