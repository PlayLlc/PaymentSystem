using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Aggregates;
using Play.Domain.Entities;

namespace Play.Domain.Repositories
{
    public abstract class Dto<_Id>
    {
        #region Instance Values

        public abstract _Id Id { get; set; }

        #endregion
    }
}