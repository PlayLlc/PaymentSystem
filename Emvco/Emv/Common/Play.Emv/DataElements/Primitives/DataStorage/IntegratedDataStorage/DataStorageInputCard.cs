using System;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Contains Terminal provided data if permanent data storage in the Card was applicable (DS Slot Management
///     Control[8]=1b), remains applicable, or becomes applicable (DS ODS Info[8]=1b). Otherwise this data item is a filler
///     to be supplied by the Kernel. The data is forwarded to the Card with the GENERATE AC command, as per DSDOL
///     formatting
/// </summary>
public record DataStorageInputCard : DataElement<ulong>
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF60;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _ByteLength = 8;

    #endregion

    #region Constructor

    public DataStorageInputCard(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static DataStorageInputCard Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static DataStorageInputCard Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.BinaryCodec.DecodeToUInt64(value);

        return new DataStorageInputCard(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion
}