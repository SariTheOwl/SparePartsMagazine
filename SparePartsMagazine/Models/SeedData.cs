using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SparePartsMagazine.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any movies.
                if (context.Parts.Any())
                {
                    return;   // DB has been seeded
                }

                context.Parts.AddRange(
                    new Part
                    {
                        Name = "Układ hamulcowy",
                        Price = 350M,
                        ModificationDate = DateTimeOffset.Now,
                        InputDate = DateTimeOffset.Now
                    },

                    new Part
                    {
                        Name = "Filtr",
                        Price = 70.50M,
                        ModificationDate = DateTimeOffset.Now,
                        InputDate = DateTimeOffset.Now
                    },

                    new Part
                    {
                        Name = "Karoseria",
                        Price = 3000M,
                        ModificationDate = DateTimeOffset.Now,
                        InputDate = DateTimeOffset.Now
                    },

                    new Part
                    {
                        Name = "Zawieszenia",
                        Price = 1999.99M,
                        ModificationDate = DateTimeOffset.Now,
                        InputDate = DateTimeOffset.Now
                    }
                );
                context.SaveChanges();
            }
        }
    }
}

