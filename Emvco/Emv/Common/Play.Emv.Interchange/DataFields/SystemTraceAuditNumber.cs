﻿using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;

namespace Play.Emv.Interchange.DataFields;

public sealed record SystemTraceAuditNumber : EmvDataField<uint>
{
    #region Static Metadata

    private const byte _MinValue = 1;
    private const nint _MaxValue = 999999;
    public static readonly DataFieldId DataFieldId = 11;
    public static readonly InterchangeEncodingId EncodingId = 
    #endregion

    #region Constructor

    /// <summary>
    ///     A snapshot of the number of transactions processed by the terminal within a business day
    /// </summary>
    /// <param name="value">
    ///     The value of the STAN. A number between 1 and 999,999
    /// </param>
    public SystemTraceAuditNumber(uint value) : base(value)
    {
        byte[] test = new byte[3]; 

        Check.DataField.ForMinimumLength(test, _MinValue, GetDataFieldId());
        Check.DataField.ForMaximumLength(test, _MaxValue, GetDataFieldId()); 
    }

    #endregion


    public static SystemTraceAuditNumber Decode(ReadOnlySpan<byte> value)
    {
        Check.DataField.For
    }


    #region Instance Members

    /// <summary>
    ///     Returns the <see cref="SystemTraceAuditNumber" /> as a byte array in Numeric format
    /// </summary>
    /// <returns></returns>
    public byte[] AsByteArray() => PlayEncoding.Numeric.GetBytes(_Value);

    public int GetByteCount() => PlayEncoding.Numeric.GetMaxByteCount(_Value.GetNumberOfDigits());
    public int GetNumberOfDigits() => _Value.GetNumberOfDigits();

    #endregion

    #region Equality

    public bool Equals(SystemTraceAuditNumber other) => _Value == other._Value;
    public override bool Equals(object? obj) => obj is SystemTraceAuditNumber sequenceTraceAuditNumber && Equals(sequenceTraceAuditNumber);
    public int GetHashCode(SystemTraceAuditNumber other) => other.GetHashCode();
    public override int GetHashCode() => unchecked(26183 * _Value.GetHashCode());

    #endregion

    #region Operator Overrides

    public static bool operator ==(SystemTraceAuditNumber left, SystemTraceAuditNumber right) => left._Value == right._Value;
    public static explicit operator uint(SystemTraceAuditNumber classType) => classType._Value;
    public static bool operator !=(SystemTraceAuditNumber left, SystemTraceAuditNumber right) => !(left == right);

    #endregion
}