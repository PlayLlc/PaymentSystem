using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.TimeClock.Domain.Aggregates.Employees;

namespace Play.TimeClock.Domain.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee, SimpleStringId>
    { }
}