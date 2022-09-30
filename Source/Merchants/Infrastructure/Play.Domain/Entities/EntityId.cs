using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Domain.Entities
{
    public abstract record EntityId<_TId>
    {
        #region Instance Values

        public readonly _TId Id;

        #endregion

        #region Constructor

        protected EntityId(_TId id)
        {
            Id = id;
        }

        #endregion
    }
}