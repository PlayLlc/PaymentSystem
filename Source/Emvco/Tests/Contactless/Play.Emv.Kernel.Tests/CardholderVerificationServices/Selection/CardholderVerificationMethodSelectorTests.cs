﻿using System;

using AutoFixture;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Services.Selection;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

namespace Play.Emv.Kernel.Tests.CardholderVerificationServices.Selection;

public class CardholderVerificationMethodSelectorTests : TestBase
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly KernelDatabase _Database;

    private readonly ISelectCardholderVerificationMethod _SystemUnderTest;

    #endregion

    #region Constructors

    public CardholderVerificationMethodSelectorTests()
    {
        _Fixture = new ContactlessFixture().Create();

        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);
        _Fixture.RegisterGlobalizationCodes();

        _SystemUnderTest = new CardholderVerificationMethodSelector();
    }

    #endregion

    #region Tests

    [Fact]
    public void CVMSelectorWithTerminalNotSupportingOfflineCvm_Process_NoCvmPerformed()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterDisabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 123, 140);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetPinEntryRequiredAndPinPadNotPresentOrNotWorking();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        ExpectedTvr(tvr);
        ExpectNoCvmResult();
    }

    [Fact]
    public void CVMSelectorWith_KernelConfigurationDoesNotSupportOnDeviceCardholderVerificationSupported_NoCvmPerformed()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterDisabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 123, 140);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetPinEntryRequiredAndPinPadNotPresentOrNotWorking();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        ExpectedTvr(tvr);
        ExpectNoCvmResult();
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CVMListTagNotPresent_NoCvmPerformedMissingICCDataInTvr()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 123, 140);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetIccDataMissing();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        ExpectedTvr(tvr);

        CvmResults expectedResult = new(CvmCodes.None, new CvmConditionCode(0), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CVMListTagIsPresentButListIsEmpty_NoCvmPerformedMissingICCDataInTvr()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 123, 140);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] { 12, 13, 14, 15, 16, 17, 18, 19 };
        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetIccDataMissing();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        ExpectedTvr(tvr);

        CvmResults expectedResult = new(CvmCodes.None, new CvmConditionCode(0), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfigurationAndCvmRulesPresent_ListHas1CvmRuleCvmConditionNotUnderstood_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 123, 140);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            12, 13, 14, 15,
            //yAmount
            16, 17, 18, 19,
            // CvmRules
            17, 16 //Problem where when there are ending zeroes(conversion to big integer ditches those zeroes).
        };

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetCardholderVerificationWasNotSuccessful();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        ExpectedTvr(tvr);

        CvmResults expectedResult = new(CvmCodes.None, new CvmConditionCode(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CVRuleIsUnderstoodCVMConditionCodeIsAlwaysButRequiredDataNotPresentInDB_IsLastRule_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 123, 140);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            12, 13, 14, 15,
            //yAmount
            16, 17, 18, 19,
            // CvmRules
            17, 4 //cvm condition code = manual cash condition
            //Problem where when there are ending zeroes(conversion to big integer ditches those zeroes).
        };

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetCardholderVerificationWasNotSuccessful();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        ExpectedTvr(tvr);

        CvmResults expectedResult = new(CvmCodes.None, new CvmConditionCode(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

        [Fact]
    public void CVMSelectorWithEnabledConfiguration_CVRuleIsUnderstoodCVMConditionCodeIsAlwaysButRequiredDataNotPresentInDB_IsLastRule_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 123, 140);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            12, 13, 14, 15,
            //yAmount
            16, 17, 18, 19,
            // CvmRules
            17, 4 //cvm condition code = manual cash condition
            //Problem where when there are ending zeroes(conversion to big integer ditches those zeroes).
        };

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetCardholderVerificationWasNotSuccessful();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        ExpectedTvr(tvr);

        CvmResults expectedResult = new(CvmCodes.None, new CvmConditionCode(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    private void ExpectNoCvmResult()
    {
        CvmResults expectedResult = new(CvmCodes.None, new CvmConditionCode(0), CvmResultCodes.Successful);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    private void ExpectedTvr(TerminalVerificationResult tvr)
    {
        TerminalVerificationResults expectedTvr = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);
        Assert.Equal(tvr, (TerminalVerificationResult)expectedTvr);
    }

    #endregion
}
