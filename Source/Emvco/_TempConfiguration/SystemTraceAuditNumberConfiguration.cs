using Play.Emv.Ber.ValueTypes;

namespace _TempConfiguration;

public class SystemTraceAuditNumberConfiguration
{
    #region Instance Values

    /// <summary>
    ///     This value determines the maximum value that the <see cref="SystemTraceAuditNumber" /> can reach before sending a
    ///     Settlement request to the Acquirer
    /// </summary>
    public uint Threshold;

    /// <summary>
    ///     This is the value of the last <see cref="SystemTraceAuditNumber" /> the Terminal held before shutting down
    /// </summary>
    public uint SystemTraceAuditNumberInitializationValue;

    #endregion
}