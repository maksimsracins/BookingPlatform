using BookingPlatform.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingPlatform.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("appointments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Price)
            .HasPrecision(10, 2);

        builder.Property(x => x.Comment)
            .HasMaxLength(1000);

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.CreatedAt);

        builder.HasOne(x => x.Business)
            .WithMany(x => x.Appointments)
            .HasForeignKey(x => x.BusinessId);

        builder.HasOne(x => x.Client)
            .WithMany(x => x.Appointments)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Employee)
            .WithMany(x => x.Appointments)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Service)
            .WithMany()
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.StartAt
        });

        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.StartAt,
            x.EndAt
        });

        builder.HasIndex(x => new
        {
            x.BusinessId,
            x.StartAt
        });

        builder.HasIndex(x => x.ClientId);
    }
}