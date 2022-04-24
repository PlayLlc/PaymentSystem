using System;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Icc.Tests;

public class GetProcessingOptionsCApduSignalTests
{
    #region Static Metadata

    private static readonly EmvCodec _Codec = new();

    #endregion

    #region Instance Members

    /// <summary>
    ///     CApduSignal_Initializing_CreatesRApduSignal
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void CApduSignal_Initializing_CreatesRApduSignal()
    {
        ProcessingOptionsDataObjectListTestTlv testData = new();
        ProcessingOptionsDataObjectList pdol = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
        GetProcessingOptionsCApduSignal sut = GetProcessingOptionsCApduSignal.Create(new DataObjectListResult(pdol.AsTagLengthValue(_Codec)));

        Assert.NotNull(sut);
    }

    [Fact]
    public void CApduSignal_Initializing_CreatesExpectedResult()
    {
        ProcessingOptionsDataObjectListTestTlv testData = new();
        ProcessingOptionsDataObjectList pdol = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
        GetProcessingOptionsCApduSignal sut = GetProcessingOptionsCApduSignal.Create(new DataObjectListResult(pdol.AsTagLengthValue(_Codec)));

        Assert.NotNull(sut);
    }

    #endregion
}