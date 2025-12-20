using Microsoft.EntityFrameworkCore;
using Pied_Piper.Models;

namespace Pied_Piper.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet properties for all entities
        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<RegistrationStatus> RegistrationStatuses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<EventTag> EventTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================================
            // EVENT CONFIGURATION
            // ============================================
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasColumnType("nvarchar(MAX)");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // One-to-Many: EventTypes -> Events
                entity.HasOne(e => e.EventType)
                    .WithMany(et => et.Events)
                    .HasForeignKey(e => e.EventTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                // One-to-Many: Users -> Events (CreatedBy)
                entity.HasOne(e => e.CreatedBy)
                    .WithMany(u => u.CreatedEvents)
                    .HasForeignKey(e => e.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes for performance
                entity.HasIndex(e => new { e.EventTypeId, e.StartDateTime, e.IsActive });
                entity.HasIndex(e => e.StartDateTime)
                    .HasFilter("[IsActive] = 1");
            });

            // ============================================
            // REGISTRATION CONFIGURATION
            // ============================================
            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.RegisteredAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // One-to-Many: Events -> Registrations
                entity.HasOne(r => r.Event)
                    .WithMany(e => e.Registrations)
                    .HasForeignKey(r => r.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                // One-to-Many: Users -> Registrations
                entity.HasOne(r => r.User)
                    .WithMany(u => u.Registrations)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // One-to-Many: RegistrationStatuses -> Registrations
                entity.HasOne(r => r.Status)
                    .WithMany(rs => rs.Registrations)
                    .HasForeignKey(r => r.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes for performance
                entity.HasIndex(r => new { r.EventId, r.StatusId });

                // Unique index: One active registration per user per event
                entity.HasIndex(r => new { r.EventId, r.UserId })
                    .IsUnique()
                    .HasFilter("[StatusId] <> 3"); // Not cancelled
            });

            // ============================================
            // USER CONFIGURATION
            // ============================================
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(u => u.FullName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(u => u.IsActive)
                    .HasDefaultValue(true);

                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // One-to-Many: Roles -> Users
                entity.HasOne(u => u.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Unique index on Email
                entity.HasIndex(u => u.Email)
                    .IsUnique();
            });

            // ============================================
            // EVENT TAG CONFIGURATION (Many-to-Many)
            // ============================================
            modelBuilder.Entity<EventTag>(entity =>
            {
                entity.HasKey(et => et.Id);

                // Many-to-Many: Events <-> Tags through EventTags
                entity.HasOne(et => et.Event)
                    .WithMany(e => e.EventTags)
                    .HasForeignKey(et => et.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(et => et.Tag)
                    .WithMany(t => t.EventTags)
                    .HasForeignKey(et => et.TagId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Unique constraint on (EventId, TagId)
                entity.HasIndex(et => new { et.EventId, et.TagId })
                    .IsUnique();

                // Index on TagId for performance
                entity.HasIndex(et => et.TagId);
            });

            // ============================================
            // EVENT TYPE CONFIGURATION
            // ============================================
            modelBuilder.Entity<EventType>(entity =>
            {
                entity.HasKey(et => et.Id);

                entity.Property(et => et.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(et => et.Description)
                    .HasMaxLength(500);

                entity.Property(et => et.IsActive)
                    .HasDefaultValue(true);

                entity.HasIndex(et => et.Name)
                    .IsUnique();

                // Seed data
                entity.HasData(
                    new EventType { Id = 1, Name = "Team Building", Description = "Team building activities", IsActive = true },
                    new EventType { Id = 2, Name = "Sports", Description = "Sports events", IsActive = true },
                    new EventType { Id = 3, Name = "Workshop", Description = "Educational workshops", IsActive = true },
                    new EventType { Id = 4, Name = "Happy Friday", Description = "Friday celebrations", IsActive = true },
                    new EventType { Id = 5, Name = "Cultural", Description = "Cultural events", IsActive = true },
                    new EventType { Id = 6, Name = "Training", Description = "Training sessions", IsActive = true },
                    new EventType { Id = 7, Name = "Social", Description = "Social gatherings", IsActive = true }
                );
            });

            // ============================================
            // REGISTRATION STATUS CONFIGURATION
            // ============================================
            modelBuilder.Entity<RegistrationStatus>(entity =>
            {
                entity.HasKey(rs => rs.Id);

                entity.Property(rs => rs.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(rs => rs.Description)
                    .HasMaxLength(200);

                entity.HasIndex(rs => rs.Name)
                    .IsUnique();

                // Seed data
                entity.HasData(
                    new RegistrationStatus { Id = 1, Name = "Confirmed", Description = "Registration confirmed" },
                    new RegistrationStatus { Id = 2, Name = "Waitlisted", Description = "On waiting list" },
                    new RegistrationStatus { Id = 3, Name = "Cancelled", Description = "Registration cancelled" }
                );
            });

            // ============================================
            // ROLE CONFIGURATION
            // ============================================
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(r => r.Description)
                    .HasMaxLength(200);

                entity.HasIndex(r => r.Name)
                    .IsUnique();

                // Seed data
                entity.HasData(
                    new Role { Id = 1, Name = "Employee", Description = "Regular employee" },
                    new Role { Id = 2, Name = "Organizer", Description = "Event organizer" },
                    new Role { Id = 3, Name = "Admin", Description = "System administrator" }
                );
            });

            // ============================================
            // TAG CONFIGURATION
            // ============================================
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(t => t.Category)
                    .HasMaxLength(50);

                entity.HasIndex(t => t.Name)
                    .IsUnique();

                // Seed data
                entity.HasData(
                    new Tag { Id = 1, Name = "outdoor", Category = "location" },
                    new Tag { Id = 2, Name = "indoor", Category = "location" },
                    new Tag { Id = 3, Name = "free", Category = "cost" },
                    new Tag { Id = 4, Name = "learning", Category = "type" },
                    new Tag { Id = 5, Name = "wellness", Category = "type" },
                    new Tag { Id = 6, Name = "food", Category = "amenity" },
                    new Tag { Id = 7, Name = "remote friendly", Category = "accessibility" },
                    new Tag { Id = 8, Name = "family friendly", Category = "accessibility" },
                    new Tag { Id = 9, Name = "networking", Category = "type" }
                );
            });
        }
    }

}
