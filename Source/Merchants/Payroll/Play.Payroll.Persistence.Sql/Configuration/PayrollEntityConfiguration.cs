using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.Domain.Common.ValueObjects;
using Play.Payroll.Domain.Aggregates;
using Play.Payroll.Domain.Entities;
using Play.Persistence.Sql;

namespace Play.Payroll.Persistence.Sql.Configuration;

internal class PayrollEntityConfiguration : IEntityTypeConfiguration<Employer>
{
    #region Instance Members

    public void Configure(EntityTypeBuilder<Employer> builder)
    {
        builder.ToTable($"{nameof(Employer)}").HasKey(x => x.Id);

        // Simple Properties
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.PrivateProperty<Employer, SimpleStringId>("_MerchantId").ValueGeneratedOnAdd();
        builder.HasOne<Employer, PaydaySchedule, SimpleStringId>("_PaydaySchedule", "PaydayScheduleId");
        builder.HasMany<Employer, Employee, SimpleStringId>("_Employees", "EmployeeId");
    }

    #endregion
}