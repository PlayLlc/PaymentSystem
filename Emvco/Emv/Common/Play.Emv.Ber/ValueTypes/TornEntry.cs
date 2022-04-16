﻿using Play.Emv.Ber.DataElements;

namespace Play.Emv.Ber;

public record TornEntry : RecordKey
{
    #region Constructor

    public TornEntry(RecordKey key) : base(key)
    { }

    public TornEntry(ApplicationPan pan, ApplicationPanSequenceNumber sequenceNumber) : base(pan, sequenceNumber)
    { }

    #endregion
}