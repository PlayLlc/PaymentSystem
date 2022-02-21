using System.Numerics;
using System.Runtime.CompilerServices;

using Play.Ber.Codecs;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Core.Specifications;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.Codecs;

/// <summary>
///     Numeric data elements consist of two numeric digits (having values in the range Hex '0' – '9') per byte.
///     These digits are right justified and padded with leading hexadecimal zeroes. Other specifications sometimes
///     refer to this data format as Binary Coded Decimal (“BCD”) or unsigned packed.
///     Example: Amount, Authorized(Numeric) is defined as “n 12” with a length of six bytes.
///     A value of 12345 is stored in Amount, Authorized (Numeric) as Hex '00 00 00 01 23 45'.
/// </summary>

// TODO: Move the actual functionality higher up to Play.Codec
public class NumericCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly Numeric _Numeric = PlayEncoding.Numeric;
    public static readonly BerEncodingId Identifier = GetBerEncodingId(typeof(NumericCodec));

    #endregion

    #region Instance Members

    public override BerEncodingId GetIdentifier() => Identifier;
    public override bool IsValid(ReadOnlySpan<byte> value) => _Numeric.IsValid(value);
    public override byte[] Encode<T>(T value) => _Numeric.GetBytes(value);
    public override byte[] Encode<T>(T value, int length) => _Numeric.GetBytes(value, length);
    public override byte[] Encode<T>(T[] value) => _Numeric.GetBytes(value);
    public override byte[] Encode<T>(T[] value, int length) => _Numeric.GetBytes(value, length);
    public byte[] Encode(ReadOnlySpan<char> value) => _Numeric.GetBytes(value, value.Length);
    public byte[] Encode(ReadOnlySpan<char> value, int length) => _Numeric.GetBytes(value, length);

    // TODO: why are you boxing you dweeb?
    public override ushort GetByteCount<T>(T value) => checked((ushort) Unsafe.SizeOf<T>());

    public override ushort GetByteCount<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return (ushort) checked((value.Length % 2) + (value.Length / 2));

        throw new NotImplementedException();
    }

    private static bool IsNibbleValid(byte value) => value is >= 0 and <= 9;
    private static bool IsValid(byte value) => _Numeric.IsValid(value);

    private static void Validate(byte value)
    {
        if (!IsValid(value))
            throw new EmvEncodingFormatException(new ArgumentOutOfRangeException(nameof(value)));
    }

    protected override void Validate(ReadOnlySpan<byte> value)
    {
        if (!IsValid(value))
            throw new EmvEncodingFormatException(new ArgumentOutOfRangeException(nameof(value)));
    }

    #endregion

    #region Serialization

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        Validate(value);

        ReadOnlySpan<byte> trimmedValue = value.TrimStart((byte) 0);
        BigInteger maximumIntegerResult = (BigInteger) Math.Pow(10, trimmedValue.Length - 1);

        if (value.Length == Specs.Integer.UInt8.ByteCount)
        {
            byte byteResult = _Numeric.GetByte(trimmedValue[0]);

            return new DecodedResult<byte>(byteResult, value.Length * 2);
        }

        if (value.Length <= Specs.Integer.UInt16.ByteCount)
        {
            ushort shortResult = _Numeric.GetUInt16(trimmedValue);

            return new DecodedResult<ushort>(shortResult, value.Length * 2);
        }

        if (value.Length <= Specs.Integer.UInt32.ByteCount)
        {
            uint intResult = _Numeric.GetUInt32(trimmedValue);

            return new DecodedResult<uint>(intResult, value.Length * 2);
        }

        if (value.Length <= Specs.Integer.UInt64.ByteCount)
        {
            ulong longResult = _Numeric.GetUInt64(trimmedValue);

            return new DecodedResult<ulong>(longResult, value.Length * 2);
        }

        BigInteger bigIntegerResult = _Numeric.GetBigInteger(trimmedValue);

        return new DecodedResult<BigInteger>(bigIntegerResult, value.Length * 2);
    }

    #endregion
}