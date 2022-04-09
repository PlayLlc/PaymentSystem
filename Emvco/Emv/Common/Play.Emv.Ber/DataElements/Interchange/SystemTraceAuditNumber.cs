using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

public readonly record struct SystemTraceAuditNumber
{
    #region Static Metadata

    private const byte _MinValue = 1;
    private const int _MaxValue = 999999;

    #endregion

    #region Instance Values

    public readonly uint _Value;

    #endregion

    #region Constructor

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="DataElementParsingException"></exception>
    public SystemTraceAuditNumber(uint value)
    {
        if (value < _MinValue)
            throw new DataElementParsingException($"The {nameof(SystemTraceAuditNumber)} must have a value of at least: [{_MinValue}]");

        if (value > _MaxValue)
            throw new DataElementParsingException($"The {nameof(SystemTraceAuditNumber)} must have a value less than: [{_MaxValue}]");

        _Value = value;
    }

    #endregion
}