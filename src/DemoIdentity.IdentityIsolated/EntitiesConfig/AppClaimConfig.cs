using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DemoIdentity.IdentityIsolated.Entities;

namespace DemoIdentity.IdentityIsolated.EntitiesConfig
{
    public class AppClaimConfig : IEntityTypeConfiguration<AppClaim>
    {
        public void Configure(EntityTypeBuilder<AppClaim> builder)
        {
            builder.ToTable("SecurityAppClaim");

            builder.HasKey(c => c.AppClaimId);

            builder.Property(c => c.Code)
              .IsRequired()
              .HasMaxLength(50);

            builder.Property(c => c.Name)
             .IsRequired()
             .HasMaxLength(50);

            builder.Property(c => c.Description)
             .HasMaxLength(300);
        }
    }
}