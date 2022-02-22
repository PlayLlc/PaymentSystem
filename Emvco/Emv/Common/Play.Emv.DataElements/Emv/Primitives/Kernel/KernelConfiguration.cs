using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Description: Indicates the Kernel configuration options.
/// </summary>
public record KernelConfiguration : DataElement<byte>, IEqualityComparer<KernelConfiguration>
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = BinaryCodec.EncodingId;
    public static readonly KernelConfiguration Default = new(0);
    public static readonly Tag Tag = 0xDF811B;

    #endregion

    #region Constructor

    public KernelConfiguration(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => PlayEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public bool IsEmvModeContactlessTransactionsNotSupported() => _Value.IsBitSet(Bits.Seven);
    public bool IsMagstripeModeContactlessTransactionsNotSupported() => _Value.IsBitSet(Bits.Eight);
    public bool IsOnDeviceCardholderVerificationSupported() => _Value.IsBitSet(Bits.Six);
    public bool IsReadAllRecordsEvenWhenNoCdaActivated() => _Value.IsBitSet(Bits.Three);
    public bool IsRelayResistanceProtocolSupported() => _Value.IsBitSet(Bits.Five);
    public bool IsReservedForPaymentSystem() => _Value.IsBitSet(Bits.Four);

    #endregion

    #region Serialization

    public static KernelConfiguration Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static KernelConfiguration Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 1;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(KernelConfiguration)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<byte> result = _Codec.Decode(PlayEncodingId, value) as DecodedResult<byte>
            ?? throw new InvalidOperationException(
                $"The {nameof(KernelConfiguration)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<byte>)}");

        return new KernelConfiguration(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(PlayEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(PlayEncodingId, _Value, length);

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