using Autofac;
using System;
namespace FileStore.Autofac
{
    public static class FileStoreConfigurationExtensions
    {
        public static void UseAutofac(this FileStoreConfiguration @this, ContainerBuilder builder)
        {
            foreach (var registration in @this.GetComponentsForRegistration())
                builder.RegisterType(registration.Value).As(registration.Key);
            foreach (var registration in @this.GetDelegateComponentsForRegistration())
                builder.Register(ctx => registration.Value.Invoke()).As(registration.Key);
        }
    }
}
