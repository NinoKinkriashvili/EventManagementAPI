using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pied_Piper.Data;
using Pied_Piper.DTOs;
using Pied_Piper.Models;

namespace Pied_Piper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AnalyticsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Helper method to get event status
        private static string GetEventStatus(Event e)
        {
            var confirmedCount = e.Registrations.Count(r => r.Status.Name == "Confirmed");

            if (confirmedCount >= e.MaxCapacity)
            {
                if (e.WaitlistEnabled)
                {
                    return "Waitlisted";
                }
                return "Full";
            }

            return "Available";
        }

        // GET: api/analytics/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? categoryId,
            [FromQuery] string? location,
            [FromQuery] string? eventStatus)
        {
            var start = startDate ?? DateTime.UtcNow.AddDays(-30);
            var end = endDate ?? DateTime.UtcNow;

            var analytics = new AnalyticsDto
            {
                KPIs = await GetKPIs(start, end, categoryId, location, eventStatus),
                EventStatusDistribution = await GetEventStatusDistribution(start, end, categoryId, location),
                RegistrationTrend = await GetRegistrationTrend(start, end),
                CategoryDistribution = await GetCategoryDistribution(start, end),
                TopEvents = await GetTopEvents(start, end, categoryId, eventStatus),
                DepartmentParticipation = await GetDepartmentParticipation(start, end)
            };

            return Ok(analytics);
        }

        // ============================================
        // PRIVATE HELPER METHODS
        // ============================================

        private async Task<KeyPerformanceIndicators> GetKPIs(
            DateTime startDate,
            DateTime endDate,
            int? categoryId,
            string? location,
            string? eventStatus)
        {
            var periodLength = endDate - startDate;

            var previousStartDate = startDate.Add(-periodLength);
            var previousEndDate = startDate;

            var eventQuery = _context.Events
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.Status)
                .Where(e => e.IsActive && e.StartDateTime >= startDate && e.StartDateTime <= endDate);

            if (categoryId.HasValue)
                eventQuery = eventQuery.Where(e => e.CategoryId == categoryId.Value);

            if (!string.IsNullOrEmpty(location))
                eventQuery = eventQuery.Where(e => e.Location.Contains(location));

            var events = await eventQuery.ToListAsync();

            // Filter by event status if provided
            if (!string.IsNullOrEmpty(eventStatus))
            {
                events = events.Where(e => GetEventStatus(e) == eventStatus).ToList();
            }

            var eventIds = events.Select(e => e.Id).ToList();

            var registrations = await _context.Registrations
                .Include(r => r.Status)
                .Where(r => eventIds.Contains(r.EventId) &&
                            r.RegisteredAt >= startDate &&
                            r.RegisteredAt <= endDate)
                .ToListAsync();

            var totalRegistrations = registrations.Count;
            var activeParticipants = registrations
                .Where(r => r.Status.Name == "Confirmed" || r.Status.Name == "Waitlisted")
                .Select(r => r.UserId)
                .Distinct()
                .Count();

            var cancelledRegistrations = registrations.Count(r => r.Status.Name == "Cancelled");
            var cancellationRate = totalRegistrations > 0
                ? Math.Round((decimal)cancelledRegistrations / totalRegistrations * 100, 2)
                : 0;

            // ============================================
            // CALCULATE PREVIOUS PERIOD DATA
            // ============================================
            var previousEventQuery = _context.Events
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.Status)
                .Where(e => e.IsActive && e.StartDateTime >= previousStartDate && e.StartDateTime < previousEndDate);

            if (categoryId.HasValue)
                previousEventQuery = previousEventQuery.Where(e => e.CategoryId == categoryId.Value);

            if (!string.IsNullOrEmpty(location))
                previousEventQuery = previousEventQuery.Where(e => e.Location.Contains(location));

            var previousEvents = await previousEventQuery.ToListAsync();

            // Filter by event status if provided
            if (!string.IsNullOrEmpty(eventStatus))
            {
                previousEvents = previousEvents.Where(e => GetEventStatus(e) == eventStatus).ToList();
            }

            var previousEventIds = previousEvents.Select(e => e.Id).ToList();

            var previousRegistrations = await _context.Registrations
                .Include(r => r.Status)
                .Where(r => previousEventIds.Contains(r.EventId) &&
                            r.RegisteredAt >= previousStartDate &&
                            r.RegisteredAt < previousEndDate)
                .ToListAsync();

            var previousTotalRegistrations = previousRegistrations.Count;
            var previousActiveParticipants = previousRegistrations
                .Where(r => r.Status.Name == "Confirmed" || r.Status.Name == "Waitlisted")
                .Select(r => r.UserId)
                .Distinct()
                .Count();

            var previousCancelledRegistrations = previousRegistrations.Count(r => r.Status.Name == "Cancelled");
            var previousCancellationRate = previousTotalRegistrations > 0
                ? Math.Round((decimal)previousCancelledRegistrations / previousTotalRegistrations * 100, 2)
                : 0;

            var previousTotalEvents = previousEvents.Count;

            // ============================================
            // CALCULATE PERCENTAGE CHANGES
            // ============================================

            var registrationsChange = CalculatePercentageChange(previousTotalRegistrations, totalRegistrations);
            var participantsChange = CalculatePercentageChange(previousActiveParticipants, activeParticipants);
            var eventsChange = CalculatePercentageChange(previousTotalEvents, events.Count);
            var cancellationRateChange = cancellationRate - previousCancellationRate;

            var now = DateTime.UtcNow;

            return new KeyPerformanceIndicators
            {
                TotalRegistrations = totalRegistrations,
                RegistrationsChangePercentage = registrationsChange,

                ActiveParticipants = activeParticipants,
                ActiveParticipantsChangePercentage = participantsChange,

                CancellationRate = cancellationRate,
                CancellationRateChange = Math.Round(cancellationRateChange, 2),

                TotalEvents = events.Count,
                TotalEventsChangePercentage = eventsChange,

                UpcomingEvents = events.Count(e => e.StartDateTime > now),
                PastEvents = events.Count(e => e.EndDateTime < now)
            };
        }

        private decimal CalculatePercentageChange(int oldValue, int newValue)
        {
            if (oldValue == 0)
            {
                return newValue > 0 ? 100 : 0;
            }

            var change = ((decimal)(newValue - oldValue) / oldValue) * 100;
            return Math.Round(change, 2);
        }


        private async Task<EventStatusDistribution> GetEventStatusDistribution(
            DateTime startDate,
            DateTime endDate,
            int? categoryId,
            string? location)
        {
            var query = _context.Events
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.Status)
                .Where(e => e.IsActive && e.StartDateTime >= startDate && e.StartDateTime <= endDate);

            if (categoryId.HasValue)
                query = query.Where(e => e.CategoryId == categoryId.Value);

            if (!string.IsNullOrEmpty(location))
                query = query.Where(e => e.Location.Contains(location));

            var events = await query.ToListAsync();

            var availableCount = events.Count(e => GetEventStatus(e) == "Available");
            var fullCount = events.Count(e => GetEventStatus(e) == "Full");
            var waitlistedCount = events.Count(e => GetEventStatus(e) == "Waitlisted");
            var totalEvents = events.Count;

            return new EventStatusDistribution
            {
                AvailableEvents = availableCount,
                FullEvents = fullCount,
                WaitlistedEvents = waitlistedCount,
                AvailablePercentage = totalEvents > 0 ? Math.Round((decimal)availableCount / totalEvents * 100, 2) : 0,
                FullPercentage = totalEvents > 0 ? Math.Round((decimal)fullCount / totalEvents * 100, 2) : 0,
                WaitlistedPercentage = totalEvents > 0 ? Math.Round((decimal)waitlistedCount / totalEvents * 100, 2) : 0
            };
        }

        private async Task<List<MonthlyRegistrationTrend>> GetRegistrationTrend(
            DateTime startDate,
            DateTime endDate)
        {
            var registrations = await _context.Registrations
                .Where(r => r.RegisteredAt >= startDate && r.RegisteredAt <= endDate)
                .GroupBy(r => new { r.RegisteredAt.Year, r.RegisteredAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            var trend = new List<MonthlyRegistrationTrend>();

            for (int i = 0; i < registrations.Count; i++)
            {
                var current = registrations[i];
                var previous = i > 0 ? registrations[i - 1].Count : 0;

                trend.Add(new MonthlyRegistrationTrend
                {
                    Month = new DateTime(current.Year, current.Month, 1).ToString("MMM"),
                    Year = current.Year,
                    CurrentMonthRegistrations = current.Count,
                    PreviousMonthRegistrations = previous
                });
            }

            return trend;
        }

        private async Task<List<CategoryDistribution>> GetCategoryDistribution(
            DateTime startDate,
            DateTime endDate)
        {
            var events = await _context.Events
                .Include(e => e.Category)
                .Where(e => e.IsActive && e.StartDateTime >= startDate && e.StartDateTime <= endDate)
                .GroupBy(e => e.Category.Title)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            var totalEvents = events.Sum(e => e.Count);

            return events.Select(e => new CategoryDistribution
            {
                CategoryName = e.CategoryName,
                EventCount = e.Count,
                Percentage = totalEvents > 0 ? Math.Round((decimal)e.Count / totalEvents * 100, 2) : 0
            }).ToList();
        }

        private async Task<List<TopEventByDemand>> GetTopEvents(
    DateTime startDate,
    DateTime endDate,
    int? categoryId,
    string? eventStatus,
    int limit = 10)
        {
            var query = _context.Events
                .Include(e => e.Category)
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.Status)
                .Where(e => e.IsActive && e.StartDateTime >= startDate && e.StartDateTime <= endDate);

            if (categoryId.HasValue)
                query = query.Where(e => e.CategoryId == categoryId.Value);

            var events = await query.ToListAsync();

            // Filter by event status if provided
            if (!string.IsNullOrEmpty(eventStatus))
            {
                events = events.Where(e => GetEventStatus(e) == eventStatus).ToList();
            }

            var topEvents = events
                .Select(e => new
                {
                    Event = e,
                    ConfirmedCount = e.Registrations.Count(r => r.Status.Name == "Confirmed"),
                    WaitlistedCount = e.Registrations.Count(r => r.Status.Name == "Waitlisted"),
                    EventStatus = GetEventStatus(e)
                })
                .OrderByDescending(x => x.ConfirmedCount)
                .Take(limit)
                .Select((x, index) => new TopEventByDemand
                {
                    Rank = index + 1,
                    EventId = x.Event.Id,
                    EventName = x.Event.Title,
                    CategoryName = x.Event.Category.Title,
                    Date = x.Event.StartDateTime,
                    CurrentCapacity = x.ConfirmedCount,
                    MaxCapacity = x.Event.MaxCapacity,
                    AvailableSlots = x.Event.MaxCapacity - x.ConfirmedCount,
                    WaitlistedCount = x.WaitlistedCount,
                    EventStatus = x.EventStatus,
                    UtilizationRate = x.Event.MaxCapacity > 0
                        ? Math.Round((decimal)x.ConfirmedCount / x.Event.MaxCapacity * 100, 2)
                        : 0
                })
                .ToList();

            return topEvents;
        }

        private async Task<List<DepartmentParticipation>> GetDepartmentParticipation(
            DateTime startDate,
            DateTime endDate)
        {
            var departments = await _context.Departments
                .Include(d => d.Users)
                    .ThenInclude(u => u.Registrations.Where(r => r.RegisteredAt >= startDate && r.RegisteredAt <= endDate))
                        .ThenInclude(r => r.Status)
                .Where(d => d.IsActive)
                .ToListAsync();

            var result = departments.Select(d =>
            {
                var totalEmployees = d.Users.Count(u => u.IsActive);
                var activeParticipants = d.Users
                    .Where(u => u.IsActive && u.Registrations.Any(r =>
                        r.Status.Name == "Confirmed" || r.Status.Name == "Waitlisted"))
                    .Count();

                var totalRegistrations = d.Users
                    .Where(u => u.IsActive)
                    .SelectMany(u => u.Registrations)
                    .Count(r => r.Status.Name == "Confirmed" || r.Status.Name == "Waitlisted");

                var participationRate = totalEmployees > 0
                    ? Math.Round((decimal)activeParticipants / totalEmployees * 100, 2)
                    : 0;

                var avgEventsPerEmployee = totalEmployees > 0
                    ? Math.Round((decimal)totalRegistrations / totalEmployees, 1)
                    : 0;

                var trend = participationRate >= 50 ? "↑+15%" : "→+1%";

                return new DepartmentParticipation
                {
                    DepartmentName = d.Name,
                    TotalEmployees = totalEmployees,
                    ActiveParticipants = activeParticipants,
                    ParticipationRate = participationRate,
                    TotalRegistrations = totalRegistrations,
                    AvgEventsPerEmployee = avgEventsPerEmployee,
                    Trend = trend
                };
            })
            .OrderByDescending(d => d.ParticipationRate)
            .ToList();

            return result;
        }
    }
}