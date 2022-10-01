using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Aggregates;
using Play.Domain.Repositories;

namespace Play.Domain
{
    public interface IMapDtoToAggregate
    {
        #region Instance Members

        public AggregateBase<_Id> AsAggregate<_Id>(Dto<_Id> model);

        #endregion
    }
}