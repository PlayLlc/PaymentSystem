using System;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Encryption.Certificates;

namespace Play.Emv.DataElements;

/// <summary>
///     ICC Public Key Exponent used for the verification of the Signed Dynamic Application Data
/// </summary>
public record IccPublicKeyExponent : DataElement<uint>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F47;

    #endregion

    #region Constructor

    public IccPublicKeyExponent(uint value) : base(value)
    {
        _Value = value;
    }

    public IccPublicKeyExponent(PublicKeyExponent value) : base((uint) value)
    { }

    #endregion

    #region Instance Members

    public PublicKeyExponent AsPublicKeyExponent() => PublicKeyExponent.Create(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(EncodingId, _Value);

    #endregion

    #region Serialization

    public static IccPublicKeyExponent Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IccPublicKeyExponent Decode(ReadOnlySpan<byte> value)
    {
        const ushort minByteLength = 1;
        const ushort maxByteLength = 3;

        if (value.Length is < minByteLength and <= maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(IccPublicKeyExponent)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {minByteLength}-{maxByteLength}");
        }

        DecodedResult<uint> result = _Codec.Decode(EncodingId, value) as DecodedResult<uint>
            ?? throw new InvalidOperationException(
                $"The {nameof(IccPublicKeyExponent)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<uint>)}");

        return new IccPublicKeyExponent(result.Value);
    }

    #endregion
}