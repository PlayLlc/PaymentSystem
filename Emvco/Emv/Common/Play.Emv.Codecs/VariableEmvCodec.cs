using System.Runtime.CompilerServices;

using Play.Ber.Codecs;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Codecs.Exceptions;

namespace Play.Emv.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class VariableEmvCodec : Codec
{
    #region Static Metadata

    private static readonly Binary _BinaryCodec = new();

    #endregion

    #region Instance Members

    public override bool IsValid(ReadOnlySpan<byte> value) => _BinaryCodec.IsValid(value);

    protected void Validate(ReadOnlySpan<byte> value)
    {
        _BinaryCodec.IsValid(value);
    }

    public override byte[] Encode<T>(T value) =>
        throw new InternalEmvEncodingException(
            $"The {nameof(VariableEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");

    public override byte[] Encode<T>(T value, int length) =>
        throw new InternalEmvEncodingException(
            $"The {nameof(VariableEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");

    public override byte[] Encode<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value));
        else if (typeof(T) == typeof(byte))
            return Unsafe.As<T[], byte[]>(ref value);
        else
        {
            throw new InternalEmvEncodingException(
                $"The {nameof(VariableEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }
    }

    public override byte[] Encode<T>(T[] value, int length)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value), length);
        else if (typeof(T) == typeof(byte))
            return Unsafe.As<T[], byte[]>(ref value)[..length];
        else
        {
            throw new InternalEmvEncodingException(
                $"The {nameof(VariableEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }
    }

    public override void Encode<T>(T value, Span<byte> buffer, ref int offset)
    {
        throw new InternalEmvEncodingException(
            $"The {nameof(VariableEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    public override void Encode<T>(T value, int length, Span<byte> buffer, ref int offset)
    {
        throw new InternalEmvEncodingException(
            $"The {nameof(VariableEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    public override void Encode<T>(T[] value, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value), buffer, ref offset);
        else if (typeof(T) == typeof(byte))
            Unsafe.As<T[], byte[]>(ref value).CopyTo(buffer[offset..]);
        else
        {
            throw new InternalEmvEncodingException(
                $"The {nameof(VariableEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }
    }

    public override void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value), length, buffer, ref offset);
        else if (typeof(T) == typeof(byte))
        {
            Unsafe.As<T[], byte[]>(ref value)[..length].CopyTo(buffer[offset..(offset + length)]);
            offset += length;
        }
        else
        {
            throw new InternalEmvEncodingException(
                $"The {nameof(VariableEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }
    }

    public byte[] Encode(ReadOnlySpan<char> value) => _BinaryCodec.GetBytes(value);
    public byte[] Encode(ReadOnlySpan<char> value, int length) => _BinaryCodec.GetBytes(value[..length]);

    public void Encode(ReadOnlySpan<char> value, Span<byte> buffer, ref int offset)
    {
        _BinaryCodec.GetBytes(value, buffer, ref offset);
    }

    public void Encode(ReadOnlySpan<char> value, int length, Span<byte> buffer, ref int offset)
    {
        _BinaryCodec.GetBytes(value[..length], buffer, ref offset);
    }

    public override ushort GetByteCount<T>(T value) => throw new NotImplementedException();
    public override ushort GetByteCount<T>(T[] value) => (ushort) (value.Length * Unsafe.SizeOf<T>());

    #endregion

    #region Serialization

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        char[] valueResult = PlayEncoding.Binary.GetChars(value);

        return new DecodedResult<char[]>(valueResult, valueResult.Length);
    }

    #endregion
}