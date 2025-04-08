using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDBContext : DbContext
    {
        public NZWalksDBContext(DbContextOptions<NZWalksDBContext> options) : base(options)
        {
        }
        public DbSet<Models.Domain.Walk> Walks { get; set; }
        public DbSet<Models.Domain.Region> Regions { get; set; }
        public DbSet<Models.Domain.Difficulty> Difficulties { get; set; }
        public DbSet<Models.Domain.Image> Images { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data for Difficulty
            // Easy Medium Hard
            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("4cb945e3-fb73-43bc-9305-861a92090ba6"),
                    Name="Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("0fc2ffb2-3667-414e-8508-5d66008e95ab") ,
                    Name="Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("67bb952f-a209-410a-9599-158d040348b6") ,
                    Name="Hard"
                }
            };
            // Seed difficulties to the database
            modelBuilder.Entity<Models.Domain.Difficulty>().HasData(difficulties);
            // Configure the primary key for the Walk entity
            modelBuilder.Entity<Models.Domain.Walk>()
                .HasKey(w => w.Id);
            // Configure the primary key for the Region entity
            modelBuilder.Entity<Models.Domain.Region>()
                .HasKey(r => r.Id);
            // Configure the primary key for the Difficulty entity
            modelBuilder.Entity<Models.Domain.Difficulty>()
                .HasKey(d => d.Id);
        }
    }
}
