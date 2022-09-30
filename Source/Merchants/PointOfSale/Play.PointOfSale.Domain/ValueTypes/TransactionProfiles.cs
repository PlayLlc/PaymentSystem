using Play.PointOfSale.Domain.ValueTypes.TransactionProfile;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.PointOfSale.Domain.ValueTypes;

internal class TransactionProfiles
{
    #region Instance Values

    private readonly TerminalTransactionQualifiers _TerminalTransactionQualifiers;

    #endregion

    #region Constructor

    public TransactionProfiles(TerminalTransactionQualifiers terminalTransactionQualifiers)
    {
        _TerminalTransactionQualifiers = terminalTransactionQualifiers;
    }

    #endregion
}