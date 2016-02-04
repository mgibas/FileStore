using System.Data.Entity.Migrations;

namespace FileStore.Persistance.EntityFramework.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<FileDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }
    }
}
