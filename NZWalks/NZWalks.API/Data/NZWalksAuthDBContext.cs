using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDBContext : IdentityDbContext
    {
        public NZWalksAuthDBContext(DbContextOptions<NZWalksAuthDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "fc22f3cd-3535-4317-93c0-43fced6dd2dc";
            var writerRoleId = "80796d96-3ad8-4024-8ce7-1f6f2272a533";
            var roles = new List<IdentityRole> 
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name ="Reader",
                    NormalizedName="Reader".ToUpper()
                },
                 new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name ="Writer",
                    NormalizedName="Writer".ToUpper()
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
