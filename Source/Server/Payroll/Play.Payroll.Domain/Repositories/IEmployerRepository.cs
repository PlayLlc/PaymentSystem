using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Payroll.Domain.Aggregates;

namespace Play.Payroll.Domain.Repositories;

public interface IEmployerRepository : IRepository<Employer, SimpleStringId>
{ }