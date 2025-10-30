using Microsoft.EntityFrameworkCore;
using VeilingAPI.Models;

namespace VeilingAPI.Data
{
    public class VeilingDbContext : DbContext
    {
        public VeilingDbContext(DbContextOptions<VeilingDbContext> options) : base(options)
        {
        }

        public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<Verkoper> Verkopers { get; set; }
        public DbSet<Koper> Kopers { get; set; }
        public DbSet<Product> Producten { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Table Per Hierarchy (TPH) inheritance
            // BELANGRIJKE OPMERKING: Gebruiker is nu abstract, dus geen "Gebruiker" discriminator waarde
            modelBuilder.Entity<Gebruiker>()
                .HasDiscriminator<string>("gebruiker_type")
                .HasValue<Verkoper>("Verkoper")
                .HasValue<Koper>("Koper");

            // Configure table name
            modelBuilder.Entity<Gebruiker>()
                .ToTable("Gebruikers");

            // Configure Verkoper to use shared columns (no shadow properties)
            modelBuilder.Entity<Verkoper>()
                .Property(v => v.bedrijfsnaam).HasColumnName("bedrijfsnaam");
            modelBuilder.Entity<Verkoper>()
                .Property(v => v.kvk).HasColumnName("kvk");
            modelBuilder.Entity<Verkoper>()
                .Property(v => v.btw_nummer).HasColumnName("btw_nummer");
            modelBuilder.Entity<Verkoper>()
                .Property(v => v.telefoon).HasColumnName("telefoon");
            modelBuilder.Entity<Verkoper>()
                .Property(v => v.rekening_nummer).HasColumnName("rekening_nummer");

            // Configure Koper to use same shared columns
            modelBuilder.Entity<Koper>()
                .Property(k => k.bedrijfsnaam).HasColumnName("bedrijfsnaam");
            modelBuilder.Entity<Koper>()
                .Property(k => k.kvk).HasColumnName("kvk");
            modelBuilder.Entity<Koper>()
                .Property(k => k.btw_nummer).HasColumnName("btw_nummer");
            modelBuilder.Entity<Koper>()
                .Property(k => k.telefoon).HasColumnName("telefoon");
            modelBuilder.Entity<Koper>()
                .Property(k => k.rekening_nummer).HasColumnName("rekening_nummer");

            // Configure Product
            modelBuilder.Entity<Product>()
                .ToTable("Producten");
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Verkoper)
                .WithMany()
                .HasForeignKey(p => p.verkoper_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure indexes for better performance
            modelBuilder.Entity<Gebruiker>()
                .HasIndex(u => u.email)
                .IsUnique();
        }
    }
}
