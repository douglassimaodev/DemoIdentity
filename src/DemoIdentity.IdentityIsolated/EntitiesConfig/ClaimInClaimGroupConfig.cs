using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DemoIdentity.IdentityIsolated.Entities;

namespace DemoIdentity.IdentityIsolated.EntitiesConfig
{
    public class ClaimInClaimGroupConfig : IEntityTypeConfiguration<ClaimInClaimGroup>
    {
        public void Configure(EntityTypeBuilder<ClaimInClaimGroup> builder)
        {
            builder.ToTable("SecurityClaimInClaimGroup");

            builder.HasKey(c => c.ClaimInClaimGroupId);
        }
    }
}