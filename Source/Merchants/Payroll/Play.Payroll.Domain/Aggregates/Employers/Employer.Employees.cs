using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Payroll.Contracts.Commands;
using Play.Payroll.Domain.Entities;
using Play.Payroll.Domain.Services;

namespace Play.Payroll.Domain.Aggregates;

public partial class Employer : Aggregate<SimpleStringId>
{
    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task CreateEmployee(IRetrieveUsers userRetriever, CreateEmployee command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Employer>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Employer>(user.MerchantId, user));
        Enforce(new EmployeeMustNotExist(command.EmployeeUserId, _Employees));

        Compensation compensation = new(GenerateSimpleStringId(), command.CompensationType, command.CompensationRate);
        var employee = Employee.Create(GenerateSimpleStringId(), command.UserId, compensation);

        _ = _Employees.Add(employee);

        Publish(new EmployeeHasBeenCreated(this, employee.Id, command.UserId));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task RemoveEmployee(IRetrieveUsers userRetriever, RemoveEmployee command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Employer>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Employer>(user.MerchantId, user));
        Enforce(new EmployeeMustExist(command.EmployeeId, _Employees));
        var employee = _Employees.First(a => a.Id == command.EmployeeId);
        Enforce(new EmployeeMustNotHaveUndeliveredPaychecks(employee));

        _Employees.RemoveWhere(a => a.Id == command.EmployeeId);
        Publish(new EmployeeHasBeenRemoved(this, command.EmployeeId, command.UserId));
    }

    #endregion
}