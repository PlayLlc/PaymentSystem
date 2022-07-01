using System.Numerics;

using Play.Codecs.Exceptions;
using Play.Core.Specifications;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Codecs.Tests.Tests.UnsignedIntegers;

public class UnsignedIntegerTests : TestBase
{
    #region Instance Values

    private readonly UnsignedIntegerCodec _SystemUnderTest;

    #endregion

    #region Constructor

    public UnsignedIntegerTests()
    {
        _SystemUnderTest = PlayCodec.UnsignedIntegerCodec;
    }

    #endregion

    #region Encoding <-> Decoding

    /// <param name="expected"></param>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="Exceptions.CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomBytes), 50, 1, 300, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] expected)
    {
        string? decoded = _SystemUnderTest.DecodeToString(expected);
        byte[]? actual = _SystemUnderTest.Encode(decoded);

        Assertion(() => { Assert.Equal(expected, actual!); }, Build.Equals.Message(expected, actual!));
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomString), 50, 1, 300, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomStringEncoding_EncodingThenDecoding_ReturnsExpectedResult(string expected)
    {
        byte[]? encoded = _SystemUnderTest.Encode(expected);
        string? decoded = _SystemUnderTest.DecodeToString(encoded);
        //does not take leading 0s into account
        Assertion(() => { Assert.Equal(expected, decoded!); }, Build.Equals.Message(expected, decoded!));
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomUShort), 50, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomDecodedUShort_EncodingThenDecoding_ReturnsExpectedResult(ushort expected)
    {
        byte[]? decoded = _SystemUnderTest.Encode(expected);
        ushort actual = _SystemUnderTest.DecodeToUInt16(decoded);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomBytes), 100, 1, Specs.Integer.UInt16.ByteCount + 1, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomEncodedUShort_DecodingThenEncoding_ReturnsExpectedResult(byte[] expected)
    {
        ushort decoded = _SystemUnderTest.DecodeToUInt16(expected);
        byte[]? encoded = _SystemUnderTest.Encode(decoded, true);
        
        Assert.Equal(expected, encoded);
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomUInt), 50, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomDecodedUInt_EncodingThenDecoding_ReturnsExpectedResult(uint expected)
    {
        byte[]? decoded = _SystemUnderTest.Encode(expected);
        uint actual = _SystemUnderTest.DecodeToUInt32(decoded);

        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomBytes), 100, 1, Specs.Integer.UInt32.ByteCount + 1, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomEncodedUInt_DecodingThenEncoding_ReturnsExpectedResult(byte[] expected)
    {
        uint decoded = _SystemUnderTest.DecodeToUInt32(expected);
        byte[]? encoded = _SystemUnderTest.Encode(decoded, true);
        
        Assertion(() => { Assert.Equal(expected, encoded); }, Build.Equals.Message(expected, encoded));
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomULong), 50, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomDecodedULong_EncodingThenDecoding_ReturnsExpectedResult(ulong expected)
    {
        byte[]? decoded = _SystemUnderTest.Encode(expected);
        ulong actual = _SystemUnderTest.DecodeToUInt64(decoded);

        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomBytes), 100, 1, Specs.Integer.UInt64.ByteCount + 1, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomEncodedULong_DecodingThenEncoding_ReturnsExpectedResult(byte[] expected)
    {
        ulong decoded = _SystemUnderTest.DecodeToUInt64(expected);
        byte[]? encoded = _SystemUnderTest.Encode(decoded, true);
        
        Assertion(() => { Assert.Equal(expected, encoded); }, Build.Equals.Message(expected, encoded));
    }

    #endregion

    #region GetByteCount

    [Fact]
    public void ValidOddString_GetByteCount_ReturnsExpectedResult()
    {
        char[] testData = { '1', '2', '3', '4', '5' };
        int expected = 2;
        int actual = _SystemUnderTest.GetByteCount(testData);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ValidEvenString_GetByteCount_ReturnsExpectedResult()
    {
        char[] testData = { '1', '2', '3', '4', '5', '6' };
        int expected = 3;
        int actual = _SystemUnderTest.GetByteCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion

    #region GetCharCount

    [Fact]
    public void Byte_GetCharCount_ReturnsExpectedResult()
    {
        byte testData = 56 ;
        nint expected = 2;
        nint actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void Ushort_GetCharCount_ReturnsExpectedResult()
    {
        ushort testData = 1234;
        nint expected = 4;
        nint actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void UInt_GetCharCount_ReturnsExpectedResult()
    {
        uint testData = 1234567;
        nint expected = 7;
        nint actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ULong_GetCharCount_ReturnsExpectedResult()
    {
        ulong testData = 1234567891011;
        nint expected = 13;
        nint actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void BigInteger_GetCharCount_ReturnsExpectedResult()
    {
        BigInteger testData = 123441231133;
        nint expected = 13;
        nint actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion

    #region Encode BigInteger

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomBigInteger), 50, MemberType = typeof(UnsignedIntegerFixture))]
    public void BigInteger_Encode_ReturnsExpectedResult(BigInteger testData)
    {
        byte[] expected = testData.ToByteArray();
        byte[] actual = _SystemUnderTest.Encode(testData);

        Assert.Equal(expected, actual);
    }

    #endregion

    #region DecodeToBigInteger

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomByteArray_DecodeToBigInteger__ReturnsExpectedResult(byte[] input)
    {
        BigInteger expected = new(input);
        BigInteger actual = _SystemUnderTest.DecodeToBigInteger(input);

        Assert.Equal(expected, actual);
    }

    #endregion

    #region DecodeToByte

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomByteArray_DecodeToByte_TheFirstByteIsReturned(byte[] input)
    {
        byte expected = input[0];
        byte actual = _SystemUnderTest.DecodeToByte(input);

        Assert.Equal(expected, actual);
    }

    #endregion

    #region Decode To DecodeToMetaData

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomBytes), 100, 1, Specs.Integer.UInt8.ByteCount + 1, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomByteArray_DecodeToMetadataInputByteCountUInt8_DecodesExpectedResult(byte[] input)
    {
        DecodedMetadata metadata = _SystemUnderTest.Decode(input);

        DecodedResult<byte> result = metadata.ToByteResult();

        Assert.NotNull(result);
        Assert.Equal(input[0], result.Value);
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomBytes), 100, 1, Specs.Integer.UInt16.ByteCount+1, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomByteArray_DecodeToMetadataInputByteCountUInt16_DecodesExpectedResult(byte[] input)
    {
        DecodedMetadata metadata = _SystemUnderTest.Decode(input);

        DecodedResult<ushort> result = metadata.ToUInt16Result();

        Assert.NotNull(result);
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomBytes), 100, 1, Specs.Integer.UInt32.ByteCount + 1, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomByteArray_DecodeToMetadataInputByteCountUInt32_DecodesExpectedResult(byte[] input)
    {
        DecodedMetadata metadata = _SystemUnderTest.Decode(input);

        DecodedResult<uint> result = metadata.ToUInt32Result();

        Assert.NotNull(result);
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomBytes), 100, 1, Specs.Integer.UInt64.ByteCount + 1, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomByteArray_DecodeToMetadataInputByteCountUInt64_DecodesExpectedResult(byte[] input)
    {
        DecodedMetadata metadata = _SystemUnderTest.Decode(input);

        DecodedResult<ulong> result = metadata.ToUInt64Result();

        Assert.NotNull(result);
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomBytes), 100, 1, Specs.Integer.UInt64.ByteCount + 10, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomByteArray_DecodeToMetadataInputByteCountBigInteger_DecodesExpectedResult(byte[] input)
    {
        DecodedMetadata metadata = _SystemUnderTest.Decode(input);

        DecodedResult<BigInteger> result = metadata.ToBigInteger();

        Assert.NotNull(result);
    }

    #endregion

    #region IsValid

    [Fact]
    public void ValidByteArray_InvokingIsValid_ReturnsTrue()
    {
        byte[] testData = new byte[] { 0x12, 0x34, 0x56 };
        Assertion(() => Assert.True(_SystemUnderTest.IsValid(testData)));
    }

    [Fact]
    public void InvalidString_InvokingIsValid_ReturnsFalse()
    {
        string testData = "534C34";
        Assertion(() => Assert.False(_SystemUnderTest.IsValid(testData)));
    }

    [Fact]
    public void ValidString_InvokingIsValid_ReturnsFalse()
    {
        string testData = "534C34";
        Assertion(() => Assert.False(_SystemUnderTest.IsValid(testData)));
    }

    #endregion
}