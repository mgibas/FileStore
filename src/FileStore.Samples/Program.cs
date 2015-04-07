using Autofac;
using FileStore.Autofac;
using FileStore.Persistance.EntityFramework;

namespace FileStore.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            FileStoreConfiguration.Init()
                .UseEntityFramework("sample.connectionstring.name")
                .UseAutofac(builder);

            var container = builder.Build();

            var store = container.Resolve<IFileStore>();
        }
    }
}
