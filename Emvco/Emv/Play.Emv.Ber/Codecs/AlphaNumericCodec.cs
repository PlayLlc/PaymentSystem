﻿using System.Runtime.CompilerServices;

using Play.Ber.Codecs;
using Play.Ber.Emv.Exceptions;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Codecs.Strings;
using Play.Core.Exceptions;

namespace Play.Ber.Emv.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class AlphaNumericCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly AlphaNumeric _AlphaNumeric = PlayEncoding.AlphaNumeric;
    public static readonly BerEncodingId Identifier = GetBerEncodingId(typeof(AlphaNumericCodec));

    #endregion

    #region Instance Members

    public override BerEncodingId GetIdentifier()
    {
        return Identifier;
    }

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        return _AlphaNumeric.IsValid(value);
    }

    public override byte[] Encode<T>(T value)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T, char>(ref value));

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    public override byte[] Encode<T>(T value, int length)
    {
        CheckCore.ForRange(length, 1, 1, nameof(length));

        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T, char>(ref value));

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    /// <exception cref="EncodingException"></exception>
    public override byte[] Encode<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan());

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    /// <exception cref="EncodingException"></exception>
    public override byte[] Encode<T>(T[] value, int length)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), length);

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    /// <exception cref="EncodingException"></exception>
    public static byte[] Encode(ReadOnlySpan<char> value)
    {
        return _AlphaNumeric.GetBytes(value);
    }

    /// <exception cref="EncodingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value, int length)
    {
        if (value.Length == length)
            return Encode(value);

        if (length > value.Length)
        {
            Span<byte> buffer = stackalloc byte[length];
            _AlphaNumeric.GetBytes(value).AsSpan().CopyTo(buffer);

            return buffer.ToArray();
        }

        return _AlphaNumeric.GetBytes(value)[..length];
    }

    public byte[] Encode(string value)
    {
        return Encode(value.AsSpan());
    }

    public override ushort GetByteCount<T>(T value)
    {
        return 1;
    }

    public override ushort GetByteCount<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return checked((ushort) value.Length);

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    /// <exception cref="EncodingException"></exception>
    protected override void Validate(ReadOnlySpan<byte> value)
    {
        if (!_AlphaNumeric.IsValid(value))
            throw new EmvEncodingFormatException(EncodingException.CharacterArrayContainsInvalidValue);
    }

    #endregion

    #region Serialization

    public override DecodedResult<char[]> Decode(ReadOnlySpan<byte> value)
    {
        return new(_AlphaNumeric.GetChars(value), value.Length);
    }

    #endregion
}