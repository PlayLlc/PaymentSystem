using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Terminal.Session;

public class SequenceConfiguration
{
    #region Instance Values

    /// <summary>
    ///     This value determines the maximum value that the <see cref="SystemTraceAuditNumber" /> can reach before sending a
    ///     Settlement request to the Acquirer
    /// </summary>
    public readonly uint Threshold;

    /// <summary>
    ///     This is the value of the last <see cref="SystemTraceAuditNumber" /> the Terminal held before shutting down
    /// </summary>
    public readonly uint InitializationValue;

    #endregion

    #region Constructor

    public SequenceConfiguration(uint threshold, uint initializationValue)
    {
        Threshold = threshold;
        InitializationValue = initializationValue;
    }

    #endregion
}