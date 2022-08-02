using System;

using AutoFixture;

using Moq;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class CardRiskManagementDataObjectList1Tests
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly Mock<ITlvReaderAndWriter> _Database;

    #endregion

    #region Constructors

    public CardRiskManagementDataObjectList1Tests()
    {
        _Fixture = new EmvFixture().Create();
        _Database = new Mock<ITlvReaderAndWriter>();
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     BerEncoding_DeserializingDataElement_CreatesPrimitiveValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new();
        CardRiskManagementDataObjectList1 testValue = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(testValue);
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElement_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new();
        CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElementTlv_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new();
        CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[]? testValue = sut.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new();
        CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(CardRiskManagementDataObjectList1.Tag, testData.EncodeValue());
        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     TagLengthValue_SerializingToBer_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new();
        CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());

        byte[] testValue = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();
        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     InvalidBerEncoding_DeserializingDataElement_Throws
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void InvalidBerEncoding_DeserializingTagLengthWithOddNumberOfBytes_ThrowsIndexOutOfRangeException()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new(new byte[] { 0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01 });

        Assert.Throws<IndexOutOfRangeException>(() => CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new();
        CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetValueByteCount();
        ushort testResult = sut.GetValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    /// <summary>
    ///     DataElement_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new();
        CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    /// <summary>
    ///     CustomDataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void CustomDataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new(new byte[] { 8, 23 });
        CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetValueByteCount();
        ushort testResult = sut.GetValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    /// <summary>
    ///     CustomDataElement_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void CustomDataElement_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new(new byte[]
        {
            8, 13
        });

        CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void DataObjectListWithExistingTag_ChecksIfTagExists_ReturnsTrue()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new();

        CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());

        Tag expectedTag = testData.GetTags()[0];

        bool exists = sut.Exists(expectedTag);

        Assert.True(exists);
    }

    [Fact]
    public void DataObjectListMissingTag_ChecksIfTagExists_ReturnsFalse()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new();

        CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());

        Tag expectedTag = new Tag(37);

        bool exists = sut.Exists(expectedTag);

        Assert.False(exists);
    }

    [Fact]
    public void DataObjectList_IsRequestedDataAvailable_ReturnsExpectedResult()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new();

        CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());

        testData.SetupTlvTagsForGivenDb(_Database);

        bool isRequestedDataAvailable = sut.IsRequestedDataAvailable(_Database.Object);

        Assert.True(isRequestedDataAvailable);
    }

    [Fact]
    public void CustomDataObjectList_IsRequestedDataAvailable_ReturnsExpectedResult()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new();

        ReadOnlySpan<byte> encoded = stackalloc byte[] { 22, 8 };
        CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(encoded);

        testData.SetupTlvTagsForGivenDb(_Database);

        bool isRequestedDataAvailable = sut.IsRequestedDataAvailable(_Database.Object);

        Assert.False(isRequestedDataAvailable);
    }

    [Fact]
    public void DataObjectList_GetNeededData_ReturnsExpectedResult()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new();

        CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());

        testData.SetupTlvTagsForGivenDb(_Database);

        Tag[] neededData = sut.GetNeededData(_Database.Object);

        Assert.Equal(testData.GetTags(), neededData);
    }

    [Fact]
    public void DataObjectList_AsDataObjectListResult_ReturnsExpectedResult()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new();

        CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());

        testData.SetupTlvTagsForGivenDb(_Database);

        DataObjectListResult expected = testData.SetupValuesForTags(_Database, _Fixture);
        DataObjectListResult actual = sut.AsDataObjectListResult(_Database.Object);

        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void DataObjectList_AsCommandTemplate_CommandTemplateCreatedSuccesfully()
    {
        CardRiskManagementDataObjectList1TestTlv testData = new();

        CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());

        testData.SetupTlvTagsForGivenDb(_Database);

        DataObjectListResult objListResult = testData.SetupValuesForTags(_Database, _Fixture);
        CommandTemplate expected = objListResult.AsCommandTemplate();
        CommandTemplate actual = sut.AsCommandTemplate(_Database.Object);

        Assert.Equal(expected, actual);
    }

    #endregion
}
