using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerId = "e2206b2b-566a-438b-958b-bde1d04475be";
            var writerId = "0f157405-6306-49ba-b433-2ecf17d55445";

            var roles = new List<IdentityRole>()
            {

                new IdentityRole()
                {
                    Id = readerId,
                    ConcurrencyStamp =readerId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                },
                new IdentityRole()
                {
                    Id = writerId,
                    ConcurrencyStamp =writerId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
