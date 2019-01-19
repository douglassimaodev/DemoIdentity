using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DemoIdentity.IdentityIsolated.Entities;
using DemoIdentity.IdentityIsolated.EntitiesConfig;

namespace DemoIdentity.IdentityIsolated.ContextConfiguration
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }
        public DbSet<ApplicationUserLogin> ApplicationUserLogins { get; set; }
        public DbSet<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }
        public DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }

        public DbSet<AppClaim> AppClaims { get; set; }
        public DbSet<ClaimGroup> ClaimGroups { get; set; }
        public DbSet<ClaimInClaimGroup> ClaimsInClaimGroup { get; set; }
        public DbSet<ClaimInRole> ClaimsInRole { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }           

            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ApplicationRoleClaimConfig());
            builder.ApplyConfiguration(new ApplicationRoleConfig());
            builder.ApplyConfiguration(new ApplicationUserClaimConfig());
            builder.ApplyConfiguration(new ApplicationUserConfig());
            builder.ApplyConfiguration(new ApplicationUserLoginConfig());
            builder.ApplyConfiguration(new ApplicationUserRoleConfig());
            builder.ApplyConfiguration(new ApplicationUserTokenConfig());

            builder.ApplyConfiguration(new AppClaimConfig());
            builder.ApplyConfiguration(new ClaimGroupConfig());
            builder.ApplyConfiguration(new ClaimInClaimGroupConfig());
            builder.ApplyConfiguration(new ClaimInRoleConfig());
        }
    }
}