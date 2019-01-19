using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DemoIdentity.IdentityIsolated.Entities;

namespace DemoIdentity.IdentityIsolated.EntitiesConfig
{
    public class ClaimInRoleConfig : IEntityTypeConfiguration<ClaimInRole>
    {
        public void Configure(EntityTypeBuilder<ClaimInRole> builder)
        {
            builder.ToTable("SecurityClaimInRole");

            builder.HasKey(c => c.ClaimInRoleId);
        }
    }
}