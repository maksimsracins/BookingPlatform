using BookingPlatform.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingPlatform.Infrastructure.Persistence.Configurations;

public sealed class EmployeeWorkingHoursConfiguration
    : IEntityTypeConfiguration<EmployeeWorkingHours>
{
    public void Configure(EntityTypeBuilder<EmployeeWorkingHours> builder)
    {
        builder.ToTable("employee_working_hours");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DayOfWeek);

        builder.Property(x => x.StartTime)
            .HasColumnType("time without time zone");

        builder.Property(x => x.EndTime)
            .HasColumnType("time without time zone");

        builder.HasOne(x => x.Employee)
            .WithMany(x => x.WorkingHours)
            .HasForeignKey(x => x.EmployeeId);

        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.DayOfWeek
        }).IsUnique();
    }
}