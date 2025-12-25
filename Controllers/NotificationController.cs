using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pied_Piper.Data;
using Pied_Piper.DTOs;
using System.Security.Claims;

namespace Pied_Piper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/notification/my-notifications
        [HttpGet("my-notifications")]
        public async Task<IActionResult> GetMyNotifications()
        {
            // Get current user ID from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Invalid token" });

            // Get all notifications for the user, sorted by newest first
            var allNotifications = await _context.Notifications
                .Include(n => n.Event)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            // Separate into new (unseen) and earlier (seen)
            var newNotifications = allNotifications
                .Where(n => !n.IsSeen)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    Type = n.Type,
                    EventId = n.EventId,
                    EventTitle = n.Event?.Title,
                    IsSeen = n.IsSeen,
                    CreatedAt = n.CreatedAt
                })
                .ToList();

            var earlierNotifications = allNotifications
                .Where(n => n.IsSeen)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    Type = n.Type,
                    EventId = n.EventId,
                    EventTitle = n.Event?.Title,
                    IsSeen = n.IsSeen,
                    CreatedAt = n.CreatedAt
                })
                .ToList();

            var response = new NotificationListResponse
            {
                New = newNotifications,
                Earlier = earlierNotifications,
                TotalUnseenCount = newNotifications.Count
            };

            // Mark all unseen notifications as seen
            var unseenNotificationIds = allNotifications
                .Where(n => !n.IsSeen)
                .Select(n => n.Id)
                .ToList();

            if (unseenNotificationIds.Any())
            {
                await _context.Notifications
                    .Where(n => unseenNotificationIds.Contains(n.Id))
                    .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsSeen, true));
            }

            return Ok(response);
        }

        // GET: api/notification/unread-count
        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Invalid token" });

            var count = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsSeen)
                .CountAsync();

            return Ok(new { unreadCount = count });
        }

        // POST: api/notification/{id}/mark-as-read
        [HttpPost("{id}/mark-as-read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Invalid token" });

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if (notification == null)
                return NotFound(new { message = "Notification not found" });

            notification.IsSeen = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Notification marked as read" });
        }

        // DELETE: api/notification/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Invalid token" });

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if (notification == null)
                return NotFound(new { message = "Notification not found" });

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Notification deleted" });
        }
    }
}