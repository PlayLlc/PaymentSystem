using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.TimeClock.Domain.Aggregates;
using Play.TimeClock.Domain.Services;

namespace Play.TimeClock.Domain.Repositories;

public interface IEmployeeRepository : IRepository<Employee, SimpleStringId>, IEnsureEmployeeDoesNotExist
{ }