using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DemoIdentity.IdentityIsolated.Entities;

namespace DemoIdentity.IdentityIsolated.EntitiesConfig
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("SecurityUser");

            builder.Property(c => c.Id)
                .HasColumnName("UserId");           
        }
    }
}