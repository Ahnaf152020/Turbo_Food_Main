using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turbo_Food_Main.Data;
using Turbo_Food_Main.Models;

namespace Turbo_Food_Main.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public FeedbacksController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ForMenuItem(int id)
        {
            var feedbacks = await _context.Feedback
                .Where(f => f.MenuItemId == id)
                .OrderByDescending(f => f.DateSubmitted)
                .Include(f => f.User)
                .ToListAsync();

            return PartialView("_FeedbackList", feedbacks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int menuItemId, int rating, string? message)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            if (rating < 1 || rating > 5)
                return BadRequest("Rating must be between 1 and 5");

            var feedback = new Feedback
            {
                MenuItemId = menuItemId,
                UserID = user.Id,
                Rating = rating,
                Message = string.IsNullOrWhiteSpace(message) ? null : message,
                DateSubmitted = DateTime.UtcNow
            };

            _context.Feedback.Add(feedback);
            await _context.SaveChangesAsync();

            // return the updated partial list
            var feedbacks = await _context.Feedback
                .Where(f => f.MenuItemId == menuItemId)
                .OrderByDescending(f => f.DateSubmitted)
                .Include(f => f.User)
                .ToListAsync();

            return PartialView("_FeedbackList", feedbacks);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ByMenuItem(int id)
        {
            var feedbacks = await _context.Feedback
                .Where(f => f.MenuItemId == id)
                .OrderByDescending(f => f.DateSubmitted)
                .Include(f => f.User)
                .ToListAsync();

            var menuItem = await _context.MenuItems.FindAsync(id);
            ViewBag.MenuItemName = menuItem?.Name ?? "Unknown";
            ViewBag.MenuItemId = id;
            return View(feedbacks);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserReview(int menuItemId)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var review = await _context.Feedback
                .FirstOrDefaultAsync(f => f.MenuItemId == menuItemId && f.UserID == user.Id);

            if (review == null)
                return NotFound();

            return Json(new { rating = review.Rating, message = review.Message, id = review.FeedbackID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int feedbackId, int rating, string? message)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var feedback = await _context.Feedback.FirstOrDefaultAsync(f => f.FeedbackID == feedbackId && f.UserID == user.Id);
            if (feedback == null)
                return NotFound();

            feedback.Rating = rating;
            feedback.Message = string.IsNullOrWhiteSpace(message) ? null : message;
            feedback.DateSubmitted = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // return the updated partial list
            var feedbacks = await _context.Feedback
                .Where(f => f.MenuItemId == feedback.MenuItemId)
                .OrderByDescending(f => f.DateSubmitted)
                .Include(f => f.User)
                .ToListAsync();

            return PartialView("_FeedbackList", feedbacks);
        }
    }
}
