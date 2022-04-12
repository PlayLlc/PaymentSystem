using Play.Emv.Ber.DataElements;

namespace Play.Emv.Ber.ValueTypes;

/// <summary>
///     A key that identifies a <see cref="Record" />
/// </summary>
public record RecordKey
{
    #region Instance Values

    /// <summary>
    ///     The account number associated to this transaction snapshot
    /// </summary>
    protected readonly ApplicationPan _ApplicationPan;

    /// <summary>
    ///     A sequential number of transaction log items associated to this <see cref="ApplicationPanSequenceNumber" />
    /// </summary>
    protected readonly ApplicationPanSequenceNumber _SequenceNumber;

    #endregion

    #region Constructor

    public RecordKey(ApplicationPan pan, ApplicationPanSequenceNumber sequenceNumber)
    {
        _ApplicationPan = pan;
        _SequenceNumber = sequenceNumber;
    }

    #endregion
}