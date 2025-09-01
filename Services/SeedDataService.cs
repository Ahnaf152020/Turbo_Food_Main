using Microsoft.EntityFrameworkCore;
using Turbo_Food_Main.Data;
using Turbo_Food_Main.Models;

namespace Turbo_Food_Main.Services
{
    public static class SeedDataService
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                // Check if database exists and has any menu items
                context.Database.EnsureCreated();

                // Look for any menu items
                if (context.MenuItems.Any())
                {
                    return; // DB has been seeded
                }

                // Add sample menu items
                var menuItems = new MenuItem[]
                {
                    new MenuItem
                    {
                        Name = "Classic Cheeseburger",
                        Description = "Juicy beef patty with cheese, lettuce, tomato, and special sauce",
                        Category = "Burgers",
                        Availability = true,
                        Price = 8.99m,
                        CreatedAt = DateTime.UtcNow
                    },
                    new MenuItem
                    {
                        Name = "Bacon Deluxe",
                        Description = "Beef patty with crispy bacon, cheddar cheese, and BBQ sauce",
                        Category = "Burgers",
                        Availability = true,
                        Price = 10.99m,
                        CreatedAt = DateTime.UtcNow
                    },
                    new MenuItem
                    {
                        Name = "Veggie Burger",
                        Description = "Plant-based patty with fresh vegetables and vegan mayo",
                        Category = "Burgers",
                        Availability = true,
                        Price = 9.49m,
                        CreatedAt = DateTime.UtcNow
                    },
                    new MenuItem
                    {
                        Name = "French Fries",
                        Description = "Crispy golden fries with sea salt",
                        Category = "Sides",
                        Availability = true,
                        Price = 3.99m,
                        CreatedAt = DateTime.UtcNow
                    },
                    new MenuItem
                    {
                        Name = "Onion Rings",
                        Description = "Crispy battered onion rings with dipping sauce",
                        Category = "Sides",
                        Availability = true,
                        Price = 4.99m,
                        CreatedAt = DateTime.UtcNow
                    },
                    new MenuItem
                    {
                        Name = "Cola",
                        Description = "Refreshing cola drink",
                        Category = "Drinks",
                        Availability = true,
                        Price = 2.49m,
                        CreatedAt = DateTime.UtcNow
                    },
                    new MenuItem
                    {
                        Name = "Chocolate Shake",
                        Description = "Rich chocolate milkshake with whipped cream",
                        Category = "Drinks",
                        Availability = true,
                        Price = 4.99m,
                        CreatedAt = DateTime.UtcNow
                    },
                    new MenuItem
                    {
                        Name = "Seasonal Salad",
                        Description = "Fresh mixed greens with seasonal vegetables",
                        Category = "Salads",
                        Availability = false, // Temporarily unavailable
                        Price = 7.99m,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.MenuItems.AddRange(menuItems);
                context.SaveChanges();
            }
        }
    }
}