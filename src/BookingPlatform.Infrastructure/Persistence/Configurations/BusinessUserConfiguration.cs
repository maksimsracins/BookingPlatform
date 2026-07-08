using BookingPlatform.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingPlatform.Infrastructure.Persistence.Configurations;

public sealed class BusinessUserConfiguration
    : IEntityTypeConfiguration<BusinessUser>
{
    public void Configure(EntityTypeBuilder<BusinessUser> builder)
    {
        builder.ToTable("business_users");

        builder.HasKey(x => new
        {
            x.BusinessId,
            x.UserId
        });

        builder.Property(x => x.Role)
            .HasConversion<int>();

        builder.HasOne(x => x.Business)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.BusinessId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Businesses)
            .HasForeignKey(x => x.UserId);
    }
}