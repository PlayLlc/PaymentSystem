using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Command data field of the GET PROCESSING OPTIONS command, coded according to PDOL.
/// </summary>
public record ProcessingOptionsDataObjectListRelatedData : DataElement<BigInteger>, IEqualityComparer<ProcessingOptionsDataObjectListRelatedData>
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF8111;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;

    #endregion

    #region Constructor

    public ProcessingOptionsDataObjectListRelatedData(BigInteger value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ProcessingOptionsDataObjectListRelatedData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ProcessingOptionsDataObjectListRelatedData Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ProcessingOptionsDataObjectListRelatedData Decode(ReadOnlySpan<byte> value)
    {
        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new ProcessingOptionsDataObjectListRelatedData(result);
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value);

    #endregion

    #region Equality

    public bool Equals(ProcessingOptionsDataObjectListRelatedData? x, ProcessingOptionsDataObjectListRelatedData? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ProcessingOptionsDataObjectListRelatedData obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion
}