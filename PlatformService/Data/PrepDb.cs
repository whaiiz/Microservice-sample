using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    // Class just to setup database in mem

    public static class PrepDb
    {
        // As it is a static class, you can't use DI

        public static void PrePopulation(IApplicationBuilder app, bool isProduction)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
            }
        }

        private static void SeedData(AppDbContext context, bool isProduction)
        {
            if(isProduction)
            {
                Console.WriteLine("--> Attempting to apply migrations");

                try
                {
                    context.Database.Migrate();
                } 
                catch (Exception ex) 
                {
                    Console.WriteLine("--> Could not run Migration", ex);
                }
            }

            if (!context.Platforms.Any())
            {
                Console.WriteLine("---> Seeding Data...");
                context.Platforms.AddRange(
                    new Platform() { Name = "Dot Net", Publisher="Microsoft", Cost="Free" },
                    new Platform() { Name = "SQL Server Express", Publisher="Microsoft", Cost="Free" },
                    new Platform() { Name = "Kubernetes", Publisher="Cloud Native Computing Foundation", Cost="Free" }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("---> We already have data");
            }
        }
    }
}
