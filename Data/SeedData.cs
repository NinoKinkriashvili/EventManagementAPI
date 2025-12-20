using Microsoft.EntityFrameworkCore;
using Pied_Piper.Models;

namespace Pied_Piper.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // Check if database has data
            if (context.EventTypes.Any())
            {
                return; // Database already seeded
            }

            // Seed Event Types
            var eventTypes = new EventType[]
            {
                new EventType { Name = "Team Building", Description = "Activities to strengthen team bonds" },
                new EventType { Name = "Sports", Description = "Physical activities and competitions" },
                new EventType { Name = "Workshop", Description = "Learning and skill development sessions" },
                new EventType { Name = "Happy Friday", Description = "End of week celebrations" },
                new EventType { Name = "Cultural", Description = "Cultural events and celebrations" },
                new EventType { Name = "Training", Description = "Professional training sessions" },
                new EventType { Name = "Social", Description = "Social gathering events" }
            };
            context.EventTypes.AddRange(eventTypes);
            context.SaveChanges();

            // Seed Registration Statuses
            var statuses = new RegistrationStatus[]
            {
                new RegistrationStatus { Name = "Confirmed", Description = "Registration is confirmed" },
                new RegistrationStatus { Name = "Waitlisted", Description = "On waiting list due to capacity" },
                new RegistrationStatus { Name = "Cancelled", Description = "Registration was cancelled" }
            };
            context.RegistrationStatuses.AddRange(statuses);
            context.SaveChanges();

            // Seed Roles
            var roles = new Role[]
            {
                new Role { Name = "Employee", Description = "Regular employee" },
                new Role { Name = "Organizer", Description = "Can create and manage events" },
                new Role { Name = "Admin", Description = "Full system access" }
            };
            context.Roles.AddRange(roles);
            context.SaveChanges();

            // Seed Tags
            var tags = new Tag[]
            {
                new Tag { Name = "outdoor", Category = "location" },
                new Tag { Name = "indoor", Category = "location" },
                new Tag { Name = "free-food", Category = "perks" },
                new Tag { Name = "remote-friendly", Category = "accessibility" },
                new Tag { Name = "family-friendly", Category = "accessibility" },
                new Tag { Name = "networking", Category = "purpose" },
                new Tag { Name = "learning", Category = "purpose" },
                new Tag { Name = "wellness", Category = "purpose" }
            };
            context.Tags.AddRange(tags);
            context.SaveChanges();

            // Seed Users
            var users = new User[]
            {
                new User
                {
                    Email = "admin@company.com",
                    FullName = "Admin User",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    RoleId = roles.First(r => r.Name == "Admin").Id
                },
                new User
                {
                    Email = "organizer@company.com",
                    FullName = "Event Organizer",
                    Password = BCrypt.Net.BCrypt.HashPassword("Organizer123!"),
                    RoleId = roles.First(r => r.Name == "Organizer").Id
                },
                new User
                {
                    Email = "john.doe@company.com",
                    FullName = "John Doe",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    RoleId = roles.First(r => r.Name == "Employee").Id
                },
                new User
                {
                    Email = "jane.smith@company.com",
                    FullName = "Jane Smith",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    RoleId = roles.First(r => r.Name == "Employee").Id
                }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            // Seed Events
            var organizerId = users.First(u => u.Email == "organizer@company.com").Id;
            var events = new Event[]
            {
                new Event
                {
                    Title = "Annual Team Building Retreat",
                    Description = "Join us for a fun-filled day of team activities and bonding!",
                    EventTypeId = eventTypes.First(et => et.Name == "Team Building").Id,
                    StartDateTime = DateTime.UtcNow.AddDays(30),
                    EndDateTime = DateTime.UtcNow.AddDays(30).AddHours(8),
                    Location = "Mountain Resort, Lake Tahoe",
                    Capacity = 50,
                    ImageUrl = "https://example.com/team-building.jpg",
                    CreatedById = organizerId
                },
                new Event
                {
                    Title = "Company Soccer Tournament",
                    Description = "Show off your soccer skills in our annual tournament!",
                    EventTypeId = eventTypes.First(et => et.Name == "Sports").Id,
                    StartDateTime = DateTime.UtcNow.AddDays(15),
                    EndDateTime = DateTime.UtcNow.AddDays(15).AddHours(4),
                    Location = "Sports Complex, Building A",
                    Capacity = 30,
                    ImageUrl = "https://example.com/soccer.jpg",
                    CreatedById = organizerId
                },
                new Event
                {
                    Title = "Leadership Skills Workshop",
                    Description = "Enhance your leadership abilities with expert trainers",
                    EventTypeId = eventTypes.First(et => et.Name == "Workshop").Id,
                    StartDateTime = DateTime.UtcNow.AddDays(7),
                    EndDateTime = DateTime.UtcNow.AddDays(7).AddHours(3),
                    Location = "Training Room 301, Main Office",
                    Capacity = 25,
                    ImageUrl = "https://example.com/workshop.jpg",
                    CreatedById = organizerId
                },
                new Event
                {
                    Title = "Happy Friday Pizza Party",
                    Description = "End the week with pizza, games, and good vibes!",
                    EventTypeId = eventTypes.First(et => et.Name == "Happy Friday").Id,
                    StartDateTime = DateTime.UtcNow.AddDays(5),
                    EndDateTime = DateTime.UtcNow.AddDays(5).AddHours(2),
                    Location = "Cafeteria, Main Office",
                    Capacity = 100,
                    ImageUrl = "https://example.com/pizza-party.jpg",
                    CreatedById = organizerId
                },
                new Event
                {
                    Title = "Diwali Celebration",
                    Description = "Celebrate the festival of lights with cultural performances",
                    EventTypeId = eventTypes.First(et => et.Name == "Cultural").Id,
                    StartDateTime = DateTime.UtcNow.AddDays(45),
                    EndDateTime = DateTime.UtcNow.AddDays(45).AddHours(3),
                    Location = "Grand Hall, Main Office",
                    Capacity = 150,
                    ImageUrl = "https://example.com/diwali.jpg",
                    CreatedById = organizerId
                },
                new Event
                {
                    Title = "Agile Methodology Training",
                    Description = "Learn Agile practices for better project management",
                    EventTypeId = eventTypes.First(et => et.Name == "Training").Id,
                    StartDateTime = DateTime.UtcNow.AddDays(20),
                    EndDateTime = DateTime.UtcNow.AddDays(20).AddHours(6),
                    Location = "Conference Room B, Main Office",
                    Capacity = 20,
                    ImageUrl = "https://example.com/agile.jpg",
                    CreatedById = organizerId
                },
                new Event
                {
                    Title = "Summer BBQ Bash",
                    Description = "Enjoy great food and company at our summer BBQ",
                    EventTypeId = eventTypes.First(et => et.Name == "Social").Id,
                    StartDateTime = DateTime.UtcNow.AddDays(60),
                    EndDateTime = DateTime.UtcNow.AddDays(60).AddHours(4),
                    Location = "Company Garden, Outdoor Area",
                    Capacity = 80,
                    ImageUrl = "https://example.com/bbq.jpg",
                    CreatedById = organizerId
                },
                new Event
                {
                    Title = "Yoga and Wellness Session",
                    Description = "Relax and rejuvenate with guided yoga and meditation",
                    EventTypeId = eventTypes.First(et => et.Name == "Workshop").Id,
                    StartDateTime = DateTime.UtcNow.AddDays(3),
                    EndDateTime = DateTime.UtcNow.AddDays(3).AddHours(1.5),
                    Location = "Wellness Center, Building C",
                    Capacity = 15,
                    ImageUrl = "https://example.com/yoga.jpg",
                    CreatedById = organizerId
                }
            };
            context.Events.AddRange(events);
            context.SaveChanges();

            // Seed Event Tags
            var eventTags = new List<EventTag>
            {
                new EventTag { EventId = events[0].Id, TagId = tags.First(t => t.Name == "outdoor").Id },
                new EventTag { EventId = events[0].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[1].Id, TagId = tags.First(t => t.Name == "outdoor").Id },
                new EventTag { EventId = events[2].Id, TagId = tags.First(t => t.Name == "learning").Id },
                new EventTag { EventId = events[2].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[3].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[3].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[4].Id, TagId = tags.First(t => t.Name == "family-friendly").Id },
                new EventTag { EventId = events[5].Id, TagId = tags.First(t => t.Name == "learning").Id },
                new EventTag { EventId = events[6].Id, TagId = tags.First(t => t.Name == "outdoor").Id },
                new EventTag { EventId = events[6].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[7].Id, TagId = tags.First(t => t.Name == "wellness").Id }
            };
            context.EventTags.AddRange(eventTags);
            context.SaveChanges();

            // Seed Registrations
            var confirmedStatus = statuses.First(s => s.Name == "Confirmed");
            var waitlistedStatus = statuses.First(s => s.Name == "Waitlisted");
            var cancelledStatus = statuses.First(s => s.Name == "Cancelled");

            var registrations = new Registration[]
            {
                // John Doe registrations
                new Registration
                {
                    EventId = events[0].Id,
                    UserId = users[2].Id,
                    StatusId = confirmedStatus.Id,
                    RegisteredAt = DateTime.UtcNow.AddDays(-5)
                },
                new Registration
                {
                    EventId = events[2].Id,
                    UserId = users[2].Id,
                    StatusId = confirmedStatus.Id,
                    RegisteredAt = DateTime.UtcNow.AddDays(-3)
                },
                new Registration
                {
                    EventId = events[3].Id,
                    UserId = users[2].Id,
                    StatusId = confirmedStatus.Id,
                    RegisteredAt = DateTime.UtcNow.AddDays(-1)
                },
                
                // Jane Smith registrations
                new Registration
                {
                    EventId = events[0].Id,
                    UserId = users[3].Id,
                    StatusId = confirmedStatus.Id,
                    RegisteredAt = DateTime.UtcNow.AddDays(-4)
                },
                new Registration
                {
                    EventId = events[1].Id,
                    UserId = users[3].Id,
                    StatusId = confirmedStatus.Id,
                    RegisteredAt = DateTime.UtcNow.AddDays(-2)
                },
                new Registration
                {
                    EventId = events[7].Id,
                    UserId = users[3].Id,
                    StatusId = waitlistedStatus.Id,
                    RegisteredAt = DateTime.UtcNow.AddHours(-5)
                }
            };
            context.Registrations.AddRange(registrations);
            context.SaveChanges();
        }
    }
}
