using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;

using HexadecimalCodec = Play.Ber.Codecs.HexadecimalCodec;
using PlayEncodingId = Play.Codecs.PlayEncodingId;

namespace Play.Icc.SecureMessaging;

/// <summary>
///     A <see cref="ConstructedValue" /> used as a wrapper a cryptographic checksum value
/// </summary>
/// <remarks>
///     ISO 7816-4 Section 5.6.3.1 Table 20
/// </remarks>
public record CryptographicChecksum : PrimitiveValue, IEqualityComparer<CryptographicChecksum>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = HexadecimalCodec.EncodingId;
    public static readonly Tag Tag = 0x8E;
    protected const byte _MinByteCount = 4;
    protected const byte _MaxByteCount = 8;

    #endregion

    #region Instance Values

    protected readonly byte[] _Value;

    #endregion

    #region Constructor

    public CryptographicChecksum(ReadOnlySpan<byte> value)
    {
        if (value.Length < _MinByteCount)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} must be between {_MinByteCount} and {_MaxByteCount}");
        }

        if (value.Length < _MaxByteCount)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} must be between {_MinByteCount} and {_MaxByteCount}");
        }

        _Value = value.ToArray();
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => _Value;
    public virtual TagLengthValue AsTagLengthValue() => AsTagLengthValue(GetTag());
    protected TagLengthValue AsTagLengthValue(Tag tag) => new(tag, _Value);
    public override PlayEncodingId GetBerEncodingId() => PlayEncodingId;
    public int GetByteCount() => _Value.Length;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => checked((ushort) _Value.Length);

    #endregion

    #region Serialization

    public static CryptographicChecksum Decode(ReadOnlySpan<byte> value) => new(value);
    public override byte[] EncodeValue(BerCodec codec) => _Value;

    /// <summary>
    ///     EncodeValue
    /// </summary>
    /// <param name="codec"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override byte[] EncodeValue(BerCodec codec, int length)
    {
        if (length > _Value.Length)
            throw new InvalidOperationException($"The argument {nameof(length)} is larger than the underlying value");

        return _Value[..length];
    }

    #endregion

    #region Equality

    public bool Equals(CryptographicChecksum? x, CryptographicChecksum? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CryptographicChecksum obj) => obj.GetHashCode();

    #endregion
}