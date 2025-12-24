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
        public DbSet<Department> Departments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<EventTag> EventTags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<AgendaItem> AgendaItems { get; set; }

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

                entity.Property(e => e.VenueName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.IsVisible)
                    .HasDefaultValue(true);

                entity.Property(e => e.WaitlistEnabled)
                    .HasDefaultValue(true);

                entity.Property(e => e.AutoApprove)
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

                // One-to-Many: Categories -> Events
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Events)
                    .HasForeignKey(e => e.CategoryId)
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
                entity.HasIndex(e => e.CategoryId);
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

                entity.Property(u => u.Password)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(u => u.PhoneNumber)
                    .HasMaxLength(20); // NEW - Optional phone number

                entity.Property(u => u.IsAdmin)
                    .HasDefaultValue(false);

                entity.Property(u => u.IsActive)
                    .HasDefaultValue(true);

                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // One-to-Many: Departments -> Users
                entity.HasOne(u => u.Department)
                    .WithMany(d => d.Users)
                    .HasForeignKey(u => u.DepartmentId)
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
            // CATEGORY CONFIGURATION
            // ============================================
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                // Unique index on Title
                entity.HasIndex(c => c.Title)
                    .IsUnique();
            });

            // ============================================
            // SPEAKER CONFIGURATION
            // ============================================
            modelBuilder.Entity<Speaker>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(s => s.Role)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(s => s.PhotoUrl)
                    .HasMaxLength(500);

                // One-to-Many: Events -> Speakers
                entity.HasOne(s => s.Event)
                    .WithMany(e => e.Speakers)
                    .HasForeignKey(s => s.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Index for querying by event
                entity.HasIndex(s => s.EventId);
            });

            // ============================================
            // AGENDA ITEM CONFIGURATION
            // ============================================
            modelBuilder.Entity<AgendaItem>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Time)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(a => a.Title)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(a => a.Description)
                    .HasMaxLength(1000);

                // One-to-Many: Events -> AgendaItems
                entity.HasOne(a => a.Event)
                    .WithMany(e => e.AgendaItems)
                    .HasForeignKey(a => a.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Index for querying by event
                entity.HasIndex(a => a.EventId);
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
            });

            // ============================================
            // DEPARTMENT CONFIGURATION
            // ============================================
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(d => d.Description)
                    .HasMaxLength(500);

                entity.Property(d => d.IsActive)
                    .HasDefaultValue(true);

                entity.HasIndex(d => d.Name)
                    .IsUnique();
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
            });
        }
    }
}