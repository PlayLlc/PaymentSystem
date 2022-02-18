using System;
using System.Runtime.CompilerServices;

using Play.Ber.InternalFactories;
using Play.Core.Exceptions;

namespace Play.Ber.Codecs;

public sealed class BooleanBerCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly byte[] _DefaultEncodedFalse = {0x00};
    private static readonly byte[] _DefaultEncodedTrue = {0xFF};
    public static readonly BerEncodingId Identifier = GetBerEncodingId(typeof(BooleanBerCodec));

    #endregion

    #region Instance Members

    public override BerEncodingId GetIdentifier() => Identifier;
    public override bool IsValid(ReadOnlySpan<byte> value) => value.Length == 1;

    protected override void Validate(ReadOnlySpan<byte> value)
    {
        CheckCore.ForExactLength(value, 1, nameof(value));
    }

    /// <exception cref="InvalidOperationException">Ignore.</exception>
    public override byte[] Encode<T>(T value)
    {
        if (typeof(T) != typeof(bool))
            throw new InvalidOperationException();

        return Encode(Unsafe.As<T, bool>(ref value));
    }

    // TODO: this is only in BER library so holding off on implementing this
    public override byte[] Encode<T>(T value, int length) => throw new NotImplementedException();

    // TODO: this is only in BER library so holding off on implementing this
    public override byte[] Encode<T>(T[] value, int length) => throw new NotImplementedException();

    public override void Encode<T>(T value, Span<byte> buffer, ref int offset)
    {
        throw new NotImplementedException();
    }

    public override void Encode<T>(T value, int length, Span<byte> buffer, ref int offset)
    {
        throw new NotImplementedException();
    }

    public override void Encode<T>(T[] value, Span<byte> buffer, ref int offset)
    {
        throw new NotImplementedException();
    }

    public override void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset)
    {
        throw new NotImplementedException();
    }

    public override byte[] Encode<T>(T[] value) => throw new NotImplementedException();
    public byte[] Encode(bool value) => value ? _DefaultEncodedTrue : _DefaultEncodedFalse;

    public override ushort GetByteCount<T>(T value)
    {
        if (typeof(T) != typeof(bool))
            return 1;

        throw new NotImplementedException();
    }

    // TODO: this is only in BER library so holding off on implementing this
    public override ushort GetByteCount<T>(T[] value) => throw new NotImplementedException();

    #endregion

    #region Serialization

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        Validate(value);

        return new DecodedResult<bool>(value[0] != 0, 1);
    }

    #endregion
}