using System.Numerics;
using System.Runtime.CompilerServices;

using Play.Ber.Codecs;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Integers;
using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Emv.Codecs;
using Play.Emv.Codecs.Exceptions;

namespace Play.Emv.Ber.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class BinaryDataElementCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly BinaryEmvCodec _Codec = new();
    public static readonly BerEncodingId Identifier = GetBerEncodingId(typeof(BinaryDataElementCodec));

    #endregion

    #region Instance Members

    public override BerEncodingId GetIdentifier() => Identifier;
    public override ushort GetByteCount<T>(T value) => _Codec.GetByteCount(value);
    public override ushort GetByteCount<T>(T[] value) => _Codec.GetByteCount(value);
    public override bool IsValid(ReadOnlySpan<byte> value) => _Codec.IsValid(value);
    public override byte[] Encode<T>(T value) => _Codec.Encode(value);
    public override byte[] Encode<T>(T value, int length) => _Codec.Encode(value);
    public override byte[] Encode<T>(T[] value) => _Codec.Encode(value);
    public override byte[] Encode<T>(T[] value, int length) => _Codec.Encode(value);
    public override void Encode<T>(T value, Span<byte> buffer, ref int offset) => _Codec.Encode(value, buffer, ref offset);

    public override void Encode<T>(T value, int length, Span<byte> buffer, ref int offset) =>
        _Codec.Encode(value, length, buffer, ref offset);

    public override void Encode<T>(T[] value, Span<byte> buffer, ref int offset) => _Codec.Encode(value, buffer, ref offset);

    public override void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset) =>
        _Codec.Encode(value, length, buffer, ref offset);

    #endregion

    #region Serialization

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length <= Specs.Integer.UInt8.ByteSize)
            return new DecodedResult<byte>(value[0], value[0].GetNumberOfDigits());
        if (value.Length <= Specs.Integer.UInt16.ByteSize)
            return new DecodedResult<ushort>(PlayEncoding.UnsignedInteger.GetUInt16(value), value[0].GetNumberOfDigits());
        if (value.Length <= Specs.Integer.UInt32.ByteSize)
            return new DecodedResult<uint>(PlayEncoding.UnsignedInteger.GetUInt32(value), value[0].GetNumberOfDigits());
        if (value.Length <= Specs.Integer.UInt64.ByteCount)
            return new DecodedResult<ulong>(PlayEncoding.UnsignedInteger.GetUInt64(value), value[0].GetNumberOfDigits());

        return new DecodedResult<BigInteger>(PlayEncoding.UnsignedInteger.GetBigInteger(value), value[0].GetNumberOfDigits());
    }

    #endregion
}