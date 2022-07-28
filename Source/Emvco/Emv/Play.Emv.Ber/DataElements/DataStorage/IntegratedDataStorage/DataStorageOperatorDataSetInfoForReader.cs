using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains instructions from the Terminal on how to proceed with the transaction if:
///     • The AC requested by the Terminal does not match the AC proposed by the Kernel
///     • The update of the slot data has failed
/// </summary>
public record DataStorageOperatorDataSetInfoForReader : DataElement<byte>
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF810A;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public DataStorageOperatorDataSetInfoForReader(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <exception cref="PlayInternalException"></exception>
    public bool IsUsableForTransactionCryptogram() => _Value.IsBitSet(Bits.Eight);

    /// <exception cref="PlayInternalException"></exception>
    public bool IsUsableForAuthorizationRequestCryptogram() => _Value.IsBitSet(Bits.Seven);

    /// <exception cref="PlayInternalException"></exception>
    public bool IsUsableForApplicationCryptogram() => _Value.IsBitSet(Bits.Six);

    /// <exception cref="PlayInternalException"></exception>
    public bool IsStopIfNoDataStorageOperatorSetTerminalSet() => _Value.IsBitSet(Bits.Three);

    /// <exception cref="PlayInternalException"></exception>
    public bool IsStopIfWriteFailedSet() => _Value.IsBitSet(Bits.Two);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageOperatorDataSetInfoForReader Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DataStorageOperatorDataSetInfoForReader Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageOperatorDataSetInfoForReader Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new DataStorageOperatorDataSetInfoForReader(result);
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.BinaryCodec.Encode(_Value, length);

    #endregion
}