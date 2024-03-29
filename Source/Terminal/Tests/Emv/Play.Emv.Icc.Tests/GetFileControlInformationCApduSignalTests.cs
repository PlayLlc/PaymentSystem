﻿using System;

using Play.Ber.Exceptions;
using Play.Emv.Ber;
using Play.Icc.Exceptions;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Testing.Icc.Apdu;

using Xunit;

namespace Play.Emv.Icc.Tests;

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
        byte[] testValue = sut.Encode();

        Assert.Equal(expectedResult, testValue);
    }

    /// <summary>
    ///     CApduSignal_InitializingWithDedicatedFileName_CreatesExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    [Fact]
    public void CApduSignal_InitializingWithDedicatedFileName_CreatesExpectedResult()
    {
        GetFileControlInformationCApduSignal sut =
            GetFileControlInformationCApduSignal.Get(DedicatedFileName.Decode(ApduTestData.CApdu.Select.Applet1.DedicatedFileName.AsSpan(), _Codec));

        byte[] expectedResult = ApduTestData.CApdu.Select.Applet1.CApdu;
        byte[] testValue = sut.Encode();

        Assert.Equal(expectedResult, testValue);
    }

    #endregion
}