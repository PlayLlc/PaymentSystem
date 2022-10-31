using System;
using System.Linq;

using AutoFixture;

using Play.Emv.Ber.DataElements;
using Play.Emv.Identifiers;
using Play.Emv.Selection.Configuration;
using Play.Emv.Selection.Contracts;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Randoms;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

namespace Play.Emv.Selection.Tests.PreProcessing;

public class PreProcessingIndicatorsTests
{
    #region Instance Values

    private readonly IFixture _Fixture;

    #endregion

    #region Constructor

    public PreProcessingIndicatorsTests()
    {
        _Fixture = new ContactlessFixture().Create();
        _Fixture.RegisterGlobalizationProperties();
    }

    #endregion

    #region Instance Members

    [Fact]
    public void PreProcessingIndicators_InstantiateFromEmpty_IsNotNull()
    {
        //Act
        PreProcessingIndicators sut = new(Array.Empty<TransactionProfile>());

        //Assert
        Assert.NotNull(sut);
    }

    [Fact]
    public void PreProcessingIndicators_InstantiateFromEmpty_CountIsZero()
    {
        //Act
        PreProcessingIndicators sut = new(Array.Empty<TransactionProfile>());

        //Assert
        Assert.Equal(0, sut.Count);
    }

    [Fact]
    public void PreProcessingIndicators_Instantiate_CountIs1()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();

        TransactionProfile transactionProfile = SelectionFactory.CreateTransactionProfile(_Fixture, true, true, true, true);

        //Act
        PreProcessingIndicators sut = new(new[] {transactionProfile});

        //Assert
        Assert.Equal(1, sut?.Count);
    }

    [Fact]
    public void PreProcessingIndicators_Instantiate_ContainsGivenKey()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();
        _Fixture.RegisterCombinationCompositeKey();

        TransactionProfile transactionProfile = SelectionFactory.CreateTransactionProfile(_Fixture, true, true, true, true);
        CombinationCompositeKey key = _Fixture.Create<CombinationCompositeKey>();

        //Act
        PreProcessingIndicators sut = new(new[] {transactionProfile});

        //Assert
        Assert.True(sut.ContainsKey(key));
    }

    [Fact]
    public void PreProcessingIndicators_Instantiate_DoesNotContainGivenKey()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();

        TransactionProfile transactionProfile = SelectionFactory.CreateTransactionProfile(_Fixture, true, true, true, true);
        CombinationCompositeKey key = _Fixture.Create<CombinationCompositeKey>();

        //Act
        PreProcessingIndicators sut = new(new[] {transactionProfile});

        //Assert
        Assert.False(sut.ContainsKey(key));
    }

    [Fact]
    public void PreProcessingIndicators_TryGetValue_ReturnsExpectedResult()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();
        _Fixture.RegisterCombinationCompositeKey();

        TransactionProfile transactionProfile = SelectionFactory.CreateTransactionProfile(_Fixture, true, true, true, true);
        CombinationCompositeKey key = _Fixture.Create<CombinationCompositeKey>();

        PreProcessingIndicators sut = new(new[] {transactionProfile});

        //Act
        bool exists = sut.TryGetValue(key, out PreProcessingIndicator value);

        //Assert
        Assert.True(exists);
        Assert.NotNull(value);
    }

    [Fact]
    public void PreProcessingIndicators_GetKernelIds_ReturnsExpectedResult()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();
        _Fixture.RegisterCombinationCompositeKey();

        TransactionProfile transactionProfile = SelectionFactory.CreateTransactionProfile(_Fixture, true, true, true, true);

        PreProcessingIndicators sut = new(new[] {transactionProfile});

        //Act
        KernelId[] kernelIds = sut.GetKernelIds();

        //Assert
        Assert.Equal(1, kernelIds?.Length);
        Assert.Equal((KernelId) transactionProfile.GetKernelId(), kernelIds[0]);
    }

    [Fact]
    public void PreprocessingIndicators_IsMatchingKernel_ReturnsTrueForGivenKernelIdentifier()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();
        _Fixture.RegisterCombinationCompositeKey();

        TransactionProfile transactionProfile = SelectionFactory.CreateTransactionProfile(_Fixture, true, true, true, true);

        PreProcessingIndicators sut = new(new[] {transactionProfile});
        KernelIdentifier kernelIdentifier = new((byte) transactionProfile.GetKernelId());

        //Act
        bool isMatching = sut.IsMatchingKernel(kernelIdentifier);

        //Assert
        Assert.True(isMatching);
    }

    //IsMatchingAid
    [Fact]
    public void PreProcessingIndicators_IsMatchingAid_ReturnsTrueForGivenApplicationIdentifier()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();
        _Fixture.RegisterCombinationCompositeKey();

        TransactionProfile transactionProfile = SelectionFactory.CreateTransactionProfile(_Fixture, true, true, true, true);

        PreProcessingIndicators sut = new(new[] {transactionProfile});
        DedicatedFileName applicationIdentifier = transactionProfile.GetApplicationIdentifier();

        //Act
        bool isMatching = sut.IsMatchingAid(applicationIdentifier);

        //Assert
        Assert.True(isMatching);
    }

    [Fact]
    public void PreProcessingIndicators_IsMatchingAid_ReturnsFalseForGivenApplicationIdentifier()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();
        _Fixture.RegisterCombinationCompositeKey();

        TransactionProfile transactionProfile = SelectionFactory.CreateTransactionProfile(_Fixture, true, true, true, true);

        PreProcessingIndicators sut = new(new[] {transactionProfile});
        DedicatedFileName applicationIdentifier = new(Randomize.Arrays.Bytes(Randomize.Integers.Int(5, 16)));

        //Act
        bool isMatching = sut.IsMatchingAid(applicationIdentifier);

        //Assert
        Assert.False(isMatching);
    }

    [Fact]
    public void PreProcessingIndicators_Reset_ReturnsExpectedResult()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();

        TransactionProfile transactionProfile = SelectionFactory.CreateTransactionProfile(_Fixture, true, true, true, true);
        TransactionProfile transactionProfile2 = SelectionFactory.CreateTransactionProfile(_Fixture, true, false, true, false);
        TransactionProfile transactionProfile3 = SelectionFactory.CreateTransactionProfile(_Fixture, false, true, false, true);

        PreProcessingIndicators sut = new(new[] {transactionProfile, transactionProfile2, transactionProfile3});

        //Act
        sut.ResetPreprocessingIndicators();

        //Assert
        Assert.True(sut.All(a =>
            (a.Value.ContactlessApplicationNotAllowed == false)
            && (a.Value.ReaderContactlessFloorLimitExceeded == false)
            && (a.Value.ReaderCvmRequiredLimitExceeded == false)
            && (a.Value.StatusCheckRequested == false)
            && (a.Value.ZeroAmount == false)));
    }

    #endregion
}