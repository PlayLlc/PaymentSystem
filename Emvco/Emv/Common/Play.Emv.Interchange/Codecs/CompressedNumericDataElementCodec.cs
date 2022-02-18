using System.Numerics;

using Play.Codecs;
using Play.Emv.Codecs;
using Play.Interchange.Codecs;

namespace Play.Emv.Ber.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class CompressedNumericDataElementCodec : InterchangeDataFieldCodec
{
    #region Static Metadata

    private static readonly CompressedNumericEmvCodec _Codec = new();
    public static readonly InterchangeEncodingId Identifier = GetEncodingId(typeof(AlphabeticDataElementCodec));

    #endregion

    #region Instance Members

    public override InterchangeEncodingId GetIdentifier() => Identifier;
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
        _Codec.Validate(value);

        ReadOnlySpan<byte> trimmedValue = _Codec.TrimTrailingBytes(value);
        BigInteger maximumIntegerResult = (BigInteger) Math.Pow(2, value.Length * 8);

        if (maximumIntegerResult <= byte.MaxValue)
        {
            byte byteResult = _Codec.DecodeByte(trimmedValue[0]);

            return new DecodedResult<byte>(byteResult, PlayEncoding.UnsignedInteger.GetCharCount(byteResult));
        }

        if (maximumIntegerResult <= ushort.MaxValue)
        {
            ushort shortResult = _Codec.DecodeUInt16(trimmedValue);

            return new DecodedResult<ushort>(shortResult, PlayEncoding.UnsignedInteger.GetCharCount(shortResult));
        }

        if (maximumIntegerResult <= uint.MaxValue)
        {
            uint intResult = _Codec.DecodeUInt32(trimmedValue);

            return new DecodedResult<uint>(intResult, PlayEncoding.UnsignedInteger.GetCharCount(intResult));
        }

        if (maximumIntegerResult <= ulong.MaxValue)
        {
            ulong longResult = _Codec.DecodeUInt64(trimmedValue);

            return new DecodedResult<ulong>(longResult, PlayEncoding.UnsignedInteger.GetCharCount(longResult));
        }

        BigInteger bigIntegerResult = _Codec.DecodeBigInteger(trimmedValue);

        return new DecodedResult<BigInteger>(bigIntegerResult, PlayEncoding.UnsignedInteger.GetCharCount(bigIntegerResult));
    }

    #endregion
}