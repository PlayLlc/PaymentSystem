using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.Kernel.Services;

private class Commit
{
    #region Instance Values

    private readonly TornRecord _TornRecord;
    private readonly DateTimeUtc _CommitTimeStamp;

    #endregion
}

private class TornEntry
{
    #region Instance Values

    private readonly ApplicationPan _ApplicationPan;
    private readonly ApplicationPanSequenceNumber _ApplicationPanSequenceNumber;

    #endregion
}

public abstract class PaymentLogItem
{
    #region Instance Values

    /// <summary>
    ///     The account number associated to this transaction snapshot
    /// </summary>
    protected readonly ApplicationPan _PrimaryAccountNumber;

    /// <summary>
    ///     A sequential number of transaction log items associated to this <see cref="PrimaryAccountNumber" />
    /// </summary>
    protected readonly uint _SequenceNumber;

    /// <summary>
    ///     Transaction date in the format YYMMDD
    /// </summary>
    protected readonly ShortDate _TransactionDate;

    #endregion

    #region Constructor

    protected PaymentLogItem(ApplicationPan primaryAccountNumber, uint sequenceNumber, ShortDate transactionDate)
    {
        _PrimaryAccountNumber = primaryAccountNumber;
        _SequenceNumber = sequenceNumber;
        _TransactionDate = transactionDate;
    }

    #endregion
}