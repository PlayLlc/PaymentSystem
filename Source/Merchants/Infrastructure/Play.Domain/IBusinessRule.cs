using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Domain;

public interface IBusinessRule
{
    #region Instance Values

    string Message { get; }

    #endregion

    #region Instance Members

    bool IsBroken();

    #endregion
}