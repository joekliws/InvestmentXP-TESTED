using Investment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment.Infra.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAsset> UserAssets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAsset>().HasKey(bc => new { bc.UserId, bc.AssetId });

            modelBuilder.Entity<UserAsset>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.Assets)
                .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<UserAsset>()
                .HasOne(ua => ua.Asset)
                .WithMany(u => u.Assets)
                .HasForeignKey(ua => ua.AssetId);
         
        }
    }
}
