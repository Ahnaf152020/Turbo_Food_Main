using Microsoft.EntityFrameworkCore;
using Turbo_Food_Main.Data;
using Turbo_Food_Main.Models;

namespace Turbo_Food_Main.Services
{
    public static class OrderSeedService
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                // Check if orders already exist
                if (context.Orders.Any())
                {
                    return; // DB has been seeded
                }

                // Get some menu items to use in orders
                var cheeseburger = context.MenuItems.FirstOrDefault(m => m.Name.Contains("Cheeseburger"));
                var fries = context.MenuItems.FirstOrDefault(m => m.Name.Contains("Fries"));
                var cola = context.MenuItems.FirstOrDefault(m => m.Name.Contains("Cola"));
                var shake = context.MenuItems.FirstOrDefault(m => m.Name.Contains("Shake"));

                // Get a user to associate with orders
                var user = context.Users.FirstOrDefault();

                if (user == null || cheeseburger == null || fries == null)
                {
                    return; // Can't seed without required data
                }

                var orders = new Order[]
                {
                    new Order
                    {
                        UserID = user.Id,
                        OrderDate = DateTime.UtcNow.AddDays(-2),
                        TotalAmount = 15.97m,
                        Status = "Completed",
                        SpecialInstructions = "Extra ketchup please",
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem { MealID = cheeseburger.ItemId, Quantity = 1, UnitPrice = cheeseburger.Price, TotalPrice = cheeseburger.Price },
                            new OrderItem { MealID = fries.ItemId, Quantity = 2, UnitPrice = fries.Price, TotalPrice = fries.Price * 2 }
                        }
                    },
                    new Order
                    {
                        UserID = user.Id,
                        OrderDate = DateTime.UtcNow.AddDays(-1),
                        TotalAmount = 12.48m,
                        Status = "Preparing",
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem { MealID = cheeseburger.ItemId, Quantity = 1, UnitPrice = cheeseburger.Price, TotalPrice = cheeseburger.Price },
                            new OrderItem { MealID = cola?.ItemId ?? 0, Quantity = 1, UnitPrice = cola?.Price ?? 2.49m, TotalPrice = cola?.Price ?? 2.49m }
                        }
                    },
                    new Order
                    {
                        UserID = user.Id,
                        OrderDate = DateTime.UtcNow,
                        TotalAmount = 8.99m,
                        Status = "Pending",
                        SpecialInstructions = "No onions",
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem { MealID = cheeseburger.ItemId, Quantity = 1, UnitPrice = cheeseburger.Price, TotalPrice = cheeseburger.Price }
                        }
                    }
                };

                context.Orders.AddRange(orders);
                context.SaveChanges();
            }
        }
    }
}