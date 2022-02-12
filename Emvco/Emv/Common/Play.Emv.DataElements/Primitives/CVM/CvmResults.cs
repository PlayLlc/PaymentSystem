using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the results of the last CVM performed
/// </summary>
public record CvmResults : DataElement<uint>, IEqualityComparer<CvmResults>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F34;
    public static readonly BerEncodingId BerEncodingId = BinaryCodec.Identifier;
    private const ushort _ByteLength = 3;

    #endregion

    #region Constructor

    public CvmResults(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => _ByteLength;
    public new ushort GetValueByteCount() => _ByteLength;
    public byte[] Encode() => _Codec.EncodeValue(BerEncodingId, _Value, _ByteLength);

    #endregion

    #region Serialization

    public static CvmResults Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static CvmResults Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length != _ByteLength)
        {
            throw new
                ArgumentOutOfRangeException($"The Primitive Value {nameof(CvmResults)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteLength} bytes in length");
        }

        DecodedResult<uint> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<uint>
            ?? throw new
                InvalidOperationException($"The {nameof(CvmResults)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        return new CvmResults(result.Value);
    }

    public override byte[] EncodeValue(BerCodec berCodec) => _Codec.EncodeValue(BerEncodingId, _Value, _ByteLength);

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