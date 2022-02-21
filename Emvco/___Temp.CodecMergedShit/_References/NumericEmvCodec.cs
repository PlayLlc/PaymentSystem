using System.Numerics;
using System.Runtime.CompilerServices;

using ___Temp.CodecMergedShit.Metadata;

using Play.Ber.Codecs;
using Play.Codecs;
using Play.Core.Specifications;

namespace ___Temp.CodecMergedShit._References;

/// <summary>
///     Numeric data elements consist of two numeric digits (having values in the range Hex '0' – '9') per byte.
///     These digits are right justified and padded with leading hexadecimal zeroes. Other specifications sometimes
///     refer to this data format as Binary Coded Decimal (“BCD”) or unsigned packed.
///     Example: Amount, Authorized(Numeric) is defined as “n 12” with a length of six bytes.
///     A value of 12345 is stored in Amount, Authorized (Numeric) as Hex '00 00 00 01 23 45'.
/// </summary>

// TODO: Move the actual functionality higher up to Play.Codec
public class NumericEmvCodec : IPlayCodec
{
    #region Metadata

    private static readonly Numeric _Numeric = PlayEncoding.Numeric;

    #endregion

    #region Count

    public ushort GetByteCount<T>(T value) where T : struct => checked((ushort) Unsafe.SizeOf<T>());

    public ushort GetByteCount<T>(T[] value) where T : struct
    {
        if (typeof(T) == typeof(char))
            return (ushort) checked((value.Length % 2) + (value.Length / 2));

        throw new NotImplementedException();
    }

    #endregion

    #region Validation

    private static void Validate(byte value)
    {
        if (!IsValid(value))
            throw new EmvEncodingFormatException(new ArgumentOutOfRangeException(nameof(value)));
    }

    public void Validate(ReadOnlySpan<byte> value)
    {
        if (!IsValid(value))
            throw new EmvEncodingFormatException(new ArgumentOutOfRangeException(nameof(value)));
    }

    #endregion

    #region Decode

    public DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        Validate(value);

        ReadOnlySpan<byte> trimmedValue = value.TrimStart((byte) 0);

        if (value.Length == Specs.Integer.UInt8.ByteSize)
        {
            byte byteResult = PlayEncoding.Numeric.GetByte(trimmedValue[0]);

            return new DecodedResult<byte>(byteResult, value.Length * 2);
        }

        if (value.Length <= Specs.Integer.UInt16.ByteSize)
        {
            ushort shortResult = PlayEncoding.Numeric.GetUInt16(trimmedValue);

            return new DecodedResult<ushort>(shortResult, value.Length * 2);
        }

        if (value.Length <= Specs.Integer.UInt32.ByteSize)
        {
            uint intResult = PlayEncoding.Numeric.GetUInt32(trimmedValue);

            return new DecodedResult<uint>(intResult, value.Length * 2);
        }

        if (value.Length <= Specs.Integer.UInt64.ByteCount)
        {
            ulong longResult = PlayEncoding.Numeric.GetUInt64(trimmedValue);

            return new DecodedResult<ulong>(longResult, value.Length * 2);
        }

        BigInteger bigIntegerResult = PlayEncoding.Numeric.GetBigInteger(trimmedValue);

        return new DecodedResult<BigInteger>(bigIntegerResult, value.Length * 2);
    }

    #endregion

    // TODO: why are you boxing you dweeb?
}