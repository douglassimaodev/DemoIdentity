using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DemoIdentity.IdentityIsolated.Entities;

namespace DemoIdentity.IdentityIsolated.EntitiesConfig
{
    public class ClaimGroupConfig : IEntityTypeConfiguration<ClaimGroup>
    {
        public void Configure(EntityTypeBuilder<ClaimGroup> builder)
        {
            builder.ToTable("SecurityClaimGroup");

            builder.HasKey(c => c.ClaimGroupId);

            builder.Property(c => c.Name)
              .IsRequired()
              .HasMaxLength(50);

            builder.Property(c => c.Description)
             .HasMaxLength(300);           

            builder
               .HasMany(c => c.ChildrenClaimGroup)
               .WithOne(c => c.ParentClaimGroup)
               .HasForeignKey(c => c.ParentClaimGroupId);
        }
    }
}