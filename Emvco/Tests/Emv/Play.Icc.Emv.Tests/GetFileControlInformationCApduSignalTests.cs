using System;

using Play.Emv.Ber;
using Play.Emv.Icc.FileControlInformation;
using Play.Emv.TestData.Icc.Apdu;
using Play.Icc.FileSystem.DedicatedFiles;

using Xunit;

namespace Play.Icc.Emv.Tests;

public class GetFileControlInformationCApduSignalTests
{
    #region Static Metadata

    private static readonly EmvCodec _Codec = new();

    #endregion

    #region Instance Members

    [Fact]
    public void CApduSignal_Initializing_CreatesRApduSignal()
    {
        GetFileControlInformationCApduSignal sut = GetFileControlInformationCApduSignal.GetProximityPaymentSystemEnvironment();

        Assert.NotNull(sut);
    }

    [Fact]
    public void CApduSignal_Initializing_CreatesExpectedResult()
    {
        GetFileControlInformationCApduSignal sut = GetFileControlInformationCApduSignal.GetProximityPaymentSystemEnvironment();
        byte[] expectedResult = ApduTestData.CApdu.Select.Ppse.PpseBytes;
        byte[] testValue = sut.Serialize();

        Assert.Equal(expectedResult, testValue);
    }

    [Fact]
    public void CApduSignal_InitializingWithDedicatedFileName_CreatesExpectedResult()
    {
        GetFileControlInformationCApduSignal sut =
            GetFileControlInformationCApduSignal.Get(DedicatedFileName.Decode(ApduTestData.CApdu.Select.Applet1.DedicatedFileName.AsSpan(),
                                                                              _Codec));

        byte[] expectedResult = ApduTestData.CApdu.Select.Applet1.CApdu;
        byte[] testValue = sut.Serialize();

        Assert.Equal(expectedResult, testValue);
    }

    #endregion
}