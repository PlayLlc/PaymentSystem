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

    internal bool AnyUndeliveredPaychecks() => _Employees.Any(a => a.AnyUndeliveredPaychecks());

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task CreateEmployee(IRetrieveUsers userRetriever, CreateEmployee command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Employer>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Employer>(_MerchantId, user));
        Enforce(new EmployeeMustNotExist(command.UserIdOfNewEmployee, _Employees));

        Compensation compensation = new(GenerateSimpleStringId(), command.CompensationType, command.CompensationRate);
        Employee employee = Employee.Create(GenerateSimpleStringId(), command.UserIdOfNewEmployee, compensation);

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
        Enforce(new EmployeeMustNotHaveUndeliveredPaychecks(_Employees.First(a => a.Id == command.EmployeeId)));

        _ = _Employees.RemoveWhere(a => a.Id == command.EmployeeId);
        Publish(new EmployeeHasBeenRemoved(this, command.EmployeeId, command.UserId));
    }

    public async Task UpdateTimeEntry(IRetrieveUsers userRetriever, UpdateTimeEntry command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Employer>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Employer>(user.MerchantId, user));
        Enforce(new EmployeeMustExist(command.EmployeeId, _Employees));

        var employee = _Employees.First(a => a.Id == command.EmployeeId);
        employee.UpdateTimeEntry(command);
        Publish(new TimeSheetHasBeenUpdated(this, command.TimeEntryId));
    }

    /// <exception cref="ValueObjectException"></exception>
    public async Task UpdateCompensation(IRetrieveUsers userRetriever, UpdateEmployeeCompensation command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Employer>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Employer>(user.MerchantId, user));
        Enforce(new EmployeeMustExist(command.EmployeeId, _Employees));

        var employee = _Employees.First(a => a.Id == command.EmployeeId);
        employee.UpdateCompensation(command);
        Publish(new TimeSheetHasBeenUpdated(this, command.EmployeeId));
    }

    #endregion
}