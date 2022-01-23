namespace Play.Ber.Lengths.Indefinite;

// TODO: ISO 7816 doesn't use indefinite length so we're skipping this for now

///// <summary>
/////		For the indefinite form, the length octets indicate that the contents octets are terminated by end-of-contents
/////		octets, and shall consist of a single octet
///// </summary>
///// <remarks>
/////
///// </remarks>
//internal readonly ref struct IndefiniteLength
//{
//	/// <summary>
//	///		Not sure why the Length value would equal the raw value of an indefinite flag, but that'_Stream how they
//	///		represent it in the example in the remarks section below
//	/// </summary>
//	/// <remarks>
//	///		[ISO8825-1] 8.6.4.2 Example
//	/// </remarks>
//	public const byte ContentByteCount = Spec.IndefiniteLength.RawValue;

//	/// <summary>
//	///		ctor
//	/// </summary>
//	/// <param name="value"></param>
//	public IndefiniteLength(ReadOnlySpan<byte> value)
//	{
//		Validate(value);
//	}

//	/// <summary>
//	/// Validate
//	/// </summary>
//	/// <param name="value"></param>
//	/// <exception cref="BerException"></exception>
//	private void Validate(ReadOnlySpan<byte> value)
//	{
//		Check.ForExactLength(value, 1, nameof(value));

//		if(!IsIndefiniteLengthFlagPresent(value))
//			throw new BerException(new ArgumentOutOfRangeException(nameof(value)));
//	}

//	public static bool IsValid(ReadOnlySpan<byte> value)
//	{
//		return value.Length == 1 && IsIndefiniteLengthFlagPresent(value);
//	}

//	/// <remarks>
//	///		[ISO8825-1] 8.1.3.6.1
//	/// </remarks>
//	/// <param name="value"></param>
//	/// <returns></returns>
//	private static bool IsIndefiniteLengthFlagPresent(ReadOnlySpan<byte> value)
//	{
//		return value[0] == Spec.IndefiniteLength.RawValue;
//	}

//	public byte Serialize()
//	{
//		return Spec.IndefiniteLength.RawValue;
//	}
//}

//public static class IndefiniteLength
//{
//    /// <summary>
//    /// </summary>
//    /// <remarks>
//    ///     <see cref="ISO8825.Part1" /> 8.1.3.6
//    /// </remarks>
//    public const byte RawValue = (byte)BitCount.Eight;
//}