using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the results of the last CVM performed
/// </summary>
public record CvmResults : DataElement<uint>, IEqualityComparer<CvmResults>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F34;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const ushort _ByteLength = 3;

    #endregion

    #region Constructor

    public CvmResults(uint value) : base(value)
    { }

    public CvmResults(CvmCode cvmPerformed, CvmConditionCode cvmConditionCode, CvmResultCodes cvmResultCode) : base(Create(cvmPerformed, cvmConditionCode,
        cvmResultCode))
    { }

    #endregion

    #region Instance Members

    private static uint Create(CvmCode cvmCode, CvmConditionCode cvmConditionCode, CvmResultCodes cvmResultCode)
    {
        uint result = (uint) cvmCode << 16;
        result |= (uint) cvmConditionCode << 8;
        result |= (uint) cvmResultCode;

        return result;
    }

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => _ByteLength;
    public new ushort GetValueByteCount() => _ByteLength;
    public byte[] Encode() => Codec.EncodeValue(EncodingId, _Value, _ByteLength);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static CvmResults Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override CvmResults Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static CvmResults Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.BinaryCodec.DecodeToUInt32(value);

        return new CvmResults(result);
    }

    public new byte[] EncodeValue() => Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(CvmResults? x, CvmResults? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CvmResults obj) => obj.GetHashCode();

    #endregion
}