using System;

using Play.Codecs;
using Play.Core.Extensions;

namespace Play.Emv.Sessions;

public readonly struct SequenceTraceAuditNumber
{
    #region Static Metadata

    private const byte _MinValue = 1;
    private const uint _MaxValue = 999999;

    #endregion

    #region Instance Values

    private readonly uint _Value;

    #endregion

    #region Constructor

    /// <summary>
    ///     A snapshot of the number of transactions processed by the terminal within a business day
    /// </summary>
    /// <param name="value">
    ///     The value of the STAN. A number between 1 and 999,999
    /// </param>
    public SequenceTraceAuditNumber(uint value)
    {
        if (value == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"The argument {nameof(value)} has a value of {value} but must be a value between {_MinValue} and {_MaxValue}");
        }

        if (value > _MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"The argument {nameof(value)} has a value of {value} but must be a value between {_MinValue} and {_MaxValue}");
        }

        _Value = value;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Returns the <see cref="SequenceTraceAuditNumber" /> as a byte array in Numeric format
    /// </summary>
    /// <returns></returns>
    public byte[] AsByteArray() => PlayEncoding.Numeric.GetBytes(_Value);

    public int GetByteCount() => PlayEncoding.Numeric.GetMaxByteCount(_Value.GetNumberOfDigits());
    public int GetNumberOfDigits() => _Value.GetNumberOfDigits();

    #endregion

    #region Equality

    public bool Equals(SequenceTraceAuditNumber other) => _Value == other._Value;

    public override bool Equals(object? obj) =>
        obj is SequenceTraceAuditNumber sequenceTraceAuditNumber && Equals(sequenceTraceAuditNumber);

    public int GetHashCode(SequenceTraceAuditNumber other) => other.GetHashCode();
    public override int GetHashCode() => unchecked(26183 * _Value.GetHashCode());

    #endregion

    #region Operator Overrides

    public static bool operator ==(SequenceTraceAuditNumber left, SequenceTraceAuditNumber right) => left._Value == right._Value;
    public static explicit operator uint(SequenceTraceAuditNumber classType) => classType._Value;
    public static bool operator !=(SequenceTraceAuditNumber left, SequenceTraceAuditNumber right) => !(left == right);

    #endregion
}