using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DemoIdentity.IdentityIsolated.Entities;

namespace DemoIdentity.IdentityIsolated.EntitiesConfig
{
    public class ApplicationUserRoleConfig : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.ToTable("SecurityUserRole");
        }
    }
}