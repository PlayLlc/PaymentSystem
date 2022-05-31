using System;

using AutoFixture;

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

    #endregion

    #region Constructor

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public ProcessingOptionsDataObjectListTests()
    {
        _Fixture = new EmvFixture().Create();
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

    ///// <summary>
    /////     TagLengthValue_CreatingDataObjectListResult_IsNotNull
    ///// </summary>
    ///// <exception cref="InvalidOperationException"></exception>
    ///// <exception cref="BerParsingException"></exception>
    //[Fact]
    //public void TagLengthValue_CreatingDataObjectListResult_IsNotNull()
    //{
    //    ProcessingOptionsDataObjectListTestTlv testData = new();

    //    ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());

    //    Mock<IReadTlvDatabase> databaseMock = new();
    //    databaseMock.Setup(a => a.IsPresentAndNotEmpty(It.IsAny<Tag>())).Returns(true);
    //    databaseMock.Setup(a => a.Get(ApplicationExpirationDate.Tag)).Returns(new ApplicationExpirationDateTestTlv().AsPrimitiveValue());

    //    DataObjectListResult testValue = sut.AsDataObjectListResult(databaseMock.Object);

    //    Assert.NotNull(testValue);
    //}

    ///// <summary>
    /////     TagLengthValue_CreatingDataObjectListResult_ReturnsExpectedResult
    ///// </summary>
    ///// <exception cref="InvalidOperationException"></exception>
    ///// <exception cref="BerParsingException"></exception>
    //[Fact]
    //public void TagLengthValue_CreatingDataObjectListResult_ReturnsExpectedResult()
    //{
    //    ProcessingOptionsDataObjectListTestTlv testData = new();

    //    ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
    //    Mock<IReadTlvDatabase> databaseMock = new();
    //    databaseMock.Setup(a => a.IsPresentAndNotEmpty(It.IsAny<Tag>())).Returns(true);
    //    databaseMock.Setup(a => a.Get(ApplicationExpirationDate.Tag)).Returns(new ApplicationExpirationDateTestTlv().AsPrimitiveValue());

    //    sut.AsDataObjectListResult(databaseMock.Object);

    //    DataObjectListResult expectedResult = new(testData.AsTagLengthValues());
    //    DataObjectListResult testValue = sut.AsDataObjectListResult(databaseMock.Object);

    //    Assert.Equal(expectedResult, testValue);
    //}

    ///// <summary>
    /////     TagLengthValue_CreatingCommandTemplate_IsNotNull
    ///// </summary>
    ///// <exception cref="InvalidOperationException"></exception>
    ///// <exception cref="BerParsingException"></exception>
    //[Fact]
    //public void TagLengthValue_CreatingCommandTemplate_IsNotNull()
    //{
    //    ProcessingOptionsDataObjectListTestTlv testData = new();

    //    ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
    //    Mock<IReadTlvDatabase> databaseMock = new();
    //    databaseMock.Setup(a => a.IsPresentAndNotEmpty(It.IsAny<Tag>())).Returns(true);
    //    databaseMock.Setup(a => a.Get(ApplicationExpirationDate.Tag)).Returns(new ApplicationExpirationDateTestTlv().AsPrimitiveValue());

    //    sut.AsDataObjectListResult(databaseMock.Object);
    //    CommandTemplate? testValue = sut.AsCommandTemplate(databaseMock.Object);

    //    Assert.NotNull(testValue);
    //}

    ///// <summary>
    /////     TagLengthValue_CreatingCommandTemplate_ReturnsExpectedResult
    ///// </summary>
    ///// <exception cref="InvalidOperationException"></exception>
    ///// <exception cref="BerParsingException"></exception>
    //[Fact]
    //public void TagLengthValue_CreatingCommandTemplate_ReturnsExpectedResult()
    //{
    //    ProcessingOptionsDataObjectListTestTlv testData = new();

    //    ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());

    //    CommandTemplate expectedResult = CommandTemplate.Decode(testData.GetTerminalValues()
    //        .SelectMany(a => a.EncodeTagLengthValue(EmvCodec.GetBerCodec())).ToArray().AsSpan());

    //    Mock<IReadTlvDatabase> databaseMock = new();
    //    databaseMock.Setup(a => a.IsPresentAndNotEmpty(It.IsAny<Tag>())).Returns(true);
    //    databaseMock.Setup(a => a.Get(ApplicationExpirationDate.Tag)).Returns(new ApplicationExpirationDateTestTlv().AsPrimitiveValue());

    //    CommandTemplate testValue = sut.AsCommandTemplate(databaseMock.Object);

    //    Assert.Equal(expectedResult, testValue);
    //}

    // TODO: Need to test the creation of a DataObjectListResult
    // ProcessingOptionsDataObjectList.AsDataObjectListResult(TagLengthValue[] terminalValues);
}