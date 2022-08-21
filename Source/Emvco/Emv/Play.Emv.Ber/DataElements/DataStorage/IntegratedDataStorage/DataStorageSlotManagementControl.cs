using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains the Card indication, obtained in the response to the GET PROCESSING OPTIONS command, about the status of
///     the slot containing data associated to the DS Requested Operator ID.
/// </summary>
public record DataStorageSlotManagementControl : DataElement<byte>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F6F;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public DataStorageSlotManagementControl(byte value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageSlotManagementControl Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DataStorageSlotManagementControl Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageSlotManagementControl Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new DataStorageSlotManagementControl(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Instance Members

    public bool IsPermanent() => _Value.IsBitSet(Bits.Eight);
    public bool IsVolatile() => _Value.IsBitSet(Bits.Seven);
    public bool IsLowVolatility() => _Value.IsBitSet(Bits.Six);
    public bool IsLocked() => _Value.IsBitSet(Bits.Five);
    public bool IsDeactivated() => _Value.IsBitSet(Bits.One);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion
}