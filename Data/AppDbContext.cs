using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Turbo_Food_Main.Models;

namespace Turbo_Food_Main.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // New DbSets
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure MenuItem entity
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasKey(e => e.ItemId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");

                // Add index for better query performance
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.Availability);
            });

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderID);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.SpecialInstructions).HasMaxLength(200);

                // Relationships
                entity.HasOne(o => o.User)
                      .WithMany()
                      .HasForeignKey(o => o.UserID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure OrderItem entity
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.OrderItemID);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");

                // Relationships
                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(oi => oi.OrderID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.MenuItem)
                      .WithMany()
                      .HasForeignKey(oi => oi.MealID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Vendor entity
            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasKey(e => e.VendorID);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNum).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(255);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                // Unique constraint for email
                entity.HasIndex(v => v.Email).IsUnique();
            });

            // Configure Feedback entity
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(e => e.FeedbackID);
                entity.Property(e => e.Rating).IsRequired();
                entity.Property(e => e.Message).HasMaxLength(1000);
                entity.Property(e => e.DateSubmitted).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.MenuItemId).IsRequired(false);

                // Check constraint for rating
                entity.HasCheckConstraint("CK_Feedback_Rating", "[Rating] BETWEEN 1 AND 5");

                // Relationships
                entity.HasOne(f => f.User)
                      .WithMany()
                      .HasForeignKey(f => f.UserID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(f => f.Order)
                      .WithMany()
                      .HasForeignKey(f => f.OrderID)
                      .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(f => f.MenuItem)
                .WithMany()
                .HasForeignKey(f => f.MenuItemId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Payment entity
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentID);
                entity.Property(e => e.AmountPaid).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PaymentDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.TransactionID).HasMaxLength(100);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("Pending");

                // Check constraints
                entity.HasCheckConstraint("CK_Payment_AmountPaid", "[AmountPaid] > 0");
                entity.HasCheckConstraint("CK_Payment_Status",
                    "[Status] IN ('Pending', 'Completed', 'Failed', 'Refunded')");

                // Relationships
                entity.HasOne(p => p.Order)
                      .WithMany()
                      .HasForeignKey(p => p.OrderID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.User)
                      .WithMany()
                      .HasForeignKey(p => p.UserID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // REMOVED HasData seeding to avoid migration errors
        }
    }
}