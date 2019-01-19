using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DemoIdentity.IdentityIsolated.Entities;

namespace DemoIdentity.IdentityIsolated.EntitiesConfig
{
    public class ApplicationUserTokenConfig : IEntityTypeConfiguration<ApplicationUserToken>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
        {
            builder.ToTable("SecurityUserToken");
        }
    }
}