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
                return;
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

            // Create 5 users (1 Admin, 1 Organizer, 3 Employees)
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
                },
                new User
                {
                    Email = "mike.wilson@company.com",
                    FullName = "Mike Wilson",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    RoleId = roles.First(r => r.Name == "Employee").Id
                }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            var adminUser = users[0];
            var organizerUser = users[1];
            var johnDoe = users[2];
            var janeSmith = users[3];
            var mikeWilson = users[4];

            //Create 10 events with mix of past, current, and future
            var events = new Event[]
            {
                new Event
                {
                    Title = "Annual Sports Tournament 2025",
                    Description = "Year-end sports competition with multiple teams",
                    EventTypeId = eventTypes.First(et => et.Name == "Sports").Id,
                    StartDateTime = new DateTime(2025, 11, 15, 9, 0, 0),
                    EndDateTime = new DateTime(2025, 11, 15, 17, 0, 0),
                    Location = "Main Sports Arena",
                    Capacity = 100,
                    ImageUrl = "https://example.com/sports.jpg",
                    CreatedById = adminUser.Id
                },
                new Event
                {
                    Title = "JavaScript Workshop",
                    Description = "Beginner-friendly JavaScript coding workshop",
                    EventTypeId = eventTypes.First(et => et.Name == "Workshop").Id,
                    StartDateTime = new DateTime(2024, 12, 1, 10, 0, 0),
                    EndDateTime = new DateTime(2024, 12, 1, 15, 0, 0),
                    Location = "Training Room A",
                    Capacity = 30,
                    ImageUrl = "https://example.com/js-workshop.jpg",
                    CreatedById = organizerUser.Id
                },
                
                new Event
                {
                    Title = "Happy Friday Celebration",
                    Description = "Weekly Friday celebration with food and games",
                    EventTypeId = eventTypes.First(et => et.Name == "Happy Friday").Id,
                    StartDateTime = new DateTime(2025, 12, 26, 18, 0, 0),
                    EndDateTime = new DateTime(2025, 12, 26, 22, 0, 0),
                    Location = "Grand Ballroom",
                    Capacity = 100,
                    ImageUrl = "https://example.com/friday.jpg",
                    CreatedById = adminUser.Id
                },
                new Event
                {
                    Title = "Team Building Retreat",
                    Description = "Outdoor team building activities and challenges",
                    EventTypeId = eventTypes.First(et => et.Name == "Team Building").Id,
                    StartDateTime = new DateTime(2025, 12, 28, 9, 0, 0),
                    EndDateTime = new DateTime(2025, 12, 28, 16, 0, 0),
                    Location = "Adventure Park",
                    Capacity = 30,
                    ImageUrl = "https://example.com/team-building.jpg",
                    CreatedById = organizerUser.Id
                },
                
                new Event
                {
                    Title = "Coffee & Code Social",
                    Description = "Casual morning coding session with coffee",
                    EventTypeId = eventTypes.First(et => et.Name == "Social").Id,
                    StartDateTime = new DateTime(2025, 1, 5, 8, 0, 0),
                    EndDateTime = new DateTime(2025, 1, 5, 10, 0, 0),
                    Location = "Cafeteria",
                    Capacity = 10,
                    ImageUrl = "https://example.com/coffee-code.jpg",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "SQL Database Training",
                    Description = "Advanced SQL database optimization techniques",
                    EventTypeId = eventTypes.First(et => et.Name == "Training").Id,
                    StartDateTime = new DateTime(2025, 1, 10, 13, 0, 0),
                    EndDateTime = new DateTime(2025, 1, 10, 17, 0, 0),
                    Location = "Training Room B",
                    Capacity = 10, 
                    ImageUrl = "https://example.com/sql-training.jpg",
                    CreatedById = adminUser.Id
                },
                
                new Event
                {
                    Title = "Python Workshop",
                    Description = "Introduction to data science using Python",
                    EventTypeId = eventTypes.First(et => et.Name == "Workshop").Id,
                    StartDateTime = new DateTime(2025, 1, 15, 9, 0, 0),
                    EndDateTime = new DateTime(2025, 1, 15, 17, 0, 0),
                    Location = "Lab Room 101",
                    Capacity = 40,
                    ImageUrl = "https://example.com/python-workshop.jpg",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "Cultural Festival 2025",
                    Description = "Annual cultural celebration with performances",
                    EventTypeId = eventTypes.First(et => et.Name == "Cultural").Id,
                    StartDateTime = new DateTime(2025, 2, 1, 9, 0, 0),
                    EndDateTime = new DateTime(2025, 2, 1, 18, 0, 0),
                    Location = "Cultural Center",
                    Capacity = 50,
                    ImageUrl = "https://example.com/cultural.jpg",
                    CreatedById = adminUser.Id
                },
                
                new Event
                {
                    Title = "Spring Networking Social",
                    Description = "Large networking event with multiple companies",
                    EventTypeId = eventTypes.First(et => et.Name == "Social").Id,
                    StartDateTime = new DateTime(2025, 3, 15, 17, 0, 0),
                    EndDateTime = new DateTime(2025, 3, 15, 21, 0, 0),
                    Location = "Convention Center",
                    Capacity = 100,
                    ImageUrl = "https://example.com/networking.jpg",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "Summer Sports Day",
                    Description = "Company-wide sports tournament and activities",
                    EventTypeId = eventTypes.First(et => et.Name == "Sports").Id,
                    StartDateTime = new DateTime(2025, 4, 10, 8, 0, 0),
                    EndDateTime = new DateTime(2025, 4, 10, 18, 0, 0),
                    Location = "Sports Complex",
                    Capacity = 100,
                    ImageUrl = "https://example.com/sports-day.jpg",
                    CreatedById = adminUser.Id
                }
            };
            context.Events.AddRange(events);
            context.SaveChanges();

            // Add more diverse event tags
            var eventTags = new List<EventTag>
            {
                new EventTag { EventId = events[0].Id, TagId = tags.First(t => t.Name == "outdoor").Id },
                new EventTag { EventId = events[0].Id, TagId = tags.First(t => t.Name == "family-friendly").Id },
                
                new EventTag { EventId = events[1].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[1].Id, TagId = tags.First(t => t.Name == "learning").Id },
                new EventTag { EventId = events[1].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                new EventTag { EventId = events[2].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[2].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[2].Id, TagId = tags.First(t => t.Name == "networking").Id },
                
                new EventTag { EventId = events[3].Id, TagId = tags.First(t => t.Name == "outdoor").Id },
                new EventTag { EventId = events[3].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[3].Id, TagId = tags.First(t => t.Name == "family-friendly").Id },
                
                new EventTag { EventId = events[4].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[4].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[4].Id, TagId = tags.First(t => t.Name == "networking").Id },
                
                new EventTag { EventId = events[5].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[5].Id, TagId = tags.First(t => t.Name == "learning").Id },
                new EventTag { EventId = events[5].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                new EventTag { EventId = events[6].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[6].Id, TagId = tags.First(t => t.Name == "learning").Id },
                new EventTag { EventId = events[6].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                new EventTag { EventId = events[7].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[7].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[7].Id, TagId = tags.First(t => t.Name == "family-friendly").Id },
                new EventTag { EventId = events[7].Id, TagId = tags.First(t => t.Name == "networking").Id },
                
                new EventTag { EventId = events[8].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[8].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[8].Id, TagId = tags.First(t => t.Name == "networking").Id },
                
                new EventTag { EventId = events[9].Id, TagId = tags.First(t => t.Name == "outdoor").Id },
                new EventTag { EventId = events[9].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[9].Id, TagId = tags.First(t => t.Name == "family-friendly").Id }
            };
            context.EventTags.AddRange(eventTags);
            context.SaveChanges();

            // Add more registrations (confirmed, waitlisted, cancelled)
            var confirmedStatus = statuses.First(s => s.Name == "Confirmed");
            var waitlistedStatus = statuses.First(s => s.Name == "Waitlisted");
            var cancelledStatus = statuses.First(s => s.Name == "Cancelled");

            var registrations = new Registration[]
            {
                new Registration
                {
                    EventId = events[0].Id,
                    UserId = adminUser.Id,
                    StatusId = confirmedStatus.Id,
                    RegisteredAt = new DateTime(2024, 11, 1, 10, 0, 0)
                },
                new Registration
                {
                    EventId = events[2].Id,
                    UserId = adminUser.Id,
                    StatusId = confirmedStatus.Id,
                    RegisteredAt = new DateTime(2024, 12, 10, 14, 30, 0)
                },
                
                new Registration
                {
                    EventId = events[0].Id,
                    UserId = johnDoe.Id,
                    StatusId = confirmedStatus.Id,
                    RegisteredAt = new DateTime(2024, 11, 2, 8, 0, 0)
                },
                new Registration
                {
                    EventId = events[1].Id,
                    UserId = johnDoe.Id,
                    StatusId = confirmedStatus.Id,
                    RegisteredAt = new DateTime(2024, 11, 25, 13, 0, 0)
                },
                new Registration
                {
                    EventId = events[4].Id,
                    UserId = johnDoe.Id,
                    StatusId = confirmedStatus.Id,
                    RegisteredAt = new DateTime(2024, 12, 20, 10, 30, 0)
                },
                
                new Registration
                {
                    EventId = events[0].Id,
                    UserId = janeSmith.Id,
                    StatusId = confirmedStatus.Id,
                    RegisteredAt = new DateTime(2024, 11, 5, 11, 0, 0)
                },
                new Registration
                {
                    EventId = events[3].Id,
                    UserId = janeSmith.Id,
                    StatusId = confirmedStatus.Id,
                    RegisteredAt = new DateTime(2024, 12, 18, 15, 45, 0)
                },
                new Registration
                {
                    EventId = events[6].Id,
                    UserId = janeSmith.Id,
                    StatusId = confirmedStatus.Id,
                    RegisteredAt = new DateTime(2024, 12, 16, 11, 0, 0)
                },
                
                new Registration
                {
                    EventId = events[2].Id,
                    UserId = mikeWilson.Id,
                    StatusId = confirmedStatus.Id,
                    RegisteredAt = new DateTime(2024, 12, 13, 12, 0, 0)
                },
                new Registration
                {
                    EventId = events[5].Id,
                    UserId = mikeWilson.Id,
                    StatusId = confirmedStatus.Id,
                    RegisteredAt = new DateTime(2025, 1, 2, 9, 0, 0)
                },
                
                new Registration
                {
                    EventId = events[4].Id,
                    UserId = janeSmith.Id,
                    StatusId = waitlistedStatus.Id,
                    RegisteredAt = new DateTime(2024, 12, 21, 18, 0, 0)
                },
                new Registration
                {
                    EventId = events[4].Id,
                    UserId = mikeWilson.Id,
                    StatusId = waitlistedStatus.Id,
                    RegisteredAt = new DateTime(2024, 12, 21, 19, 30, 0)
                },
                new Registration
                {
                    EventId = events[5].Id,
                    UserId = johnDoe.Id,
                    StatusId = waitlistedStatus.Id,
                    RegisteredAt = new DateTime(2025, 1, 3, 10, 0, 0)
                },
                
                new Registration
                {
                    EventId = events[3].Id,
                    UserId = johnDoe.Id,
                    StatusId = cancelledStatus.Id,
                    RegisteredAt = new DateTime(2024, 12, 17, 11, 0, 0)
                },
                new Registration
                {
                    EventId = events[6].Id,
                    UserId = mikeWilson.Id,
                    StatusId = cancelledStatus.Id,
                    RegisteredAt = new DateTime(2024, 12, 14, 14, 0, 0)
                },
                new Registration
                {
                    EventId = events[0].Id,
                    UserId = mikeWilson.Id,
                    StatusId = cancelledStatus.Id,
                    RegisteredAt = new DateTime(2024, 11, 10, 16, 0, 0)
                }
            };
            context.Registrations.AddRange(registrations);
            context.SaveChanges();
        }
    }
}