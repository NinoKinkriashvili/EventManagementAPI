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
            // Seed Departments
            // ============================================
            var departments = new Department[]
            {
                new Department { Name = "Engineering", Description = "Software development and engineering", IsActive = true },
                new Department { Name = "Marketing", Description = "Marketing and communications", IsActive = true },
                new Department { Name = "Sales", Description = "Sales and business development", IsActive = true },
                new Department { Name = "HR", Description = "Human resources", IsActive = true },
                new Department { Name = "Finance", Description = "Finance and accounting", IsActive = true },
                new Department { Name = "Operations", Description = "Operations and logistics", IsActive = true },
                new Department { Name = "General", Description = "General staff", IsActive = true }
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
                    PhoneNumber = "+995555123456", // NEW
                    DepartmentId = departments.First(d => d.Name == "Engineering").Id,
                    IsAdmin = true
                },
                new User
                {
                    Email = "organizer@company.com",
                    FullName = "Event Organizer",
                    Password = BCrypt.Net.BCrypt.HashPassword("Organizer123!"),
                    PhoneNumber = "+995555234567", // NEW
                    DepartmentId = departments.First(d => d.Name == "HR").Id,
                    IsAdmin = true
                },
                new User
                {
                    Email = "john.doe@company.com",
                    FullName = "John Doe",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555345678", // NEW
                    DepartmentId = departments.First(d => d.Name == "Engineering").Id,
                    IsAdmin = false
                },
                new User
                {
                    Email = "jane.smith@company.com",
                    FullName = "Jane Smith",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555456789", // NEW
                    DepartmentId = departments.First(d => d.Name == "Marketing").Id,
                    IsAdmin = false
                },
                new User
                {
                    Email = "mike.wilson@company.com",
                    FullName = "Mike Wilson",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555567890", // NEW
                    DepartmentId = departments.First(d => d.Name == "Sales").Id,
                    IsAdmin = false
                },
                new User
                {
                    Email = "mano.tiler@company.com",
                    FullName = "Mano Tiller",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555678901",
                    DepartmentId = departments.First(d => d.Name == "Operations").Id,
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow.AddMonths(-1)
                },
                new User
                {
                    Email = "shako.gernava@company.com",
                    FullName = "Shako Gernava",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555678902",
                    DepartmentId = departments.First(d => d.Name == "Finance").Id,
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow.AddMonths(-2)
                },
                new User
                {
                    Email = "lile.janjgava@company.com",
                    FullName = "Lile Janjgava",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555678903",
                    DepartmentId = departments.First(d => d.Name == "HR").Id,
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow.AddMonths(1)
                },
                new User
                {
                    Email = "nino.kiknadze@company.com",
                    FullName = "Nino Kiknadze",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555700001",
                    DepartmentId = departments.First(d => d.Name == "Marketing").Id,
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow.AddMonths(-2)
                },
                new User
                {
                    Email = "giorgi.beridze@company.com",
                    FullName = "Giorgi Beridze",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555700002",
                    DepartmentId = departments.First(d => d.Name == "General").Id,
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow.AddMonths(-2).AddDays(-3)
                },
                new User
                {
                    Email = "mariam.gelashvili@company.com",
                    FullName = "Mariam Gelashvili",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555700003",
                    DepartmentId = departments.First(d => d.Name == "Marketing").Id,
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow.AddMonths(-2).AddDays(-10)
                },
                new User
                {
                    Email = "luka.tsereteli@company.com",
                    FullName = "Luka Tsereteli",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555700004",
                    DepartmentId = departments.First(d => d.Name == "Operations").Id,
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow.AddMonths(-2).AddDays(-20)
                },

                new User
                {
                    Email = "ana.kapanadze@company.com",
                    FullName = "Ana Kapanadze",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555700011",
                    DepartmentId = departments.First(d => d.Name == "HR").Id,
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow.AddMonths(-1)
                },
                new User
                {
                    Email = "dato.koberidze@company.com",
                    FullName = "Dato Koberidze",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555700012",
                    DepartmentId = departments.First(d => d.Name == "Sales").Id,
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow.AddMonths(-1).AddDays(-7)
                },
                new User
                {
                    Email = "salome.chikvaidze@company.com",
                    FullName = "Salome Chikvaidze",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555700013",
                    DepartmentId = departments.First(d => d.Name == "General").Id,
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow.AddMonths(-1).AddDays(-15)
                },

                new User
                {
                    Email = "irakli.mchedlishvili@company.com",
                    FullName = "Irakli Mchedlishvili",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555700021",
                    DepartmentId = departments.First(d => d.Name == "Engineering").Id,
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow.AddMonths(1)
                },
                new User
                {
                    Email = "ketevan.japaridze@company.com",
                    FullName = "Ketevan Japaridze",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555700022",
                    DepartmentId = departments.First(d => d.Name == "Sales").Id,
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow.AddMonths(1).AddDays(5)
                },
                new User
                {
                    Email = "levan.kharatishvili@company.com",
                    FullName = "Levan Kharatishvili",
                    Password = BCrypt.Net.BCrypt.HashPassword("Employee123!"),
                    PhoneNumber = "+995555700023",
                    DepartmentId = departments.First(d => d.Name == "Finance").Id,
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow.AddMonths(1).AddDays(12)
                }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            var adminUser = users[0];
            var organizerUser = users[1];
            var johnDoe = users[2];
            var janeSmith = users[3];
            var mikeWilson = users[4];
            var manoTiler = users[5];
            var shakoGernava = users[6];
            var lileJanjgava = users[7];
            var ninoKiknadze = users[8];
            var giorgiBeridze = users[9];
            var mariamGelashvili = users[10];
            var lukaTsereteli = users[11];

            var anaKapanadze = users[12];
            var datoKoberidze = users[13];
            var salomeChikvaidze = users[14];

            var irakliMchedlishvili = users[15];
            var ketevanJaparidze = users[16];
            var levanKharatishvili = users[17];


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
            var wellnessCategory = categories.First(c => c.Title == "Wellness");
            var networkingCategory = categories.First(c => c.Title == "Networking");

            // ============================================
            // Create FUTURE Events (2026)
            // ============================================
            var events = new Event[]
            {
                // January 2026
                new Event
                {
                    Title = "New Year Kickoff Meeting",
                    Description = "Start the year with team alignment and goal setting",
                    EventTypeId = hybridType.Id,
                    CategoryId = socialCategory.Id,
                    StartDateTime = new DateTime(2026, 1, 5, 10, 0, 0),
                    EndDateTime = new DateTime(2026, 1, 5, 12, 0, 0),
                    RegistrationDeadline = new DateTime(2026, 1, 3, 23, 59, 59),
                    Location = "Main Office Building",
                    VenueName = "Grand Conference Room",
                    MinCapacity = 30,
                    MaxCapacity = 100,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 20,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1540575467063-178a50c2df87",
                    CreatedById = adminUser.Id
                },
                new Event
                {
                    Title = "Python for Beginners Workshop",
                    Description = "Learn Python programming from scratch",
                    EventTypeId = virtualType.Id,
                    CategoryId = workshopCategory.Id,
                    StartDateTime = new DateTime(2026, 1, 12, 14, 0, 0),
                    EndDateTime = new DateTime(2026, 1, 12, 17, 0, 0),
                    RegistrationDeadline = new DateTime(2026, 1, 10, 23, 59, 59),
                    Location = "Online",
                    VenueName = "Zoom Virtual Room",
                    MinCapacity = 10,
                    MaxCapacity = 50,
                    WaitlistEnabled = false,
                    WaitlistCapacity = null,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1526374965328-7f61d4dc18c5",
                    CreatedById = organizerUser.Id
                },
                
                // February 2026
                new Event
                {
                    Title = "Valentine's Day Team Lunch",
                    Description = "Celebrate Valentine's Day with your colleagues",
                    EventTypeId = inPersonType.Id,
                    CategoryId = socialCategory.Id,
                    StartDateTime = new DateTime(2026, 2, 14, 12, 0, 0),
                    EndDateTime = new DateTime(2026, 2, 14, 14, 0, 0),
                    RegistrationDeadline = new DateTime(2026, 2, 12, 23, 59, 59),
                    Location = "Downtown Restaurant District",
                    VenueName = "Bella Vista Restaurant",
                    MinCapacity = 20,
                    MaxCapacity = 60,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 15,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1414235077428-338989a2e8c0",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "Winter Sports Challenge",
                    Description = "Indoor sports tournament - basketball and volleyball",
                    EventTypeId = inPersonType.Id,
                    CategoryId = sportsCategory.Id,
                    StartDateTime = new DateTime(2026, 2, 21, 9, 0, 0),
                    EndDateTime = new DateTime(2026, 2, 21, 17, 0, 0),
                    RegistrationDeadline = new DateTime(2026, 2, 18, 23, 59, 59),
                    Location = "Sports Complex",
                    VenueName = "Indoor Arena",
                    MinCapacity = 40,
                    MaxCapacity = 80,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 20,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1546519638-68e109498ffc",
                    CreatedById = adminUser.Id
                },

                // March 2026
                new Event
                {
                    Title = "International Women's Day Celebration",
                    Description = "Celebrating women in tech and leadership",
                    EventTypeId = hybridType.Id,
                    CategoryId = culturalCategory.Id,
                    StartDateTime = new DateTime(2026, 3, 8, 15, 0, 0),
                    EndDateTime = new DateTime(2026, 3, 8, 18, 0, 0),
                    RegistrationDeadline = new DateTime(2026, 3, 6, 23, 59, 59),
                    Location = "Main Office Building",
                    VenueName = "Auditorium Hall",
                    MinCapacity = 50,
                    MaxCapacity = 150,
                    WaitlistEnabled = true,
                    WaitlistCapacity = null,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1573496359142-b8d87734a5a2",
                    CreatedById = adminUser.Id
                },
                new Event
                {
                    Title = "React Advanced Patterns Workshop",
                    Description = "Master advanced React patterns and best practices",
                    EventTypeId = hybridType.Id,
                    CategoryId = workshopCategory.Id,
                    StartDateTime = new DateTime(2026, 3, 15, 10, 0, 0),
                    EndDateTime = new DateTime(2026, 3, 15, 16, 0, 0),
                    RegistrationDeadline = new DateTime(2026, 3, 12, 23, 59, 59),
                    Location = "Tech Campus",
                    VenueName = "Lab Room 301",
                    MinCapacity = 15,
                    MaxCapacity = 35,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 10,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1633356122544-f134324a6cee",
                    CreatedById = organizerUser.Id
                },

                // April 2026
                new Event
                {
                    Title = "Spring Team Building Retreat",
                    Description = "Outdoor team activities in beautiful spring weather",
                    EventTypeId = inPersonType.Id,
                    CategoryId = teamBuildingCategory.Id,
                    StartDateTime = new DateTime(2026, 4, 10, 9, 0, 0),
                    EndDateTime = new DateTime(2026, 4, 10, 17, 0, 0),
                    RegistrationDeadline = new DateTime(2026, 4, 5, 23, 59, 59),
                    Location = "Mountain Resort",
                    VenueName = "Adventure Park",
                    MinCapacity = 25,
                    MaxCapacity = 60,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 15,
                    AutoApprove = false,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1528605105345-5344ea20e269",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "Mental Health & Wellness Seminar",
                    Description = "Learn techniques for stress management and work-life balance",
                    EventTypeId = virtualType.Id,
                    CategoryId = wellnessCategory.Id,
                    StartDateTime = new DateTime(2026, 4, 22, 13, 0, 0),
                    EndDateTime = new DateTime(2026, 4, 22, 15, 0, 0),
                    RegistrationDeadline = new DateTime(2026, 4, 20, 23, 59, 59),
                    Location = "Online",
                    VenueName = "Microsoft Teams",
                    MinCapacity = 20,
                    MaxCapacity = 100,
                    WaitlistEnabled = false,
                    WaitlistCapacity = null,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1506126613408-eca07ce68773",
                    CreatedById = adminUser.Id
                },

                // May 2026
                new Event
                {
                    Title = "Cloud Computing Masterclass",
                    Description = "AWS, Azure, and GCP - comprehensive cloud training",
                    EventTypeId = hybridType.Id,
                    CategoryId = trainingCategory.Id,
                    StartDateTime = new DateTime(2026, 5, 8, 9, 0, 0),
                    EndDateTime = new DateTime(2026, 5, 8, 17, 0, 0),
                    RegistrationDeadline = new DateTime(2026, 5, 5, 23, 59, 59),
                    Location = "Main Office Building",
                    VenueName = "Training Center A",
                    MinCapacity = 15,
                    MaxCapacity = 40,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 10,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1451187580459-43490279c0fa",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "Company Anniversary Gala",
                    Description = "Celebrating 10 years of innovation and success",
                    EventTypeId = inPersonType.Id,
                    CategoryId = socialCategory.Id,
                    StartDateTime = new DateTime(2026, 5, 20, 18, 0, 0),
                    EndDateTime = new DateTime(2026, 5, 20, 23, 0, 0),
                    RegistrationDeadline = new DateTime(2026, 5, 15, 23, 59, 59),
                    Location = "Grand Hotel Downtown",
                    VenueName = "Crystal Ballroom",
                    MinCapacity = 100,
                    MaxCapacity = 250,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 50,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1511795409834-ef04bbd61622",
                    CreatedById = adminUser.Id
                },

                // June 2026
                new Event
                {
                    Title = "Summer Networking BBQ",
                    Description = "Casual outdoor networking with food and drinks",
                    EventTypeId = inPersonType.Id,
                    CategoryId = networkingCategory.Id,
                    StartDateTime = new DateTime(2026, 6, 12, 17, 0, 0),
                    EndDateTime = new DateTime(2026, 6, 12, 21, 0, 0),
                    RegistrationDeadline = new DateTime(2026, 6, 10, 23, 59, 59),
                    Location = "Company Park",
                    VenueName = "Outdoor BBQ Area",
                    MinCapacity = 40,
                    MaxCapacity = 120,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 30,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1555939594-58d7cb561ad1",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "AI & Machine Learning Summit",
                    Description = "Exploring the future of AI in business",
                    EventTypeId = hybridType.Id,
                    CategoryId = workshopCategory.Id,
                    StartDateTime = new DateTime(2026, 6, 25, 9, 0, 0),
                    EndDateTime = new DateTime(2026, 6, 25, 17, 0, 0),
                    RegistrationDeadline = new DateTime(2026, 6, 22, 23, 59, 59),
                    Location = "Convention Center",
                    VenueName = "Main Hall",
                    MinCapacity = 50,
                    MaxCapacity = 200,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 50,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1677442136019-21780ecad995",
                    CreatedById = adminUser.Id
                },

                // July 2026
                new Event
                {
                    Title = "Summer Olympics Watch Party",
                    Description = "Watch Olympic games together with snacks and drinks",
                    EventTypeId = inPersonType.Id,
                    CategoryId = socialCategory.Id,
                    StartDateTime = new DateTime(2026, 7, 10, 19, 0, 0),
                    EndDateTime = new DateTime(2026, 7, 10, 23, 0, 0),
                    RegistrationDeadline = new DateTime(2026, 7, 8, 23, 59, 59),
                    Location = "Main Office Building",
                    VenueName = "Entertainment Lounge",
                    MinCapacity = 30,
                    MaxCapacity = 80,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 20,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1461896836934-ffe607ba8211",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "Yoga & Meditation Morning",
                    Description = "Start your day with mindfulness and wellness",
                    EventTypeId = inPersonType.Id,
                    CategoryId = wellnessCategory.Id,
                    StartDateTime = new DateTime(2026, 7, 18, 7, 0, 0),
                    EndDateTime = new DateTime(2026, 7, 18, 9, 0, 0),
                    RegistrationDeadline = new DateTime(2026, 7, 16, 23, 59, 59),
                    Location = "Wellness Center",
                    VenueName = "Meditation Room",
                    MinCapacity = 10,
                    MaxCapacity = 30,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 10,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1544367567-0f2fcb009e0b",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "Q1 2024 Strategy Offsite",
                    Description = "Leadership and planning offsite (historical data)",
                    EventTypeId = inPersonType.Id,
                    CategoryId = trainingCategory.Id,
                    StartDateTime = new DateTime(2024, 3, 15, 10, 0, 0),
                    EndDateTime = new DateTime(2024, 3, 15, 17, 0, 0),
                    RegistrationDeadline = new DateTime(2024, 3, 10, 23, 59, 59),
                    Location = "Gudauri",
                    VenueName = "Mountain Lodge",
                    MinCapacity = 10,
                    MaxCapacity = 40,
                    WaitlistEnabled = false,
                    WaitlistCapacity = null,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1528605105345-5344ea20e269",
                    CreatedById = adminUser.Id
                },
                new Event
                {
                    Title = "Summer 2024 Company Picnic",
                    Description = "Outdoor picnic for all departments (historical data)",
                    EventTypeId = inPersonType.Id,
                    CategoryId = socialCategory.Id,
                    StartDateTime = new DateTime(2024, 7, 20, 12, 0, 0),
                    EndDateTime = new DateTime(2024, 7, 20, 18, 0, 0),
                    RegistrationDeadline = new DateTime(2024, 7, 15, 23, 59, 59),
                    Location = "City Park",
                    VenueName = "Picnic Zone A",
                    MinCapacity = 30,
                    MaxCapacity = 200,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 30,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1555939594-58d7cb561ad1",
                    CreatedById = organizerUser.Id
                },

                // Past: 2025 
                new Event
                {
                    Title = "January 2025 New Joiners Orientation",
                    Description = "Onboarding and culture introduction (historical data)",
                    EventTypeId = hybridType.Id,
                    CategoryId = trainingCategory.Id,
                    StartDateTime = new DateTime(2025, 1, 10, 11, 0, 0),
                    EndDateTime = new DateTime(2025, 1, 10, 13, 0, 0),
                    RegistrationDeadline = new DateTime(2025, 1, 8, 23, 59, 59),
                    Location = "Main Office / Online",
                    VenueName = "Orientation Room",
                    MinCapacity = 5,
                    MaxCapacity = 60,
                    WaitlistEnabled = false,
                    WaitlistCapacity = null,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1540575467063-178a50c2df87",
                    CreatedById = organizerUser.Id
                },
                new Event
                {
                    Title = "September 2025 Sales Bootcamp",
                    Description = "Intensive sales training (historical data)",
                    EventTypeId = inPersonType.Id,
                    CategoryId = trainingCategory.Id,
                    StartDateTime = new DateTime(2025, 9, 5, 9, 30, 0),
                    EndDateTime = new DateTime(2025, 9, 5, 16, 30, 0),
                    RegistrationDeadline = new DateTime(2025, 9, 1, 23, 59, 59),
                    Location = "Training Hub",
                    VenueName = "Room B",
                    MinCapacity = 10,
                    MaxCapacity = 50,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 10,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1451187580459-43490279c0fa",
                    CreatedById = adminUser.Id
                },
                new Event
                {
                    Title = "November 2025 Tech Meetup",
                    Description = "Internal tech talks and networking (historical data)",
                    EventTypeId = hybridType.Id,
                    CategoryId = networkingCategory.Id,
                    StartDateTime = new DateTime(2025, 11, 18, 18, 0, 0),
                    EndDateTime = new DateTime(2025, 11, 18, 20, 30, 0),
                    RegistrationDeadline = new DateTime(2025, 11, 16, 23, 59, 59),
                    Location = "Main Office / Online",
                    VenueName = "Auditorium",
                    MinCapacity = 20,
                    MaxCapacity = 120,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 20,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1677442136019-21780ecad995",
                    CreatedById = adminUser.Id
                },

                // Dec 2025
                new Event
                {
                    Title = "December 2025 Year-End Review",
                    Description = "Wrap-up, KPIs, and next steps (near-current data)",
                    EventTypeId = hybridType.Id,
                    CategoryId = socialCategory.Id,
                    StartDateTime = new DateTime(2025, 12, 26, 16, 0, 0),
                    EndDateTime = new DateTime(2025, 12, 26, 18, 0, 0),
                    RegistrationDeadline = new DateTime(2025, 12, 25, 23, 59, 59),
                    Location = "Main Office / Online",
                    VenueName = "Grand Conference Room",
                    MinCapacity = 30,
                    MaxCapacity = 150,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 25,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1511795409834-ef04bbd61622",
                    CreatedById = organizerUser.Id
                },

                // 2026 / 2027 
                new Event
                {
                    Title = "Autumn 2026 Leadership Workshop",
                    Description = "Leadership skills and communication training (future data)",
                    EventTypeId = inPersonType.Id,
                    CategoryId = workshopCategory.Id,
                    StartDateTime = new DateTime(2026, 10, 9, 10, 0, 0),
                    EndDateTime = new DateTime(2026, 10, 9, 17, 0, 0),
                    RegistrationDeadline = new DateTime(2026, 10, 5, 23, 59, 59),
                    Location = "Leadership Center",
                    VenueName = "Hall 2",
                    MinCapacity = 12,
                    MaxCapacity = 45,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 10,
                    AutoApprove = false,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1526374965328-7f61d4dc18c5",
                    CreatedById = adminUser.Id
                },
                new Event
                {
                    Title = "June 2027 Innovation Hackathon",
                    Description = "48-hour hackathon for new product ideas (future data)",
                    EventTypeId = hybridType.Id,
                    CategoryId = teamBuildingCategory.Id,
                    StartDateTime = new DateTime(2027, 6, 18, 9, 0, 0),
                    EndDateTime = new DateTime(2027, 6, 19, 18, 0, 0),
                    RegistrationDeadline = new DateTime(2027, 6, 10, 23, 59, 59),
                    Location = "Tech Campus / Online",
                    VenueName = "Innovation Lab",
                    MinCapacity = 20,
                    MaxCapacity = 200,
                    WaitlistEnabled = true,
                    WaitlistCapacity = 50,
                    AutoApprove = true,
                    IsVisible = true,
                    ImageUrl = "https://images.unsplash.com/photo-1633356122544-f134324a6cee",
                    CreatedById = organizerUser.Id
                },
            };
            context.Events.AddRange(events);
            context.SaveChanges();

            // ============================================
            // Add Speakers
            // ============================================
            var speakers = new Speaker[]
            {
                // Python Workshop
                new Speaker
                {
                    EventId = events[1].Id,
                    Name = "Dr. Sarah Chen",
                    Role = "Python Expert & Data Scientist",
                    PhotoUrl = "https://i.pravatar.cc/150?img=1"
                },
                
                // React Workshop
                new Speaker
                {
                    EventId = events[5].Id,
                    Name = "Alex Johnson",
                    Role = "Senior React Developer",
                    PhotoUrl = "https://i.pravatar.cc/150?img=12"
                },
                new Speaker
                {
                    EventId = events[5].Id,
                    Name = "Maria Rodriguez",
                    Role = "Frontend Architect",
                    PhotoUrl = "https://i.pravatar.cc/150?img=5"
                },
                
                // Cloud Computing
                new Speaker
                {
                    EventId = events[8].Id,
                    Name = "David Kim",
                    Role = "Cloud Solutions Architect",
                    PhotoUrl = "https://i.pravatar.cc/150?img=13"
                },
                
                // AI Summit
                new Speaker
                {
                    EventId = events[11].Id,
                    Name = "Dr. Emily Watson",
                    Role = "AI Research Lead",
                    PhotoUrl = "https://i.pravatar.cc/150?img=9"
                },
                new Speaker
                {
                    EventId = events[11].Id,
                    Name = "Michael Zhang",
                    Role = "ML Engineer",
                    PhotoUrl = "https://i.pravatar.cc/150?img=14"
                },
                
                // Wellness Seminar
                new Speaker
                {
                    EventId = events[7].Id,
                    Name = "Lisa Thompson",
                    Role = "Wellness Coach",
                    PhotoUrl = "https://i.pravatar.cc/150?img=10"
                }
            };
            context.Speakers.AddRange(speakers);
            context.SaveChanges();

            // ============================================
            // Add Agenda Items
            // ============================================
            var agendaItems = new AgendaItem[]
            {
                // Python Workshop
                new AgendaItem { EventId = events[1].Id, Time = "14:00-14:30", Title = "Introduction to Python", Description = "Basics and setup" },
                new AgendaItem { EventId = events[1].Id, Time = "14:30-15:30", Title = "Python Fundamentals", Description = "Variables, loops, functions" },
                new AgendaItem { EventId = events[1].Id, Time = "15:30-16:30", Title = "Hands-on Coding", Description = "Build your first Python program" },
                new AgendaItem { EventId = events[1].Id, Time = "16:30-17:00", Title = "Q&A Session", Description = "Ask your questions" },
                
                // React Workshop
                new AgendaItem { EventId = events[5].Id, Time = "10:00-11:00", Title = "Advanced Hooks", Description = "useCallback, useMemo, custom hooks" },
                new AgendaItem { EventId = events[5].Id, Time = "11:00-12:30", Title = "State Management", Description = "Context API and Redux patterns" },
                new AgendaItem { EventId = events[5].Id, Time = "12:30-13:30", Title = "Lunch Break", Description = null },
                new AgendaItem { EventId = events[5].Id, Time = "13:30-15:00", Title = "Performance Optimization", Description = "React.memo, lazy loading" },
                new AgendaItem { EventId = events[5].Id, Time = "15:00-16:00", Title = "Best Practices", Description = "Code organization and patterns" },
                
                // Team Building
                new AgendaItem { EventId = events[6].Id, Time = "09:00-09:30", Title = "Welcome & Icebreakers", Description = "Get to know each other" },
                new AgendaItem { EventId = events[6].Id, Time = "09:30-12:00", Title = "Team Challenges", Description = "Problem-solving activities" },
                new AgendaItem { EventId = events[6].Id, Time = "12:00-13:00", Title = "Lunch", Description = "BBQ lunch provided" },
                new AgendaItem { EventId = events[6].Id, Time = "13:00-16:00", Title = "Outdoor Activities", Description = "Sports and games" },
                new AgendaItem { EventId = events[6].Id, Time = "16:00-17:00", Title = "Wrap-up", Description = "Team reflections" },
                
                // Cloud Computing
                new AgendaItem { EventId = events[8].Id, Time = "09:00-10:30", Title = "AWS Fundamentals", Description = "EC2, S3, Lambda" },
                new AgendaItem { EventId = events[8].Id, Time = "10:30-12:00", Title = "Azure Overview", Description = "VMs, Storage, Functions" },
                new AgendaItem { EventId = events[8].Id, Time = "12:00-13:00", Title = "Lunch Break", Description = null },
                new AgendaItem { EventId = events[8].Id, Time = "13:00-14:30", Title = "GCP Essentials", Description = "Compute Engine, Cloud Storage" },
                new AgendaItem { EventId = events[8].Id, Time = "14:30-16:00", Title = "Multi-Cloud Strategy", Description = "Best practices" },
                new AgendaItem { EventId = events[8].Id, Time = "16:00-17:00", Title = "Hands-on Lab", Description = "Deploy a real application" },
                
                // AI Summit
                new AgendaItem { EventId = events[11].Id, Time = "09:00-10:00", Title = "Keynote: Future of AI", Description = "Industry trends and predictions" },
                new AgendaItem { EventId = events[11].Id, Time = "10:00-12:00", Title = "ML in Business", Description = "Real-world case studies" },
                new AgendaItem { EventId = events[11].Id, Time = "12:00-13:00", Title = "Networking Lunch", Description = null },
                new AgendaItem { EventId = events[11].Id, Time = "13:00-15:00", Title = "Workshop: Building ML Models", Description = "Hands-on training" },
                new AgendaItem { EventId = events[11].Id, Time = "15:00-17:00", Title = "Panel Discussion", Description = "Ethics in AI" }
            };
            context.AgendaItems.AddRange(agendaItems);
            context.SaveChanges();

            // ============================================
            // Add Event Tags
            // ============================================
            var eventTags = new List<EventTag>
            {
                // New Year Kickoff
                new EventTag { EventId = events[0].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[0].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                // Python Workshop
                new EventTag { EventId = events[1].Id, TagId = tags.First(t => t.Name == "learning").Id },
                new EventTag { EventId = events[1].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                // Valentine's Lunch
                new EventTag { EventId = events[2].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[2].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[2].Id, TagId = tags.First(t => t.Name == "networking").Id },
                
                // Winter Sports
                new EventTag { EventId = events[3].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[3].Id, TagId = tags.First(t => t.Name == "wellness").Id },
                
                // Women's Day
                new EventTag { EventId = events[4].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[4].Id, TagId = tags.First(t => t.Name == "networking").Id },
                new EventTag { EventId = events[4].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                // React Workshop
                new EventTag { EventId = events[5].Id, TagId = tags.First(t => t.Name == "learning").Id },
                new EventTag { EventId = events[5].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                // Team Building
                new EventTag { EventId = events[6].Id, TagId = tags.First(t => t.Name == "outdoor").Id },
                new EventTag { EventId = events[6].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[6].Id, TagId = tags.First(t => t.Name == "family-friendly").Id },
                
                // Wellness Seminar
                new EventTag { EventId = events[7].Id, TagId = tags.First(t => t.Name == "wellness").Id },
                new EventTag { EventId = events[7].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                // Cloud Computing
                new EventTag { EventId = events[8].Id, TagId = tags.First(t => t.Name == "learning").Id },
                new EventTag { EventId = events[8].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                // Anniversary Gala
                new EventTag { EventId = events[9].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[9].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[9].Id, TagId = tags.First(t => t.Name == "networking").Id },
                
                // Summer BBQ
                new EventTag { EventId = events[10].Id, TagId = tags.First(t => t.Name == "outdoor").Id },
                new EventTag { EventId = events[10].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                new EventTag { EventId = events[10].Id, TagId = tags.First(t => t.Name == "networking").Id },
                
                // AI Summit
                new EventTag { EventId = events[11].Id, TagId = tags.First(t => t.Name == "learning").Id },
                new EventTag { EventId = events[11].Id, TagId = tags.First(t => t.Name == "networking").Id },
                new EventTag { EventId = events[11].Id, TagId = tags.First(t => t.Name == "remote-friendly").Id },
                
                // Olympics Watch Party
                new EventTag { EventId = events[12].Id, TagId = tags.First(t => t.Name == "indoor").Id },
                new EventTag { EventId = events[12].Id, TagId = tags.First(t => t.Name == "free-food").Id },
                
                // Yoga Morning
                new EventTag { EventId = events[13].Id, TagId = tags.First(t => t.Name == "wellness").Id },
                new EventTag { EventId = events[13].Id, TagId = tags.First(t => t.Name == "indoor").Id }
            };
            context.EventTags.AddRange(eventTags);
            context.SaveChanges();

            // ============================================
            // Add Sample Registrations
            // ============================================
            var confirmedStatus = statuses.First(s => s.Name == "Confirmed");
            var waitlistedStatus = statuses.First(s => s.Name == "Waitlisted");

            var registrations = new Registration[]
            {
                // Some confirmed registrations
                new Registration { EventId = events[0].Id, UserId = johnDoe.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddDays(-5) },
                new Registration { EventId = events[0].Id, UserId = janeSmith.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddDays(-4) },
                new Registration { EventId = events[0].Id, UserId = mikeWilson.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddDays(-3) },

                new Registration { EventId = events[1].Id, UserId = johnDoe.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddDays(-2) },
                new Registration { EventId = events[1].Id, UserId = janeSmith.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddDays(-1) },

                new Registration { EventId = events[2].Id, UserId = mikeWilson.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddDays(-3) },
                new Registration { EventId = events[2].Id, UserId = johnDoe.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddDays(-2) },
                new Registration { EventId = events[2].Id, UserId = janeSmith.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddDays(-1) },

                new Registration { EventId = events[5].Id, UserId = janeSmith.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddDays(-6) },
                new Registration { EventId = events[5].Id, UserId = mikeWilson.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddDays(-5) },
                // new
                new Registration { EventId = events[11].Id, UserId = johnDoe.Id, StatusId = waitlistedStatus.Id, RegisteredAt = DateTime.UtcNow.AddDays(-1) },
                  new Registration { EventId = events[0].Id, UserId = manoTiler.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddMonths(-2).AddDays(-3) },
                  new Registration { EventId = events[1].Id, UserId = shakoGernava.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddMonths(-1).AddDays(-7) },
                new Registration { EventId = events[2].Id, UserId = lileJanjgava.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddMonths(1).AddDays(2)},

                new Registration { EventId = events[0].Id, UserId = ninoKiknadze.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddMonths(-2).AddDays(-2) },
                new Registration { EventId = events[0].Id, UserId = giorgiBeridze.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddMonths(-2).AddDays(-8) },
                new Registration { EventId = events[1].Id, UserId = mariamGelashvili.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddMonths(-2).AddDays(-15) },
                new Registration { EventId = events[2].Id, UserId = lukaTsereteli.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddMonths(-2).AddDays(-20) },

                new Registration { EventId = events[1].Id, UserId = anaKapanadze.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddMonths(-1).AddDays(-1) },
                new Registration { EventId = events[2].Id, UserId = datoKoberidze.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddMonths(-1).AddDays(-6) },
                new Registration { EventId = events[0].Id, UserId = salomeChikvaidze.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddMonths(-1).AddDays(-12) },

                new Registration { EventId = events[2].Id, UserId = irakliMchedlishvili.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddMonths(1).AddDays(1) },
                new Registration { EventId = events[0].Id, UserId = ketevanJaparidze.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddMonths(1).AddDays(5) },
                new Registration { EventId = events[1].Id, UserId = levanKharatishvili.Id, StatusId = confirmedStatus.Id, RegisteredAt = DateTime.UtcNow.AddMonths(1).AddDays(10) },
              };
            context.Registrations.AddRange(registrations);
            context.SaveChanges();
        }
    }
}