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
            // Seed Event Types (delivery mode)
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
            // Seed Categories (activity type)
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
            // Seed Roles
            // ============================================
            var roles = new Role[]
            {
                new Role { Name = "Employee", Description = "Regular employee" },
                new Role { Name = "Organizer", Description = "Can create and manage events" },
                new Role { Name = "Admin", Description = "Full system access" }
            };
            context.Roles.AddRange(roles);
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

            // Get references for easy access
            var inPersonType = eventTypes.First(et => et.Name == "In-Person");
            var virtualType = eventTypes.First(et => et.Name == "Virtual");
            var hybridType = eventTypes.First(et => et.Name == "Hybrid");

            var sportsCategory = categories.First(c => c.Title == "Sports");
            var workshopCategory = categories.First(c => c.Title == "Workshop");
            var teamBuildingCategory = categories.First(c => c.Title == "Team Building");
            var trainingCategory = categories.First(c => c.Title == "Training");
            var socialCategory = categories.First(c => c.Title == "Social");
            var culturalCategory = categories.First(c => c.Title == "Cultural");

            // ============================================
            // Create Events with ALL new fields
            // ============================================
            var events = new Event[]
            {
                new Event
                {
                    Title = "Annual Sports Tournament 2025",
                    Description = "Year-end sports competition with multiple teams",
                    EventTypeId = inPersonType.Id,
                    CategoryId = sportsCategory.Id,
                    StartDateTime = new DateTime(2025, 11, 15, 9, 0, 0),
                    EndDateTime = new DateTime(2025, 11, 15, 17, 0, 0),
                    RegistrationDeadline = new DateTime(2025, 11, 10, 23, 59, 59),
                    Location = "Main Office Building",
                    VenueName = "Sports Arena Complex",
                    //Capacity = 80,
                    MinCapacity = 40,
                    MaxCapacity = 100,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 20,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://example.com/sports.jpg",
                    CreatedById = adminUser.Id
                },
                new Event
                {
                    Title = "JavaScript Workshop",
                    Description = "Beginner-friendly JavaScript coding workshop",
                    EventTypeId = hybridType.Id,
                    CategoryId = workshopCategory.Id,
                    StartDateTime = new DateTime(2024, 12, 27, 10, 0, 0),
                    EndDateTime = new DateTime(2024, 12, 27, 15, 0, 0),
                    RegistrationDeadline = new DateTime(2024, 12, 25, 23, 59, 59),
                    Location = "Main Office - Training Wing",
                    VenueName = "Training Room A",
                    //Capacity = 25,
                    MinCapacity = 10,
                    MaxCapacity = 30,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 10,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://example.com/js-workshop.jpg",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "Year-End Celebration",
                    Description = "Company year-end celebration with food and games",
                    EventTypeId = inPersonType.Id,
                    CategoryId = socialCategory.Id,
                    StartDateTime = new DateTime(2025, 12, 26, 18, 0, 0),
                    EndDateTime = new DateTime(2025, 12, 26, 22, 0, 0),
                    RegistrationDeadline = new DateTime(2025, 12, 20, 23, 59, 59),
                    Location = "Downtown Convention Center",
                    VenueName = "Grand Ballroom",
                    //Capacity = 150,
                    MinCapacity = 80,
                    MaxCapacity = 200,
                    WaitlistEnabled = true,
                    WaitlistCapacity = null, // Unlimited waitlist
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://example.com/celebration.jpg",
                    CreatedById = adminUser.Id
                },
                new Event
                {
                    Title = "Team Building Retreat",
                    Description = "Outdoor team building activities and challenges",
                    EventTypeId = inPersonType.Id,
                    CategoryId = teamBuildingCategory.Id,
                    StartDateTime = new DateTime(2025, 12, 28, 9, 0, 0),
                    EndDateTime = new DateTime(2025, 12, 28, 16, 0, 0),
                    RegistrationDeadline = new DateTime(2025, 12, 24, 23, 59, 59),
                    Location = "Mountain Resort Area",
                    VenueName = "Adventure Park Grounds",
                    //Capacity = 40,
                    MinCapacity = 20,
                    MaxCapacity = 50,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 15,
                    AutoApprove = false, // Manual approval required
                    IsVisible = true,
                    ImageUrl = "https://example.com/team-building.jpg",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "Coffee & Code Social",
                    Description = "Casual morning coding session with coffee",
                    EventTypeId = inPersonType.Id,
                    CategoryId = socialCategory.Id,
                    StartDateTime = new DateTime(2025, 1, 5, 8, 0, 0),
                    EndDateTime = new DateTime(2025, 1, 5, 10, 0, 0),
                    RegistrationDeadline = new DateTime(2025, 1, 4, 18, 0, 0),
                    Location = "Main Office Building",
                    VenueName = "Employee Cafeteria",
                    //Capacity = 15,
                    MinCapacity = 8,
                    MaxCapacity = 20,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 5,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://example.com/coffee-code.jpg",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "SQL Database Training",
                    Description = "Advanced SQL database optimization techniques",
                    EventTypeId = virtualType.Id,
                    CategoryId = trainingCategory.Id,
                    StartDateTime = new DateTime(2025, 1, 10, 13, 0, 0),
                    EndDateTime = new DateTime(2025, 1, 10, 17, 0, 0),
                    RegistrationDeadline = new DateTime(2025, 1, 8, 23, 59, 59),
                    Location = "Online",
                    VenueName = "Zoom Meeting Room",
                    //Capacity = 50,
                    MinCapacity = 15,
                    MaxCapacity = 100,
                    WaitlistEnabled = false, // No waitlist for virtual events
                    WaitlistCapacity = null,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://example.com/sql-training.jpg",
                    CreatedById = adminUser.Id
                },
                new Event
                {
                    Title = "Python Data Science Workshop",
                    Description = "Introduction to data science using Python",
                    EventTypeId = hybridType.Id,
                    CategoryId = workshopCategory.Id,
                    StartDateTime = new DateTime(2025, 1, 15, 9, 0, 0),
                    EndDateTime = new DateTime(2025, 1, 15, 17, 0, 0),
                    RegistrationDeadline = new DateTime(2025, 1, 12, 23, 59, 59),
                    Location = "Tech Campus",
                    VenueName = "Lab Room 101",
                    //Capacity = 35,
                    MinCapacity = 15,
                    MaxCapacity = 40,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 10,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://example.com/python-workshop.jpg",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "Cultural Festival 2025",
                    Description = "Annual cultural celebration with performances",
                    EventTypeId = inPersonType.Id,
                    CategoryId = culturalCategory.Id,
                    StartDateTime = new DateTime(2025, 2, 1, 9, 0, 0),
                    EndDateTime = new DateTime(2025, 2, 1, 18, 0, 0),
                    RegistrationDeadline = new DateTime(2025, 1, 25, 23, 59, 59),
                    Location = "City Cultural Center",
                    VenueName = "Main Performance Hall",
                    //Capacity = 200,
                    MinCapacity = 100,
                    MaxCapacity = 300,
                    WaitlistEnabled = true,
                    WaitlistCapacity = null,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://example.com/cultural.jpg",
                    CreatedById = adminUser.Id
                },
                new Event
                {
                    Title = "Spring Networking Mixer",
                    Description = "Professional networking event with industry leaders",
                    EventTypeId = hybridType.Id,
                    CategoryId = socialCategory.Id,
                    StartDateTime = new DateTime(2025, 3, 15, 17, 0, 0),
                    EndDateTime = new DateTime(2025, 3, 15, 21, 0, 0),
                    RegistrationDeadline = new DateTime(2025, 3, 10, 23, 59, 59),
                    Location = "Business District",
                    VenueName = "Convention Center Hall B",
                    //Capacity = 120,
                    MinCapacity = 50,
                    MaxCapacity = 150,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 30,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://example.com/networking.jpg",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "Summer Sports Day",
                    Description = "Company-wide sports tournament and activities",
                    EventTypeId = inPersonType.Id,
                    CategoryId = sportsCategory.Id,
                    StartDateTime = new DateTime(2025, 4, 10, 8, 0, 0),
                    EndDateTime = new DateTime(2025, 4, 10, 18, 0, 0),
                    RegistrationDeadline = new DateTime(2025, 4, 5, 23, 59, 59),
                    Location = "Sports Complex",
                    VenueName = "Olympic Stadium",
                    //Capacity = 150,
                    MinCapacity = 80,
                    MaxCapacity = 200,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 50,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://example.com/sports-day.jpg",
                    CreatedById = adminUser.Id
                }
            };
            context.Events.AddRange(events);
            context.SaveChanges();

            // ============================================
            // Add Speakers
            // ============================================
            var speakers = new Speaker[]
            {
                // JavaScript Workshop speakers
                new Speaker
                {
                    EventId = events[1].Id,
                    Name = "John Smith",
                    Role = "Senior JavaScript Developer",
                    PhotoUrl = "https://example.com/speakers/john-smith.jpg"
                },
                new Speaker
                {
                    EventId = events[1].Id,
                    Name = "Sarah Johnson",
                    Role = "Tech Lead at Google",
                    PhotoUrl = "https://example.com/speakers/sarah-johnson.jpg"
                },
                
                // SQL Training speakers
                new Speaker
                {
                    EventId = events[5].Id,
                    Name = "Robert Chen",
                    Role = "Database Architect",
                    PhotoUrl = "https://example.com/speakers/robert-chen.jpg"
                },
                
                // Python Workshop speakers
                new Speaker
                {
                    EventId = events[6].Id,
                    Name = "Michael Anderson",
                    Role = "Data Science Expert",
                    PhotoUrl = "https://example.com/speakers/michael-anderson.jpg"
                },
                new Speaker
                {
                    EventId = events[6].Id,
                    Name = "Emily Davis",
                    Role = "ML Engineer at Microsoft",
                    PhotoUrl = "https://example.com/speakers/emily-davis.jpg"
                },
                
                // Cultural Festival speakers
                new Speaker
                {
                    EventId = events[7].Id,
                    Name = "Maria Garcia",
                    Role = "Cultural Ambassador",
                    PhotoUrl = "https://example.com/speakers/maria-garcia.jpg"
                },
                
                // Networking Mixer speakers
                new Speaker
                {
                    EventId = events[8].Id,
                    Name = "David Kim",
                    Role = "CEO of TechStart",
                    PhotoUrl = "https://example.com/speakers/david-kim.jpg"
                },
                new Speaker
                {
                    EventId = events[8].Id,
                    Name = "Lisa Brown",
                    Role = "VP of Innovation",
                    PhotoUrl = "https://example.com/speakers/lisa-brown.jpg"
                }
            };
            context.Speakers.AddRange(speakers);
            context.SaveChanges();

            // ============================================
            // Add Agenda Items
            // ============================================
            var agendaItems = new AgendaItem[]
            {
                // JavaScript Workshop agenda
                new AgendaItem
                {
                    EventId = events[1].Id,
                    Time = "10:00-10:30",
                    Title = "Introduction to Modern JavaScript",
                    Description = "Overview of ES6+ features and modern JavaScript development"
                },
                new AgendaItem
                {
                    EventId = events[1].Id,
                    Time = "10:30-12:00",
                    Title = "Hands-on Coding Session",
                    Description = "Build a simple web application using React and modern JS"
                },
                new AgendaItem
                {
                    EventId = events[1].Id,
                    Time = "12:00-13:00",
                    Title = "Lunch Break",
                    Description = "Networking lunch provided"
                },
                new AgendaItem
                {
                    EventId = events[1].Id,
                    Time = "13:00-14:30",
                    Title = "Advanced Topics",
                    Description = "Async/await, promises, and modern patterns"
                },
                new AgendaItem
                {
                    EventId = events[1].Id,
                    Time = "14:30-15:00",
                    Title = "Q&A Session",
                    Description = "Ask questions and get feedback from experts"
                },
                
                // SQL Training agenda
                new AgendaItem
                {
                    EventId = events[5].Id,
                    Time = "13:00-14:00",
                    Title = "SQL Performance Basics",
                    Description = "Understanding query execution plans and indexes"
                },
                new AgendaItem
                {
                    EventId = events[5].Id,
                    Time = "14:00-15:00",
                    Title = "Advanced Optimization",
                    Description = "Query tuning and database optimization techniques"
                },
                new AgendaItem
                {
                    EventId = events[5].Id,
                    Time = "15:00-16:00",
                    Title = "Real-world Case Studies",
                    Description = "Analyzing and solving common performance issues"
                },
                new AgendaItem
                {
                    EventId = events[5].Id,
                    Time = "16:00-17:00",
                    Title = "Hands-on Lab",
                    Description = "Practice optimizing queries on sample databases"
                },
                
                // Python Workshop agenda
                new AgendaItem
                {
                    EventId = events[6].Id,
                    Time = "09:00-10:00",
                    Title = "Python Fundamentals",
                    Description = "Quick review of Python basics and syntax"
                },
                new AgendaItem
                {
                    EventId = events[6].Id,
                    Time = "10:00-12:00",
                    Title = "Data Science Libraries",
                    Description = "Introduction to pandas, numpy, and matplotlib"
                },
                new AgendaItem
                {
                    EventId = events[6].Id,
                    Time = "12:00-13:00",
                    Title = "Lunch Break",
                    Description = null
                },
                new AgendaItem
                {
                    EventId = events[6].Id,
                    Time = "13:00-15:00",
                    Title = "Machine Learning Basics",
                    Description = "Introduction to scikit-learn and basic ML models"
                },
                new AgendaItem
                {
                    EventId = events[6].Id,
                    Time = "15:00-17:00",
                    Title = "Hands-on Project",
                    Description = "Build and train your first ML model"
                },
                
                // Team Building Retreat agenda
                new AgendaItem
                {
                    EventId = events[3].Id,
                    Time = "09:00-09:30",
                    Title = "Welcome & Ice Breakers",
                    Description = "Get to know your team members"
                },
                new AgendaItem
                {
                    EventId = events[3].Id,
                    Time = "09:30-11:00",
                    Title = "Outdoor Challenge 1",
                    Description = "Team problem-solving activities"
                },
                new AgendaItem
                {
                    EventId = events[3].Id,
                    Time = "11:00-12:30",
                    Title = "Outdoor Challenge 2",
                    Description = "Physical team-building exercises"
                },
                new AgendaItem
                {
                    EventId = events[3].Id,
                    Time = "12:30-13:30",
                    Title = "BBQ Lunch",
                    Description = "Team lunch and networking"
                },
                new AgendaItem
                {
                    EventId = events[3].Id,
                    Time = "13:30-15:30",
                    Title = "Final Team Challenge",
                    Description = "Culminating team activity and competition"
                },
                new AgendaItem
                {
                    EventId = events[3].Id,
                    Time = "15:30-16:00",
                    Title = "Wrap-up & Reflection",
                    Description = "Team discussion and awards ceremony"
                },
                
                // Cultural Festival agenda
                new AgendaItem
                {
                    EventId = events[7].Id,
                    Time = "09:00-10:00",
                    Title = "Opening Ceremony",
                    Description = "Welcome speeches and traditional performances"
                },
                new AgendaItem
                {
                    EventId = events[7].Id,
                    Time = "10:00-12:00",
                    Title = "Cultural Exhibits",
                    Description = "Explore booths representing different cultures"
                },
                new AgendaItem
                {
                    EventId = events[7].Id,
                    Time = "12:00-13:00",
                    Title = "International Food Fair",
                    Description = "Sample cuisines from around the world"
                },
                new AgendaItem
                {
                    EventId = events[7].Id,
                    Time = "13:00-15:00",
                    Title = "Dance & Music Performances",
                    Description = "Live performances from various cultural groups"
                },
                new AgendaItem
                {
                    EventId = events[7].Id,
                    Time = "15:00-17:00",
                    Title = "Interactive Workshops",
                    Description = "Learn traditional crafts and customs"
                },
                new AgendaItem
                {
                    EventId = events[7].Id,
                    Time = "17:00-18:00",
                    Title = "Closing Ceremony",
                    Description = "Final performances and farewell"
                }
            };
            context.AgendaItems.AddRange(agendaItems);
            context.SaveChanges();

            // ============================================
            // Add Event Tags
            // ============================================
            var eventTags = new List<EventTag>
            {
                // Sports Tournament
                new EventTag { EventId = events[0].Id, TagId = tags.First(t => t.Name == "outdoor").Id },
                new EventTag { EventId = events[0].Id, TagId = tags.First(t => t.Name == "family-friendly").Id },
                
                // JavaScript Workshop
                new EventTag { EventId = events[1].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[1].Id, TagId = tags.First(t => t.Name == "learning").Id },
                new EventTag { EventId = events[1].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                // Year-End Celebration
                new EventTag { EventId = events[2].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[2].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[2].Id, TagId = tags.First(t => t.Name == "networking").Id },
                
                // Team Building Retreat
                new EventTag { EventId = events[3].Id, TagId = tags.First(t => t.Name == "outdoor").Id },
                new EventTag { EventId = events[3].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[3].Id, TagId = tags.First(t => t.Name == "family-friendly").Id },
                
                // Coffee & Code
                new EventTag { EventId = events[4].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[4].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[4].Id, TagId = tags.First(t => t.Name == "networking").Id },
                
                // SQL Training
                new EventTag { EventId = events[5].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                new EventTag { EventId = events[5].Id, TagId = tags.First(t => t.Name == "learning").Id },
                
                // Python Workshop
                new EventTag { EventId = events[6].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[6].Id, TagId = tags.First(t => t.Name == "learning").Id },
                new EventTag { EventId = events[6].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                // Cultural Festival
                new EventTag { EventId = events[7].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[7].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[7].Id, TagId = tags.First(t => t.Name == "family-friendly").Id },
                new EventTag { EventId = events[7].Id, TagId = tags.First(t => t.Name == "networking").Id },
                
                // Networking Mixer
                new EventTag { EventId = events[8].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[8].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[8].Id, TagId = tags.First(t => t.Name == "networking").Id },
                new EventTag { EventId = events[8].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                // Summer Sports Day
                new EventTag { EventId = events[9].Id, TagId = tags.First(t => t.Name == "outdoor").Id },
                new EventTag { EventId = events[9].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[9].Id, TagId = tags.First(t => t.Name == "family-friendly").Id }
            };
            context.EventTags.AddRange(eventTags);
            context.SaveChanges();

            // ============================================
            // Add Registrations
            // ============================================
            var confirmedStatus = statuses.First(s => s.Name == "Confirmed");
            var waitlistedStatus = statuses.First(s => s.Name == "Waitlisted");
            var cancelledStatus = statuses.First(s => s.Name == "Cancelled");

            var registrations = new Registration[]
            {
                // Confirmed registrations
                new Registration { EventId = events[0].Id, UserId = adminUser.Id, StatusId = confirmedStatus.Id, RegisteredAt = new DateTime(2024, 11, 1, 10, 0, 0) },
                new Registration { EventId = events[2].Id, UserId = adminUser.Id, StatusId = confirmedStatus.Id, RegisteredAt = new DateTime(2024, 12, 10, 14, 30, 0) },

                new Registration { EventId = events[0].Id, UserId = johnDoe.Id, StatusId = confirmedStatus.Id, RegisteredAt = new DateTime(2024, 11, 2, 8, 0, 0) },
                new Registration { EventId = events[1].Id, UserId = johnDoe.Id, StatusId = confirmedStatus.Id, RegisteredAt = new DateTime(2024, 11, 25, 13, 0, 0) },
                new Registration { EventId = events[4].Id, UserId = johnDoe.Id, StatusId = confirmedStatus.Id, RegisteredAt = new DateTime(2024, 12, 20, 10, 30, 0) },

                new Registration { EventId = events[0].Id, UserId = janeSmith.Id, StatusId = confirmedStatus.Id, RegisteredAt = new DateTime(2024, 11, 5, 11, 0, 0) },
                new Registration { EventId = events[3].Id, UserId = janeSmith.Id, StatusId = confirmedStatus.Id, RegisteredAt = new DateTime(2024, 12, 18, 15, 45, 0) },
                new Registration { EventId = events[6].Id, UserId = janeSmith.Id, StatusId = confirmedStatus.Id, RegisteredAt = new DateTime(2024, 12, 16, 11, 0, 0) },

                new Registration { EventId = events[2].Id, UserId = mikeWilson.Id, StatusId = confirmedStatus.Id, RegisteredAt = new DateTime(2024, 12, 13, 12, 0, 0) },
                new Registration { EventId = events[5].Id, UserId = mikeWilson.Id, StatusId = confirmedStatus.Id, RegisteredAt = new DateTime(2025, 1, 2, 9, 0, 0) },
                
                // Waitlisted registrations
                new Registration { EventId = events[4].Id, UserId = janeSmith.Id, StatusId = waitlistedStatus.Id, RegisteredAt = new DateTime(2024, 12, 21, 18, 0, 0) },
                new Registration { EventId = events[4].Id, UserId = mikeWilson.Id, StatusId = waitlistedStatus.Id, RegisteredAt = new DateTime(2024, 12, 21, 19, 30, 0) },
                new Registration { EventId = events[5].Id, UserId = johnDoe.Id, StatusId = waitlistedStatus.Id, RegisteredAt = new DateTime(2025, 1, 3, 10, 0, 0) },
                
                // Cancelled registrations
                new Registration { EventId = events[3].Id, UserId = johnDoe.Id, StatusId = cancelledStatus.Id, RegisteredAt = new DateTime(2024, 12, 17, 11, 0, 0) },
                new Registration { EventId = events[6].Id, UserId = mikeWilson.Id, StatusId = cancelledStatus.Id, RegisteredAt = new DateTime(2024, 12, 14, 14, 0, 0) },
                new Registration { EventId = events[0].Id, UserId = mikeWilson.Id, StatusId = cancelledStatus.Id, RegisteredAt = new DateTime(2024, 11, 10, 16, 0, 0) }
            };
            context.Registrations.AddRange(registrations);
            context.SaveChanges();
        }
    }
}