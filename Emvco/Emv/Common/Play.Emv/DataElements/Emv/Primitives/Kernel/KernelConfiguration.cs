using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Description: Indicates the Kernel configuration options.
/// </summary>
public record KernelConfiguration : DataElement<byte>, IEqualityComparer<KernelConfiguration>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly KernelConfiguration Default = new KernelConfiguration(0);
    public static readonly Tag Tag = 0xDF811B;

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

    public static KernelConfiguration Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static KernelConfiguration Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 1;

        if (value.Length != byteLength)
        {
            throw new DataElementParsingException(
                $"The Primitive Value {nameof(KernelConfiguration)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<byte> result = _Codec.Decode(EncodingId, value) as DecodedResult<byte>
            ?? throw new DataElementParsingException(
                $"The {nameof(KernelConfiguration)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<byte>)}");

        return new KernelConfiguration(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

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