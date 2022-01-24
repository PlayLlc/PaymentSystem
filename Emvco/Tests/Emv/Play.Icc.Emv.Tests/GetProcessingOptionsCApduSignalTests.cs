using System;

using Play.Ber.Emv;
using Play.Emv.DataElements;
using Play.Emv.TestData.Ber.Primitive;
using Play.Icc.Emv.GetProcessingOptions;

using Xunit;

namespace Play.Icc.Emv.Tests;

public class GetProcessingOptionsCApduSignalTests
{
    #region Static Metadata

    private static readonly EmvCodec _Codec = new();

    #endregion

    #region Instance Members

    [Fact]
    public void CApduSignal_Initializing_CreatesRApduSignal()
    {
        ProcessingOptionsDataObjectListTestTlv testData = new();
        ProcessingOptionsDataObjectList pdol = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
        GetProcessingOptionsCApduSignal sut =
            GetProcessingOptionsCApduSignal.Create(pdol.AsDataObjectListResult(testData.GetTerminalValues()));

        Assert.NotNull(sut);
    }

    #endregion

    //[Fact]
    //public void CApduSignal_Initializing_CreatesExpectedResult()
    //{
    //    throw new Exception();

    //    //ProcessingOptionsDataObjectListTestTlv testData = new();
    //    //ProcessingOptionsDataObjectList pdol = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
    //    //GetProcessingOptionsCApduSignal sut = GetProcessingOptionsCApduSignal.Create(pdol.AsDataObjectListResult(testData.GetTerminalValues()));

    //    //Assert.NotNull(sut);

    //    //DataObjectListResult testDataDolr = new(testData.GetTerminalValues());
    //}
}