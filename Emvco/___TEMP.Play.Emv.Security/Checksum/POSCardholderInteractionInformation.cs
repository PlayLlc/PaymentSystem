using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Emv.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Core.Extensions;

namespace ___TEMP.Play.Emv.Security.Checksum;

/// <summary>
///     Description: The POS Cardholder Interaction Information informs the Kernel about the indicators set in the mobile
///     phone that may influence
///     the action flow of the merchant and cardholder.
/// </summary>
public record PosCardholderInteractionInformation : PrimitiveValue, IEqualityComparer<PosCardholderInteractionInformation>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0xDF4B;

    #endregion

    #region Instance Values

    private readonly uint _Value;

    #endregion

    #region Constructor

    public PosCardholderInteractionInformation(uint value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public bool AckRequired()
    {
        return _Value.IsBitSet(10);
    }

    public bool ContextIsConflicting()
    {
        return _Value.IsBitSet(12);
    }

    public static bool EqualsStatic(PosCardholderInteractionInformation? x, PosCardholderInteractionInformation? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override BerEncodingId GetBerEncodingId()
    {
        return BerEncodingId;
    }

    public override Tag GetTag()
    {
        return Tag;
    }

    public override ushort GetValueByteCount(BerCodec codec)
    {
        return codec.GetByteCount(GetBerEncodingId(), _Value);
    }

    public bool OfflineChangePinRequired()
    {
        return _Value.IsBitSet(11);
    }

    public bool OfflineDataCardVerificationMethodRequired()
    {
        return _Value.IsBitSet(9);
    }

    public bool OnDeviceCardholderVerificationMethodVerificationSuccessful()
    {
        return _Value.IsBitSet(13);
    }

    public bool WalletRequiresSecondTap()
    {
        return _Value.IsBitSet(17);
    }

    #endregion

    #region Serialization

    public static PosCardholderInteractionInformation Decode(ReadOnlyMemory<byte> value, BerCodec codec)
    {
        return Decode(value.Span, codec);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static PosCardholderInteractionInformation Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort byteLength = 3;

        if (value.Length != byteLength)
        {
            throw new
                ArgumentOutOfRangeException($"The Primitive Value {nameof(PosCardholderInteractionInformation)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<uint> result = codec.Decode(BerEncodingId, value) as DecodedResult<uint>
            ?? throw new
                InvalidOperationException($"The {nameof(PosCardholderInteractionInformation)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<uint>)}");

        return new PosCardholderInteractionInformation(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec)
    {
        return codec.EncodeValue(BerEncodingId, _Value);
    }

    public override byte[] EncodeValue(BerCodec codec, int length)
    {
        return codec.EncodeValue(BerEncodingId, _Value, length);
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

    public int GetHashCode(PosCardholderInteractionInformation obj)
    {
        return obj.GetHashCode();
    }

    #endregion
}