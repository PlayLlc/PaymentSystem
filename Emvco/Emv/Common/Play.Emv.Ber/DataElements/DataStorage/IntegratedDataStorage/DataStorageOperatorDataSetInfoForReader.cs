﻿using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
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

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageOperatorDataSetCard Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DataStorageOperatorDataSetCard Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageOperatorDataSetCard Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new DataStorageOperatorDataSetCard(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsUsableForTransactionCryptogram() => _Value.IsBitSet(Bits.Eight);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsUsableForAuthorizationRequestCryptogram() => _Value.IsBitSet(Bits.Seven);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsUsableForApplicationCryptogram() => _Value.IsBitSet(Bits.Six);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsStopIfNoDataStorageOperatorSetTerminalSet() => _Value.IsBitSet(Bits.Three);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsStopIfWriteFailedSet() => _Value.IsBitSet(Bits.Two);

    #endregion
}