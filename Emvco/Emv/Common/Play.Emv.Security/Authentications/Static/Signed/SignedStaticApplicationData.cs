using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.Codecs;

namespace Play.Emv.Security.Authentications.Static.Signed;

/// <summary>
///     Generated by the issuer using the private key that corresponds to the public key authenticated in the Issuer Public
///     Key
///     Certificate.It is a digital signature covering critical ICC-resident static data elements, as described in section
///     5.4.
/// </summary>
public record SignedStaticApplicationData : PrimitiveValue, IEqualityComparer<SignedStaticApplicationData>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryCodec.Identifier;
    public static readonly Tag Tag = 0x93;

    #endregion

    #region Instance Values

    private readonly byte[] _Value;

    #endregion

    #region Constructor

    public SignedStaticApplicationData(byte[] value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => _Value;
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public int GetByteCount() => _Value.Length;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static SignedStaticApplicationData Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static SignedStaticApplicationData Decode(ReadOnlySpan<byte> value, BerCodec codec) => new(value.ToArray());

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(SignedStaticApplicationData? x, SignedStaticApplicationData? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(SignedStaticApplicationData obj) => obj.GetHashCode();

    #endregion
}