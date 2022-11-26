using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Persistence.Sql;
using Play.TimeClock.Domain.Aggregates;
using Play.TimeClock.Domain.Entities;

namespace Play.Loyalty.Persistence.Sql.Configuration;

internal class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
{
    #region Instance Members

    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable($"{nameof(Employee)}s").HasKey(x => x.Id);

        // Simple Properties
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.PrivateProperty<Employee, SimpleStringId>($"_MerchantId").ValueGeneratedOnAdd();
        builder.PrivateProperty<Employee, SimpleStringId>($"_UserId").ValueGeneratedOnAdd();
        builder.HasOne<Employee, TimePuncher, SimpleStringId>($"_TimePuncher", "TimePuncherId");
        builder.HasMany<Employee, TimeEntry, SimpleStringId>($"_TimeEntries", "TimeEntryId");
    }

    #endregion
}