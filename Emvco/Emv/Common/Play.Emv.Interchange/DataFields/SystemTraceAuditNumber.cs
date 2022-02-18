using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;

namespace Play.Emv.Interchange.DataFields;

public sealed record SystemTraceAuditNumber : EmvDataField<uint>
{
    #region Static Metadata

    public static readonly DataFieldId DataFieldId = 11;
    public static readonly InterchangeEncodingId EncodingId = NumericInterchangeCodec.Identifier;
    private const byte _MinValue = 1;
    private const nint _MaxValue = 999999;
    private const nint _ByteLength = 999999;

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

    #region Instance Members

    /// <summary>
    ///     Returns the <see cref="SystemTraceAuditNumber" /> as a byte array in Numeric format
    /// </summary>
    /// <returns></returns>
    public byte[] AsByteArray() => PlayEncoding.Numeric.GetBytes(_Value);
       

    #endregion

    #region Serialization

    public static SystemTraceAuditNumber Decode(ReadOnlySpan<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteLength, DataFieldId);

        _Codec.

    }

    #endregion

    #region Equality
     
    public override DataFieldId GetDataFieldId() => throw new NotImplementedException();

    public override InterchangeEncodingId GetEncodingId() => throw new NotImplementedException();  

    #endregion
     
}