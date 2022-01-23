﻿using Play.Emv.DataElements;
using Play.Globalization.Time;

namespace Play.Emv.Terminal;

public abstract class PaymentLogItem
{
    #region Instance Values

    /// <summary>
    ///     The account number associated to this transaction snapshot
    /// </summary>
    protected readonly PrimaryAccountNumber _PrimaryAccountNumber;

    /// <summary>
    ///     A sequential number of transaction log items associated to this <see cref="PrimaryAccountNumber" />
    /// </summary>
    protected readonly uint _SequenceNumber;

    /// <summary>
    ///     Transaction date in the format YYMMDD
    /// </summary>
    protected readonly ShortDateValue _TransactionDate;

    #endregion

    #region Constructor

    protected PaymentLogItem(PrimaryAccountNumber primaryAccountNumber, uint sequenceNumber, ShortDateValue transactionDate)
    {
        _PrimaryAccountNumber = primaryAccountNumber;
        _SequenceNumber = sequenceNumber;
        _TransactionDate = transactionDate;
    }

    #endregion
}