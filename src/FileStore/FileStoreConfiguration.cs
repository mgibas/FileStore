using System;
using System.Collections.Generic;
namespace FileStore
{
    public class FileStoreConfiguration
    {
        public FileStoreConfiguration()
        {
            componentsForRegistration = new Dictionary<Type, Type>();
            delegateComponentsForRegistration = new Dictionary<Type, Func<object>>();
        }

        public static FileStoreConfiguration Init()
        {
            var configuration = new FileStoreConfiguration();
            configuration.componentsForRegistration.Add(typeof(IFileStore), typeof(VersionedFileStore));
            return configuration;
        }

        private Dictionary<Type, Type> componentsForRegistration;
        private Dictionary<Type, Func<object>> delegateComponentsForRegistration;

        public void AddComponentForRegistration(Type service, Func<object> implementation)
        {
            delegateComponentsForRegistration.Add(service, implementation);
        }
        public void AddComponentForRegistration(Type service, Type implementation)
        {
            componentsForRegistration.Add(service, implementation);
        }
        public Dictionary<Type, Type> GetComponentsForRegistration()
        {
            return componentsForRegistration;
        }
        public Dictionary<Type, Func<object>> GetDelegateComponentsForRegistration()
        {
            return delegateComponentsForRegistration;
        }
    }
}
