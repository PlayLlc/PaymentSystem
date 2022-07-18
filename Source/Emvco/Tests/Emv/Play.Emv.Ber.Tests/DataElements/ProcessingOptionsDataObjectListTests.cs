using System;

using AutoFixture;

using Moq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class ProcessingOptionsDataObjectListTests : TestBase
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly Mock<ITlvReaderAndWriter> _Database;

    #endregion

    #region Constructor

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public ProcessingOptionsDataObjectListTests()
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
        ProcessingOptionsDataObjectListTestTlv testData = new();
        ProcessingOptionsDataObjectList testValue = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
        Assertion(() => Assert.NotNull(testValue));
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElement_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        ProcessingOptionsDataObjectListTestTlv testData = new();
        ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
        byte[] expected = testData.EncodeValue();
        byte[]? actual = sut.EncodeValue();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        ;
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElementTlv_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        ProcessingOptionsDataObjectListTestTlv testData = new();
        ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
        byte[] expected = testData.EncodeTagLengthValue();
        byte[]? actual = sut.EncodeTagLengthValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    /// <summary>
    ///     BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        ProcessingOptionsDataObjectListTestTlv testData = new();
        ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? actual = sut.AsTagLengthValue();
        TagLengthValue expected = new(ProcessingOptionsDataObjectList.Tag, testData.EncodeValue());
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    /// <summary>
    ///     TagLengthValue_SerializingToBer_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        ProcessingOptionsDataObjectListTestTlv testData = new();

        ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
        byte[] actual = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expected = testData.EncodeTagLengthValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    /// <summary>
    ///     TagLengthValue_SerializingToBer_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void PrimitiveValue_GetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        ProcessingOptionsDataObjectListTestTlv testData = new();

        ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
        int expected = testData.GetTagLengthValueByteCount();
        int actual = sut.GetTagLengthValueByteCount();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void PrimitiveValue_GetValueByteCount_ReturnsExpectedResult()
    {
        ProcessingOptionsDataObjectListTestTlv testData = new();

        ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
        int expected = testData.GetValueByteCount();
        int actual = sut.GetValueByteCount();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void PrimitiveValue_EncodingTagLengthValue_ReturnsExpectedResult()
    {
        byte[] expected = EmvFixture.ProcessingOptionsDataObjectListBuilder.GetDefaultEncodedTagLengthValue();
        ProcessingOptionsDataObjectList sut = _Fixture.Create<ProcessingOptionsDataObjectList>();
        byte[] actual = sut.EncodeTagLengthValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void PrimitiveValue_EncodingValue_ReturnsExpectedResult()
    {
        byte[] expected = EmvFixture.ProcessingOptionsDataObjectListBuilder.GetDefaultEncodedValue();
        ProcessingOptionsDataObjectList sut = _Fixture.Create<ProcessingOptionsDataObjectList>();
        byte[] actual = sut.EncodeValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void PrimitiveValue_DecodingValue_ReturnsExpectedResult()
    {
        ProcessingOptionsDataObjectList expected = _Fixture.Create<ProcessingOptionsDataObjectList>();
        ProcessingOptionsDataObjectList actual =
            ProcessingOptionsDataObjectList.Decode(EmvFixture.ProcessingOptionsDataObjectListBuilder.GetDefaultEncodedValue().AsSpan());

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion

    //[Fact]
    //public void DataObjectList_AsDataObjectListResult_ReturnsExpectedResult()
    //{
    //    CardRiskManagementDataObjectList1TestTlv testData = new();

    //    CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());

    //    testData.SetupTlvTagsForGivenDb(_Database);

    //    DataObjectListResult expected = testData.SetupValuesForTags(_Database, _Fixture);
    //    DataObjectListResult actual = sut.AsDataObjectListResult(_Database.Object);

    //    Assert.NotNull(actual);
    //    Assert.Equal(expected, actual);
    //}

    //[Fact]
    //public void DataObjectList_AsCommandTemplate_CommandTemplateCreatedSuccesfully()
    //{
    //    CardRiskManagementDataObjectList1TestTlv testData = new();

    //    CardRiskManagementDataObjectList1 sut = CardRiskManagementDataObjectList1.Decode(testData.EncodeValue().AsSpan());

    //    testData.SetupTlvTagsForGivenDb(_Database);

    //    DataObjectListResult objListResult = testData.SetupValuesForTags(_Database, _Fixture);
    //    CommandTemplate expected = objListResult.AsCommandTemplate();
    //    CommandTemplate actual = sut.AsCommandTemplate(_Database.Object);

    //    Assert.Equal(expected, actual);
    //}
}