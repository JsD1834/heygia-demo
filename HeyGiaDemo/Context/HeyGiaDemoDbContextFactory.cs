using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace HeyGiaDemo.Context
{
    public class HeyGiaDemoDbContextFactory: IDesignTimeDbContextFactory<HeyGiaDemoDbContext>
    {
        public HeyGiaDemoDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HeyGiaDemoDbContext>();
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlite(connectionString);

            return new HeyGiaDemoDbContext(optionsBuilder.Options);
        }
    }
}
