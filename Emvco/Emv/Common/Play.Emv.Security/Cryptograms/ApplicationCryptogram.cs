using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.Codecs;
using Play.Icc.SecureMessaging;

namespace Play.Emv.Security.Cryptograms;

/// <summary>
///     Cryptogram returned by the ICC in response of the GENERATE AC command
/// </summary>
public record ApplicationCryptogram : CryptographicChecksum, IEqualityComparer<ApplicationCryptogram>
{
    #region Static Metadata

    public new static readonly BerEncodingId BerEncodingId = BinaryCodec.Identifier;
    public new static readonly Tag Tag = 0x9F26;
    private const byte _ByteCount = 8;

    #endregion

    #region Constructor

    public ApplicationCryptogram(ReadOnlySpan<byte> value) : base(value.ToArray())
    {
        if (value.Length != _ByteCount)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"The argument {nameof(value)} must be {_ByteCount} bytes in size to initialize a {nameof(ApplicationCryptogram)}");
        }
    }

    #endregion

    #region Instance Members

    public new BerEncodingId GetBerEncodingId() => BerEncodingId;
    public new Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static ApplicationCryptogram Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationCryptogram Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        if (value.Length != _ByteCount)
        {
            throw new
                ArgumentOutOfRangeException($"The Primitive Value {nameof(ApplicationCryptogram)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteCount} bytes in length");
        }

        return new ApplicationCryptogram(value);
    }

    public override byte[] EncodeValue(BerCodec codec) => _Value;

    public override byte[] EncodeValue(BerCodec codec, int length)
    {
        if (length == _Value.Length)
            return _Value;

        if (length > _Value.Length)
        {
            Span<byte> buffer = stackalloc byte[length];
            _Value.CopyTo(buffer);

            return buffer.ToArray();
        }

        return _Value[..length];
    }

    #endregion

    #region Equality

    public bool Equals(ApplicationCryptogram? x, ApplicationCryptogram? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationCryptogram obj) => obj.GetHashCode();

    #endregion
}