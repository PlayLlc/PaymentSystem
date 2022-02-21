using System;

using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Integers;
using Play.Codecs.Metadata;

namespace Play.Ber.Codecs;

public class BinaryIntegerBerCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly UnsignedInteger _Codec = PlayEncoding.UnsignedInteger;
    public static readonly BerEncodingId Identifier = GetBerEncodingId(typeof(BinaryIntegerBerCodec));

    #endregion

    #region Instance Members

    public override BerEncodingId GetIdentifier() => Identifier;
    public override bool IsValid(ReadOnlySpan<byte> value) => _Codec.IsValid(value);

    /// <summary>
    ///     Validate
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InvalidOperationException"></exception>
    protected override void Validate(ReadOnlySpan<byte> value)
    {
        if (!_Codec.IsValid(value))
            throw new BerFormatException($"The argument {nameof(value)} contains an invalid {nameof(BinaryIntegerBerCodec)} encoding");
    }

    public override byte[] Encode<T>(T value) => throw new NotImplementedException();
    public override byte[] Encode<T>(T value, int length) => throw new NotImplementedException();
    public override byte[] Encode<T>(T[] value) => throw new NotImplementedException();
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

    public override ushort GetByteCount<T>(T value) => throw new NotImplementedException();
    public override ushort GetByteCount<T>(T[] value) => throw new NotImplementedException();

    #endregion

    #region Serialization

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value) => throw new NotImplementedException();

    #endregion
}