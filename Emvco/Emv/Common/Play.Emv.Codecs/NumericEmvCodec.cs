using System.Runtime.CompilerServices;

using Play.Ber.Codecs;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Codecs.Exceptions;

namespace Play.Emv.Codecs;

/// <summary>
///     Numeric data elements consist of two numeric digits (having values in the range Hex '0' – '9') per byte.
///     These digits are right justified and padded with leading hexadecimal zeroes. Other specifications sometimes
///     refer to this data format as Binary Coded Decimal (“BCD”) or unsigned packed.
///     Example: Amount, Authorized(Numeric) is defined as “n 12” with a length of six bytes.
///     A value of 12345 is stored in Amount, Authorized (Numeric) as Hex '00 00 00 01 23 45'.
/// </summary>

// TODO: Move the actual functionality higher up to Play.Codec
public class NumericEmvCodec : Codec
{
    #region Static Metadata

    private static readonly Numeric _Numeric = PlayEncoding.Numeric;

    #endregion

    #region Instance Members

    public override bool IsValid(ReadOnlySpan<byte> value) => _Numeric.IsValid(value);
    public override byte[] Encode<T>(T value) => _Numeric.GetBytes(value);
    public override byte[] Encode<T>(T value, int length) => _Numeric.GetBytes(value, length);
    public override byte[] Encode<T>(T[] value) => _Numeric.GetBytes(value);
    public override byte[] Encode<T>(T[] value, int length) => _Numeric.GetBytes(value, length);

    public override void Encode<T>(T value, Span<byte> buffer, ref int offset)
    {
        _Numeric.GetBytes(value, buffer, ref offset);
    }

    public override void Encode<T>(T value, int length, Span<byte> buffer, ref int offset)
    {
        _Numeric.GetBytes(value, length, buffer, ref offset);
    }

    public override void Encode<T>(T[] value, Span<byte> buffer, ref int offset)
    {
        _Numeric.GetBytes(value, buffer, ref offset);
    }

    public override void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset)
    {
        _Numeric.GetBytes(value, length, buffer, ref offset);
    }

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

    public void Validate(ReadOnlySpan<byte> value)
    {
        if (!IsValid(value))
            throw new EmvEncodingFormatException(new ArgumentOutOfRangeException(nameof(value)));
    }

    #endregion
}