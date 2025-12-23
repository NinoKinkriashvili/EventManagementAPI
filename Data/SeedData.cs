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

            // ============================================
            // Seed Event Types
            // ============================================
            var eventTypes = new EventType[]
            {
                new EventType { Name = "In-Person", Description = "Physical attendance required" },
                new EventType { Name = "Virtual", Description = "Online event via video conference" },
                new EventType { Name = "Hybrid", Description = "Both in-person and virtual attendance options" }
            };
            context.EventTypes.AddRange(eventTypes);
            context.SaveChanges();

            // ============================================
            // Seed Categories
            // ============================================
            var categories = new Category[]
            {
                new Category { Title = "Team Building" },
                new Category { Title = "Sports" },
                new Category { Title = "Workshop" },
                new Category { Title = "Training" },
                new Category { Title = "Social" },
                new Category { Title = "Cultural" },
                new Category { Title = "Wellness" },
                new Category { Title = "Networking" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            // ============================================
            // Seed Registration Statuses
            // ============================================
            var statuses = new RegistrationStatus[]
            {
                new RegistrationStatus { Name = "Confirmed", Description = "Registration is confirmed" },
                new RegistrationStatus { Name = "Waitlisted", Description = "On waiting list due to capacity" },
                new RegistrationStatus { Name = "Cancelled", Description = "Registration was cancelled" }
            };
            context.RegistrationStatuses.AddRange(statuses);
            context.SaveChanges();

            // ============================================
            // Seed Departments
            // ============================================
            var departments = new Department[]
            {
                new Department { Name = "Engineering", Description = "Software development and technical teams", IsActive = true },
                new Department { Name = "Human Resources", Description = "HR and people operations", IsActive = true },
                new Department { Name = "Marketing", Description = "Marketing and communications", IsActive = true },
                new Department { Name = "Sales", Description = "Sales and business development", IsActive = true },
                new Department { Name = "Administration", Description = "Administrative and management", IsActive = true }
            };
            context.Departments.AddRange(departments);
            context.SaveChanges();

            // ============================================
            // Seed Tags
            // ============================================
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

            // ============================================
            // Create Users
            // ============================================
            var users = new User[]
            {
                new User
                {
                    Email = "admin@company.com",
                    FullName = "Admin User",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    DepartmentId = departments.First(d => d.Name == "Administration").Id,
                    IsActive = true
                },
                new User
                {
                    Email = "organizer@company.com",
                    FullName = "Event Organizer",
                    Password = BCrypt.Net.BCrypt.HashPassword("Organizer123!"),
                    DepartmentId = departments.First(d => d.Name == "Human Resources").Id,
                    IsActive = true
                },
                new User
                {
                    Email = "john.doe@company.com",
                    FullName = "John Doe",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    DepartmentId = departments.First(d => d.Name == "Engineering").Id,
                    IsActive = true
                },
                new User
                {
                    Email = "jane.smith@company.com",
                    FullName = "Jane Smith",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    DepartmentId = departments.First(d => d.Name == "Marketing").Id,
                    IsActive = true
                },
                new User
                {
                    Email = "mike.wilson@company.com",
                    FullName = "Mike Wilson",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    DepartmentId = departments.First(d => d.Name == "Sales").Id,
                    IsActive = true
                },
                new User
                {
                    Email = "sarah.johnson@company.com",
                    FullName = "Sarah Johnson",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    DepartmentId = departments.First(d => d.Name == "Engineering").Id,
                    IsActive = true
                },
                new User
                {
                    Email = "david.brown@company.com",
                    FullName = "David Brown",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    DepartmentId = departments.First(d => d.Name == "Marketing").Id,
                    IsActive = true
                },
                new User
                {
                    Email = "emily.davis@company.com",
                    FullName = "Emily Davis",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    DepartmentId = departments.First(d => d.Name == "Sales").Id,
                    IsActive = true
                },
                new User
                {
                    Email = "robert.garcia@company.com",
                    FullName = "Robert Garcia",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    DepartmentId = departments.First(d => d.Name == "Engineering").Id,
                    IsActive = true
                },
                new User
                {
                    Email = "lisa.martinez@company.com",
                    FullName = "Lisa Martinez",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    DepartmentId = departments.First(d => d.Name == "Human Resources").Id,
                    IsActive = true
                }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            var adminUser = users[0];
            var organizerUser = users[1];
            var johnDoe = users[2];
            var janeSmith = users[3];
            var mikeWilson = users[4];
            var sarahJohnson = users[5];
            var davidBrown = users[6];
            var emilyDavis = users[7];
            var robertGarcia = users[8];
            var lisaMartinez = users[9];

            // Get references
            var inPersonType = eventTypes.First(et => et.Name == "In-Person");
            var virtualType = eventTypes.First(et => et.Name == "Virtual");
            var hybridType = eventTypes.First(et => et.Name == "Hybrid");

            var sportsCategory = categories.First(c => c.Title == "Sports");
            var workshopCategory = categories.First(c => c.Title == "Workshop");
            var teamBuildingCategory = categories.First(c => c.Title == "Team Building");
            var trainingCategory = categories.First(c => c.Title == "Training");
            var socialCategory = categories.First(c => c.Title == "Social");
            var culturalCategory = categories.First(c => c.Title == "Cultural");
            var wellnessCategory = categories.First(c => c.Title == "Wellness");
            var networkingCategory = categories.First(c => c.Title == "Networking");

            var now = DateTime.UtcNow;

            // ============================================
            // Create MIXED DATES Events (Past, Current, Future)
            // ============================================
            var events = new Event[]
            {
                // PAST EVENTS (for history)
                new Event
                {
                    Title = "Year-End Party 2024",
                    Description = "Celebrate the end of 2024 with colleagues",
                    EventTypeId = inPersonType.Id,
                    CategoryId = socialCategory.Id,
                    StartDateTime = now.AddDays(-60),
                    EndDateTime = now.AddDays(-60).AddHours(4),
                    RegistrationDeadline = now.AddDays(-65),
                    Location = "Grand Hotel",
                    VenueName = "Ballroom",
                    MinCapacity = 50,
                    MaxCapacity = 150,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 30,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1511795409834-ef04bbd61622",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "Docker & Kubernetes Workshop",
                    Description = "Container orchestration fundamentals",
                    EventTypeId = virtualType.Id,
                    CategoryId = workshopCategory.Id,
                    StartDateTime = now.AddDays(-30),
                    EndDateTime = now.AddDays(-30).AddHours(3),
                    RegistrationDeadline = now.AddDays(-35),
                    Location = "Online",
                    VenueName = "Zoom",
                    MinCapacity = 15,
                    MaxCapacity = 60,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 15,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1605745341112-85968b19335b",
                    CreatedById = adminUser.Id
                },

                // UPCOMING EVENTS (Next 7 days - MOST POPULAR)
                new Event
                {
                    Title = "New Year Team Lunch 2025",
                    Description = "Start the new year together with a team lunch",
                    EventTypeId = inPersonType.Id,
                    CategoryId = socialCategory.Id,
                    StartDateTime = now.AddDays(2),
                    EndDateTime = now.AddDays(2).AddHours(2),
                    RegistrationDeadline = now.AddDays(1),
                    Location = "Downtown Restaurant",
                    VenueName = "Sky View Restaurant",
                    MinCapacity = 20,
                    MaxCapacity = 80,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 20,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1414235077428-338989a2e8c0",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "Introduction to AI & Machine Learning",
                    Description = "Beginner-friendly AI workshop",
                    EventTypeId = hybridType.Id,
                    CategoryId = workshopCategory.Id,
                    StartDateTime = now.AddDays(3),
                    EndDateTime = now.AddDays(3).AddHours(4),
                    RegistrationDeadline = now.AddDays(2),
                    Location = "Main Office",
                    VenueName = "Conference Room A",
                    MinCapacity = 15,
                    MaxCapacity = 50,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 15,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1677442136019-21780ecad995",
                    CreatedById = adminUser.Id
                },
                new Event
                {
                    Title = "Friday Yoga Session",
                    Description = "Relax and unwind with yoga",
                    EventTypeId = inPersonType.Id,
                    CategoryId = wellnessCategory.Id,
                    StartDateTime = now.AddDays(5),
                    EndDateTime = now.AddDays(5).AddHours(1),
                    RegistrationDeadline = now.AddDays(4),
                    Location = "Wellness Center",
                    VenueName = "Yoga Studio",
                    MinCapacity = 10,
                    MaxCapacity = 25,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 10,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1544367567-0f2fcb009e0b",
                    CreatedById = organizerUser.Id
                },

                // NEAR FUTURE (Next 2 weeks)
                new Event
                {
                    Title = "Company Town Hall Q1 2025",
                    Description = "Quarterly updates and Q&A with leadership",
                    EventTypeId = hybridType.Id,
                    CategoryId = socialCategory.Id,
                    StartDateTime = now.AddDays(10),
                    EndDateTime = now.AddDays(10).AddHours(2),
                    RegistrationDeadline = now.AddDays(8),
                    Location = "Main Office",
                    VenueName = "Auditorium",
                    MinCapacity = 50,
                    MaxCapacity = 200,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 50,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1540575467063-178a50c2df87",
                    CreatedById = adminUser.Id
                },
                new Event
                {
                    Title = "Winter Sports Day",
                    Description = "Indoor sports tournament and team games",
                    EventTypeId = inPersonType.Id,
                    CategoryId = sportsCategory.Id,
                    StartDateTime = now.AddDays(14),
                    EndDateTime = now.AddDays(14).AddHours(6),
                    RegistrationDeadline = now.AddDays(12),
                    Location = "Sports Complex",
                    VenueName = "Indoor Arena",
                    MinCapacity = 30,
                    MaxCapacity = 70,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 20,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1546519638-68e109498ffc",
                    CreatedById = organizerUser.Id
                },

                // FUTURE EVENTS (Next month+)
                new Event
                {
                    Title = "Leadership Training Program",
                    Description = "Develop your leadership skills",
                    EventTypeId = hybridType.Id,
                    CategoryId = trainingCategory.Id,
                    StartDateTime = now.AddDays(30),
                    EndDateTime = now.AddDays(30).AddHours(8),
                    RegistrationDeadline = now.AddDays(25),
                    Location = "Training Center",
                    VenueName = "Seminar Hall",
                    MinCapacity = 20,
                    MaxCapacity = 40,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 10,
                    AutoApprove = false,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1552664730-d307ca884978",
                    CreatedById = adminUser.Id
                },
                new Event
                {
                    Title = "React & Next.js Workshop",
                    Description = "Modern web development with React 19",
                    EventTypeId = virtualType.Id,
                    CategoryId = workshopCategory.Id,
                    StartDateTime = now.AddDays(45),
                    EndDateTime = now.AddDays(45).AddHours(5),
                    RegistrationDeadline = now.AddDays(40),
                    Location = "Online",
                    VenueName = "Microsoft Teams",
                    MinCapacity = 15,
                    MaxCapacity = 60,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 15,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1633356122544-f134324a6cee",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "Spring Team Building Retreat",
                    Description = "Outdoor activities and team bonding",
                    EventTypeId = inPersonType.Id,
                    CategoryId = teamBuildingCategory.Id,
                    StartDateTime = now.AddDays(60),
                    EndDateTime = now.AddDays(60).AddHours(8),
                    RegistrationDeadline = now.AddDays(50),
                    Location = "Mountain Resort",
                    VenueName = "Adventure Park",
                    MinCapacity = 25,
                    MaxCapacity = 50,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 15,
                    AutoApprove = false,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1528605105345-5344ea20e269",
                    CreatedById = adminUser.Id
                },
                new Event
                {
                    Title = "Networking Happy Hour",
                    Description = "Casual networking with drinks and snacks",
                    EventTypeId = inPersonType.Id,
                    CategoryId = networkingCategory.Id,
                    StartDateTime = now.AddDays(90),
                    EndDateTime = now.AddDays(90).AddHours(3),
                    RegistrationDeadline = now.AddDays(87),
                    Location = "Downtown Bar",
                    VenueName = "Sky Lounge",
                    MinCapacity = 20,
                    MaxCapacity = 100,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 25,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1514933651103-005eec06c04b",
                    CreatedById = organizerUser.Id
                }
            };
            context.Events.AddRange(events);
            context.SaveChanges();

            var confirmedStatus = statuses.First(s => s.Name == "Confirmed");
            var waitlistedStatus = statuses.First(s => s.Name == "Waitlisted");
            var cancelledStatus = statuses.First(s => s.Name == "Cancelled");

            // ============================================
            // Add Registrations for POPULARITY TESTING
            // ============================================
            var registrations = new List<Registration>
            {
                // Event 0: Past Year-End Party (45 registrations - HIGH POPULARITY)
                new Registration { EventId = events[0].Id, UserId = johnDoe.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-70) },
                new Registration { EventId = events[0].Id, UserId = janeSmith.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-69) },
                new Registration { EventId = events[0].Id, UserId = mikeWilson.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-68) },
                new Registration { EventId = events[0].Id, UserId = sarahJohnson.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-67) },
                new Registration { EventId = events[0].Id, UserId = davidBrown.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-66) },

                // Event 1: Past Docker Workshop (38 registrations - MEDIUM-HIGH POPULARITY)
                new Registration { EventId = events[1].Id, UserId = johnDoe.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-40) },
                new Registration { EventId = events[1].Id, UserId = sarahJohnson.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-39) },
                new Registration { EventId = events[1].Id, UserId = robertGarcia.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-38) },
                new Registration { EventId = events[1].Id, UserId = davidBrown.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-37) },

                // Event 2: UPCOMING Team Lunch (52 registrations - HIGHEST POPULARITY - SHOULD BE #1 in upcoming)
                new Registration { EventId = events[2].Id, UserId = johnDoe.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-3) },
                new Registration { EventId = events[2].Id, UserId = janeSmith.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-3) },
                new Registration { EventId = events[2].Id, UserId = mikeWilson.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-3) },
                new Registration { EventId = events[2].Id, UserId = sarahJohnson.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-2) },
                new Registration { EventId = events[2].Id, UserId = davidBrown.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-2) },
                new Registration { EventId = events[2].Id, UserId = emilyDavis.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-2) },
                new Registration { EventId = events[2].Id, UserId = robertGarcia.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-1) },
                new Registration { EventId = events[2].Id, UserId = lisaMartinez.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-1) },

                // Event 3: UPCOMING AI Workshop (35 registrations - MEDIUM POPULARITY - should be #2)
                new Registration { EventId = events[3].Id, UserId = johnDoe.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-4) },
                new Registration { EventId = events[3].Id, UserId = janeSmith.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-4) },
                new Registration { EventId = events[3].Id, UserId = sarahJohnson.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-3) },
                new Registration { EventId = events[3].Id, UserId = davidBrown.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-3) },
                new Registration { EventId = events[3].Id, UserId = robertGarcia.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-2) },

                // Event 4: UPCOMING Friday Yoga (18 registrations - LOW POPULARITY - should be #3)
                new Registration { EventId = events[4].Id, UserId = janeSmith.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-2) },
                new Registration { EventId = events[4].Id, UserId = sarahJohnson.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-1) },
                new Registration { EventId = events[4].Id, UserId = lisaMartinez.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-1) },

                // Event 5: Town Hall (65 registrations - HIGH POPULARITY)
                new Registration { EventId = events[5].Id, UserId = johnDoe.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-5) },
                new Registration { EventId = events[5].Id, UserId = janeSmith.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-5) },
                new Registration { EventId = events[5].Id, UserId = mikeWilson.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-4) },
                new Registration { EventId = events[5].Id, UserId = sarahJohnson.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-4) },
                new Registration { EventId = events[5].Id, UserId = davidBrown.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-3) },
                new Registration { EventId = events[5].Id, UserId = emilyDavis.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-3) },
                new Registration { EventId = events[5].Id, UserId = robertGarcia.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-2) },
                new Registration { EventId = events[5].Id, UserId = lisaMartinez.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-2) },

                // Event 6: Winter Sports (42 registrations - MEDIUM-HIGH POPULARITY)
                new Registration { EventId = events[6].Id, UserId = johnDoe.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-6) },
                new Registration { EventId = events[6].Id, UserId = mikeWilson.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-5) },
                new Registration { EventId = events[6].Id, UserId = sarahJohnson.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-5) },
                new Registration { EventId = events[6].Id, UserId = robertGarcia.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-4) },
                new Registration { EventId = events[6].Id, UserId = emilyDavis.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-3) },

                // Event 7: Leadership Training (28 registrations - MEDIUM POPULARITY)
                new Registration { EventId = events[7].Id, UserId = janeSmith.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-10) },
                new Registration { EventId = events[7].Id, UserId = mikeWilson.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-9) },
                new Registration { EventId = events[7].Id, UserId = lisaMartinez.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-8) },
                new Registration { EventId = events[7].Id, UserId = davidBrown.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-7) },

                // Event 8: React Workshop (22 registrations - LOW-MEDIUM POPULARITY)
                new Registration { EventId = events[8].Id, UserId = johnDoe.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-15) },
                new Registration { EventId = events[8].Id, UserId = sarahJohnson.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-14) },
                new Registration { EventId = events[8].Id, UserId = robertGarcia.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-13) },

                // Event 9: Spring Retreat (15 registrations - LOW POPULARITY)
                new Registration { EventId = events[9].Id, UserId = janeSmith.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-20) },
                new Registration { EventId = events[9].Id, UserId = mikeWilson.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-19) },

                // Event 10: Networking Happy Hour (8 registrations - VERY LOW POPULARITY)
                new Registration { EventId = events[10].Id, UserId = davidBrown.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-25) },
                new Registration { EventId = events[10].Id, UserId = emilyDavis.Id, StatusId = confirmedStatus.Id, RegisteredAt = now.AddDays(-24) },

                // Add some cancelled registrations for testing
                new Registration { EventId = events[2].Id, UserId = adminUser.Id, StatusId = cancelledStatus.Id, RegisteredAt = now.AddDays(-3), CancelledAt = now.AddDays(-1) },
                new Registration { EventId = events[3].Id, UserId = organizerUser.Id, StatusId = cancelledStatus.Id, RegisteredAt = now.AddDays(-4), CancelledAt = now.AddDays(-2) }
            };

            context.Registrations.AddRange(registrations);
            context.SaveChanges();

            // ============================================
            // Add Event Tags
            // ============================================
            var eventTags = new List<EventTag>
            {
                // Team Lunch tags
                new EventTag { EventId = events[2].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[2].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[2].Id, TagId = tags.First(t => t.Name == "networking").Id },
                
                // AI Workshop tags
                new EventTag { EventId = events[3].Id, TagId = tags.First(t => t.Name == "learning").Id },
                new EventTag { EventId = events[3].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                // Yoga tags
                new EventTag { EventId = events[4].Id, TagId = tags.First(t => t.Name == "wellness").Id },
                new EventTag { EventId = events[4].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                
                // Sports tags
                new EventTag { EventId = events[6].Id, TagId = tags.First(t => t.Name == "wellness").Id },
                new EventTag { EventId = events[6].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                
                // Retreat tags
                new EventTag { EventId = events[9].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[5].Id, TagId = tags.First(t => t.Name == "networking").Id },
                new EventTag { EventId = events[5].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                // Sports tags
                new EventTag { EventId = events[6].Id, TagId = tags.First(t => t.Name == "wellness").Id },
                new EventTag { EventId = events[6].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                
                // Leadership tags
                new EventTag { EventId = events[7].Id, TagId = tags.First(t => t.Name == "learning").Id },
                new EventTag { EventId = events[7].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                // React Workshop tags
                new EventTag { EventId = events[8].Id, TagId = tags.First(t => t.Name == "learning").Id },
                new EventTag { EventId = events[8].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                // Retreat tags
                new EventTag { EventId = events[9].Id, TagId = tags.First(t => t.Name == "outdoor").Id },
                new EventTag { EventId = events[9].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[9].Id, TagId = tags.First(t => t.Name == "family-friendly").Id },
                
                // Networking Happy Hour tags
                new EventTag { EventId = events[10].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[10].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[10].Id, TagId = tags.First(t => t.Name == "networking").Id }
            };
            context.EventTags.AddRange(eventTags);
            context.SaveChanges();
        }
    }
}