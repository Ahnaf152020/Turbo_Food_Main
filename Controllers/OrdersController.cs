using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turbo_Food_Main.Data;
using Turbo_Food_Main.Models;
using Turbo_Food_Main.ViewModels;

public class OrdersController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<Users> _userManager;

    public OrdersController(AppDbContext context, UserManager<Users> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Cart()
    {
        return View();
    }

    public IActionResult Payment()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> ConfirmOrder([FromBody] CartRequest request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Json(new { success = false });

        decimal totalAmount = request.Items.Sum(x => x.totalPrice);

        var order = new Order
        {
            UserID = user.Id,
            OrderDate = DateTime.UtcNow,
            TotalAmount = totalAmount,
            Status = "Pending",
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        foreach (var item in request.Items)
        {
            var orderItem = new OrderItem
            {
                OrderID = order.OrderID,
                MealID = item.id,
                Quantity = item.quantity,
                UnitPrice = item.price,
                TotalPrice = item.totalPrice
            };
            _context.OrderItems.Add(orderItem);
        }

        await _context.SaveChangesAsync();
        return Json(new { success = true });
    }
    public async Task<IActionResult> History()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var history = await _context.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.MenuItem)
            .Where(oi => oi.Order.UserID == user.Id)
            .OrderByDescending(oi => oi.Order.OrderDate)
            .Select(oi => new OrderHistoryViewModel
            {
                OrderID = oi.OrderID,
                OrderDate = oi.Order.OrderDate,
                Status = oi.Order.Status,
                MealID = oi.MealID,
                MealName = oi.MenuItem.Name,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                TotalPrice = oi.TotalPrice
            })
            .ToListAsync();

        return View(history);
    }

}

public class CartRequest
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();
}

public class CartItem
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string category { get; set; } = string.Empty;
    public decimal price { get; set; }
    public int quantity { get; set; }
    public decimal totalPrice { get; set; }
}
