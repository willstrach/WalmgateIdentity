using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WalmgateIdentity.Infrastructure.Database.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("tenant", "admin");
        builder.HasKey(tenant => tenant.Id);
        builder.HasQueryFilter(tenant => !tenant.IsDeleted);

        builder.Property(tenant => tenant.Id)
            .HasColumnName("id");

        builder.Property(tenant => tenant.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.HasIndex(tenant => tenant.Name)
            .HasFilter(@"is_deleted is not true")
            .IncludeProperties(tenant => tenant.Id);

        builder.Property(tenant => tenant.IsDeleted)
            .IsRequired()
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);            

        builder.Property(tenant => tenant.DeletedAt)
            .HasColumnName("deleted_at");
    }
}
