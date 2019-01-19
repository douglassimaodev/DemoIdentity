using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DemoIdentity.IdentityIsolated.Entities;

namespace DemoIdentity.IdentityIsolated.EntitiesConfig
{
    public class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.ToTable("SecurityRole");

            builder.Property(c => c.Id)
               .HasColumnName("RoleId");

            builder
              .HasMany(c => c.ClaimsInRole)
              .WithOne(c => c.ApplicationRole)
              .HasForeignKey(c => c.RoleId);
        }
    }
}