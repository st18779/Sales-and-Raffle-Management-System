using server.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace server.Data
{
    public class ChineseSaleContext: DbContext
    {
        public ChineseSaleContext(DbContextOptions<ChineseSaleContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Purchases> purchases { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(10);
                entity.Property(e=> e.Password).IsRequired();
                entity.Property(e => e.IsAdmin).IsRequired();
            });

            modelBuilder.Entity<Gift>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.IsRandomized).IsRequired();
                entity.Property(e => e.ImageUrl).IsRequired();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                //entity.HasMany(e => e.Gifts)
                //       .WithOne(e => e.Category)
            });

            modelBuilder.Entity<Purchases>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NumOfTickets).IsRequired();
                entity.Property(e => e.IsClosed).IsRequired();
                entity.Property(e => e.PurchaseDate).IsRequired().HasDefaultValue(DateTime.Now.Date);
            });


        }

    }
}
