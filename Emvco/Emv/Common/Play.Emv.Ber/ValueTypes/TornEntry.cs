using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Kernel.Services;

public record TornEntry : RecordKey
{
    #region Constructor

    public TornEntry(RecordKey key) : base(key)
    { }

    public TornEntry(ApplicationPan pan, ApplicationPanSequenceNumber sequenceNumber) : base(pan, sequenceNumber)
    { }

    #endregion
}