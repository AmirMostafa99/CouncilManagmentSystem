using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Xml;

namespace CouncilsManagmentSystem.Models
{
    public class ApplicationDbContext : IdentityDbContext <ApplicationUser>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions <ApplicationDbContext> options) : base(options) 
        { 
        }

        public  DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<Collage> collages { get; set; }
        public DbSet<TypeCouncil> typeCouncils { get; set; }
        public DbSet<Councils> Councils { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Topic> topics { get; set; }
        public DbSet<CouncilMembers> CouncilMembers { get; set; }
        public DbSet<Permissionss> permissionss { get; set; }
        public DbSet<Notifications> Notifications { get; set; }

        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CouncilMembers>()
                .HasKey(c => new { c.MemberId, c.CouncilId });


            // I Need To change the tables names but it make ERROR

            //modelBuilder.Entity<ApplicationUser>()
            //    .ToTable("Users", "Security")
            //    .Ignore(e => e.PhoneNumberConfirmed);
            //modelBuilder.Entity<ApplicationUser>().ToTable("Roles", "Security");
            //modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Security");
            //modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Security");
            //modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Security"); //TODO: ignore this 
            //modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Security"); //TODO: ignore this 
            //modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserToken", "Security");


            // This Code Make the data U enter mast be in arabic 
            modelBuilder.Entity<ApplicationUser>()
            .Property(p => p.UserName)
            .IsUnicode(true);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.functional_characteristic)
                .IsUnicode(true);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.academic_degree)
                .IsUnicode(true);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.administrative_degree)
                .IsUnicode(true);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.Discription)
                .IsUnicode(true);


        }

    }
}
