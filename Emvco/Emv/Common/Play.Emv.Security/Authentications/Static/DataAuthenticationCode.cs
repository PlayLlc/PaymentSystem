using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;

namespace Play.Emv.Security.Authentications.Static;

/// <summary>
///     An issuer assigned value that is retained by the terminal during the verification process of the Signed Static
///     Application Data
/// </summary>
public record DataAuthenticationCode : PrimitiveValue, IEqualityComparer<DataAuthenticationCode>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F45;

    #endregion

    #region Instance Values

    private readonly ushort _Value;

    #endregion

    #region Constructor

    public DataAuthenticationCode(ushort value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public byte[] AsBytes()
    {
        return new[] {(byte) (_Value >> 8), (byte) _Value};
    }

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static DataAuthenticationCode Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static DataAuthenticationCode Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort byteLength = 2;

        if (value.Length != byteLength)
        {
            throw new
                ArgumentOutOfRangeException($"The Primitive Value {nameof(DataAuthenticationCode)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<ushort> result = codec.Decode(BerEncodingId, value) as DecodedResult<ushort>
            ?? throw new
                InvalidOperationException($"The {nameof(DataAuthenticationCode)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        return new DataAuthenticationCode(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(DataAuthenticationCode? x, DataAuthenticationCode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DataAuthenticationCode obj) => obj.GetHashCode();

    #endregion
}