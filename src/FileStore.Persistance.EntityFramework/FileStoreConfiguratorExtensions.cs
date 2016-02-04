using AutoMapper;
using System;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;

namespace FileStore.Persistance.EntityFramework
{
    [ExcludeFromCodeCoverage]
    public static class FileStoreConfiguratorExtensions
    {
        private static string _connectionString;

        public static IFileStoreConfigurator UseEntityFramework(this IFileStoreConfigurator @this, string connectionStringName)
        {
            if (string.IsNullOrEmpty(connectionStringName))
                throw new ArgumentNullException("connectionStringName", @"Entity Framework in order to with selected database need to have propper connection string.
                Please provide connection string name of connection string located in Your app config or connection string it self");

            _connectionString = connectionStringName;
            @this.UsePersistance(new EntityFrameworkPersistance(new FileDbContext(connectionStringName), Mapper.Engine));
            return @this;
        }

        public static IFileStoreConfigurator InitializeDatabase(this IFileStoreConfigurator @this)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<FileDbContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FileDbContext, Migrations.Configuration>());
            new FileDbContext(_connectionString).Database.Initialize(false);
            return @this;
        }
    }
}
