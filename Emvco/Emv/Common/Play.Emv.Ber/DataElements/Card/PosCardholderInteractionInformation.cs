﻿using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

public record PosCardholderInteractionInformation : DataElement<uint>, IEqualityComparer<PosCardholderInteractionInformation>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF4B;
    private const byte _ByteLength = 3;

    #endregion

    #region Constructor

    public PosCardholderInteractionInformation(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public bool IsSecondTapRequiredForWallet() => _Value.IsBitSet(1);
    public bool IsOnDeviceCvmRequired() => _Value.IsBitSet(9);
    public bool IsAcknowledgementRequired() => _Value.IsBitSet(10);
    public bool IsOfflinePinChangeRequired() => _Value.IsBitSet(11);
    public bool IsContextConflicting() => _Value.IsBitSet(12);
    public bool IsOnDeviceCvmVerificationSuccessful() => _Value.IsBitSet(9);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    public static bool EqualsStatic(PosCardholderInteractionInformation? x, PosCardholderInteractionInformation? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static PosCardholderInteractionInformation Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override PosCardholderInteractionInformation Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static PosCardholderInteractionInformation Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.BinaryCodec.DecodeToUInt32(value);

        return new PosCardholderInteractionInformation(result);
    }

    #endregion

    #region Equality

    public bool Equals(PosCardholderInteractionInformation? x, PosCardholderInteractionInformation? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(PosCardholderInteractionInformation obj) => obj.GetHashCode();

    #endregion
}