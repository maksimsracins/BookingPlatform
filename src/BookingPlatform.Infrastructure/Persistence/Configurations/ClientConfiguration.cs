using BookingPlatform.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingPlatform.Infrastructure.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName)
            .HasMaxLength(200);

        builder.Property(x => x.Phone)
            .HasMaxLength(30);

        builder.Property(x => x.ExternalId)
            .HasMaxLength(100);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.HasIndex(x => new
        {
            x.BusinessId,
            x.ExternalId
        }).IsUnique();

        builder.HasOne(x => x.Business)
            .WithMany(x => x.Clients)
            .HasForeignKey(x => x.BusinessId);

        builder.HasMany(x => x.Appointments)
            .WithOne(x => x.Client)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}