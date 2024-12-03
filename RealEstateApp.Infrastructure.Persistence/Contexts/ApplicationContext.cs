using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RealEstateApp.Core.Domain.Common;
using RealEstateApp.Core.Domain.Entities;
namespace RealEstateApp.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        #region DbSets
        public DbSet<Property> Properties { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Improvement> Improvements { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<ImprovementProperty> ImprovementProperties { get; set; }
        public DbSet<SaleType> SaleTypes { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        #endregion
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultAppUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = "DefaultAppUser";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración Fluent API

            #region Tables
            modelBuilder.Entity<Property>().ToTable("Properties");
            modelBuilder.Entity<Image>().ToTable("Images");
            modelBuilder.Entity<Improvement>().ToTable("Improvements");
            modelBuilder.Entity<Offer>().ToTable("Offers");
            modelBuilder.Entity<PropertyType>().ToTable("PropertyTypes");
            modelBuilder.Entity<SaleType>().ToTable("SaleTypes");
            modelBuilder.Entity<Chat>().ToTable("Chats");
            modelBuilder.Entity<Favorite>().ToTable("Favorites");
            #endregion

            #region Primary Keys
            modelBuilder.Entity<Property>().HasKey(p => p.Id);
            modelBuilder.Entity<Image>().HasKey(i => i.Id);
            modelBuilder.Entity<Improvement>().HasKey(im => im.Id);
            modelBuilder.Entity<Offer>().HasKey(o => o.Id);
            modelBuilder.Entity<PropertyType>().HasKey(pt => pt.Id);
            modelBuilder.Entity<SaleType>().HasKey(st => st.Id);
            modelBuilder.Entity<Chat>().HasKey(c => c.Id);
            modelBuilder.Entity<Favorite>().HasKey(f => f.Id);
            #endregion

            #region Relationships
            modelBuilder.Entity<Property>()
                 .HasMany(p => p.Images)
                 .WithOne(i => i.Property)
                 .HasForeignKey(i => i.PropertyId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Property>()
                .HasMany(p => p.Improvements)
                .WithMany(im => im.Properties)
                .UsingEntity<ImprovementProperty>(
                    j => j.HasOne(ip => ip.Improvement)
                            .WithMany()
                            .HasForeignKey(ip => ip.ImprovementId),
                    j => j.HasOne(ip => ip.Property)
                            .WithMany()
                            .HasForeignKey(ip => ip.PropertyId),
                    j =>
                    {
                        j.HasKey(t => new { t.PropertyId, t.ImprovementId });
                    });

            modelBuilder.Entity<PropertyType>()
                .HasMany(pt => pt.Properties)
                .WithOne(p => p.PropertyType)
                .HasForeignKey(p => p.PropertyTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SaleType>()
                .HasMany(st => st.Properties)
                .WithOne(p => p.SaleType)
                .HasForeignKey(p => p.SaleTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Offer>()
                .HasOne(o => o.Property)
                .WithMany()
                .HasForeignKey(o => o.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Property)
                .WithMany()
                .HasForeignKey(f => f.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Property)
                .WithMany()
                .HasForeignKey(c => c.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Configuration Propierties
            modelBuilder.Entity<Property>()
                .Property(p => p.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            modelBuilder.Entity<Property>()
                .Property(p => p.PropertySizeMeters)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Offer>()
                .Property(o => o.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            modelBuilder.Entity<Property>()
                .Property(p => p.PropertyCode)
                .IsRequired()
                .HasMaxLength(6);

            modelBuilder.Entity<Property>()
                .Property(p => p.Description)
                .HasMaxLength(1000);

            modelBuilder.Entity<PropertyType>()
                .Property(pt => pt.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<SaleType>()
                .Property(st => st.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Improvement>()
                .Property(im => im.Name)
                .IsRequired()
                .HasMaxLength(200);
            #endregion
        }
    }
}
