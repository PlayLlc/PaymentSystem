using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Description: Indicates the Kernel configuration options.
/// </summary>
public record KernelConfiguration : DataElement<byte>, IEqualityComparer<KernelConfiguration>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly KernelConfiguration Default = new(0);
    public static readonly Tag Tag = 0xDF811B;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public KernelConfiguration(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public bool IsEmvModeSupported() => !_Value.IsBitSet(Bits.Seven);
    public bool IsMagstripeModeSupported() => !_Value.IsBitSet(Bits.Eight);
    public bool IsOnDeviceCardholderVerificationSupported() => _Value.IsBitSet(Bits.Six);

    /// <summary>
    ///     When this flag is set the kernel will read all of the records listed in the <see cref="ApplicationFileLocator" />,
    ///     regardless if Combined Data Authentication is supported or not
    /// </summary>
    /// <returns></returns>
    public bool IsReadAllRecordsActivated() => _Value.IsBitSet(Bits.Three);

    public bool IsRelayResistanceProtocolSupported() => _Value.IsBitSet(Bits.Five);
    public bool IsReservedForPaymentSystem() => _Value.IsBitSet(Bits.Four);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static KernelConfiguration Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static KernelConfiguration Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new KernelConfiguration(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(KernelConfiguration? x, KernelConfiguration? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(KernelConfiguration obj) => obj.GetHashCode();

    #endregion
}