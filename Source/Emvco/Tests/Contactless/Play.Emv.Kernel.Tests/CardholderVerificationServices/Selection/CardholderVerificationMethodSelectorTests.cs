using System;

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

    #region Constructor

    public CardholderVerificationMethodSelectorTests()
    {
        _Fixture = new ContactlessFixture().Create();
        _Fixture.RegisterGlobalizationProperties();
        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);
        _SystemUnderTest = new CardholderVerificationMethodSelector();
    }

    #endregion

    #region Tests

    [Fact]
    public void CVMSelectorWithTerminalNotSupportingOfflineCvm_Process_NoCvmPerformed()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterDisabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 123, 140);
        CvmResults expectedResult = new(CvmCodes.None, new CvmConditionCode(0), CvmResultCodes.Unknown);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWith_KernelConfigurationDoesNotSupportOnDeviceCardholderVerificationSupported_NoCvmPerformed()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterDisabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 123, 140);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(CvmCodes.None, new CvmConditionCode(0), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    [Fact]
    public void
        CVMSelectorWithKernelConfigurationWithAIPAndConfigSupportCardholderVerification_AmountAuthorizedGreaterThenThreshHold_ConfirmationCodeVerifiedOnDeviceCardholderCvmResult()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 140, 123);

        KernelConfiguration kernelConfiguration = new(0b0010_0000);
        _Database.Update(kernelConfiguration);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetPinEntryRequiredAndPinPadNotPresentOrNotWorking();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(CvmCodes.OfflinePlaintextPin, new CvmConditionCode(0), CvmResultCodes.Successful);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.ConfirmationCodeVerified, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CVMListTagNotPresent_NoCvmPerformedMissingICCDataInTvr()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 123, 140);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetIccDataMissing();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(CvmCodes.None, new CvmConditionCode(0), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    [Fact]
    public void
        CVMSelectorWithEnabledConfigurationAndCvmRulesPresent_ListHas1CvmRuleCvmConditionNotUnderstood_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 123, 140);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            12, 13, 14, 15,

            //yAmount
            16, 17, 18, 19,

            // CvmRules
            17, 16
        };

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetCardholderVerificationWasNotSuccessful();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(new CvmCode(0), new CvmConditionCode(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void
        CVMSelectorWithEnabledConfiguration_CVRuleIsUnderstoodCVMConditionCodeIsManualButRequiredDataNotPresentInDBIsLastRule_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 123, 140);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            12, 13, 14, 15,

            //yAmount
            16, 17, 18, 19,

            // CvmRules
            17, 4 //cvm condition code = manual cash condition
        };

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetCardholderVerificationWasNotSuccessful();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(new CvmCode(0), new CvmConditionCode(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void
        CVMSelectorWithEnabledConfiguration_CVRuleWithCVMConditionAmountInApplicationCurrencyAndOverXValueConditionConditionNotSatisfied_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 345, 470);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            17, 7 //cvm condition code = AmountInApplicationCurrencyAndOverXValueCondition
        };

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetCardholderVerificationWasNotSuccessful();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(new CvmCode(0), new CvmConditionCode(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void
        CVMSelectorWithEnabledConfiguration_CVRuleWithCVMConditionAmountInApplicationCurrencyAndOverYValueConditionConditionNotSatisfied_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 345, 470);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            17, 9 //cvm condition code = AmountInApplicationCurrencyAndOverYValueCondition
        };

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetCardholderVerificationWasNotSuccessful();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(new CvmCode(0), new CvmConditionCode(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void
        CVMSelectorWithEnabledConfiguration_CVRuleWithCVMConditionAmountInApplicationCurrencyAndUnderXValueConditionConditionNotSatisfied_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            17, 6 //cvm condition code = AmountInApplicationCurrencyAndUnderXValueCondition
        };

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetCardholderVerificationWasNotSuccessful();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(new CvmCode(0), new CvmConditionCode(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void
        CVMSelectorWithEnabledConfiguration_CVRuleWithCVMConditionAmountInApplicationCurrencyAndUnderYValueConditionConditionNotSatisfied_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            17, 8 //cvm condition code = AmountInApplicationCurrencyAndUnderXValueCondition
        };

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetCardholderVerificationWasNotSuccessful();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(new CvmCode(0), new CvmConditionCode(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void
        CVMSelectorWithEnabledConfiguration_CVRuleWithCVMConditionManualCashConditionNotSatisfied_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            17, 4 //cvm condition code = AmountInApplicationCurrencyAndUnderXValueCondition
        };

        _Database.Update(TransactionTypes.CardVerification);
        _Database.Update(new PosEntryMode(3));

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetCardholderVerificationWasNotSuccessful();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(new CvmCode(0), new CvmConditionCode(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    //NotUnattendedCashOrManualCashOrPurchaseWithCashback
    [Fact]
    public void
        CVMSelectorWithEnabledConfiguration_CVRuleWithCVMConditionNotUnattendedCashOrManualCashOrPurchaseWithCashbackConditionNotSatisfied_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            17, 2 //cvm condition code = NotUnattendedCashOrManualCashOrPurchaseWithCashback
        };

        _Database.Update(TransactionTypes.CashAdvance);
        _Database.Update(new TerminalType(4));

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetCardholderVerificationWasNotSuccessful();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(new CvmCode(0), new CvmConditionCode(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void
        CVMSelectorWithEnabledConfiguration_CVRuleWithCVMConditionPurchaseWithCashbackConditionConditionNotSatisfied_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            17, 3 //cvm condition code = NotUnattendedCashOrManualCashOrPurchaseWithCashback
        };

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetCardholderVerificationWasNotSuccessful();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(new CvmCode(0), new CvmConditionCode(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void
        CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCodeCvmCodeIsNotRecognized_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            0b0100_1000, //cvm code -> invalid cvm code but with b7 set.
            3 //cvm condition code = SupportsCvmCondition
        };

        TerminalCapabilities terminalCapabilities = new(0b1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetUnrecognizedCvm();
        tvr.SetCardholderVerificationWasNotSuccessful();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(new CvmCode(0), new CvmConditionCode(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void
        CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCodeCvmCodeIsRecognized_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            2, //cvm code -> Online Enciphered pin.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 15 not set -> does not support online enciphered pin.
        TerminalCapabilities terminalCapabilities = new(0b1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        //Recognized cvm but not supported by the terminal capabilities.
        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetCardholderVerificationWasNotSuccessful();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(new CvmCode(2), new CvmConditionCode(3), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void
        CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCodeCvmCodeIsRecognizedAndTerminalSupportsIt_SuccessfulOnlineEncipheredPinSelection()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            2, //cvm code -> Online Enciphered pin.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 15 set -> supports online enciphered pin.
        TerminalCapabilities terminalCapabilities = new(0b0100_1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        //Recognized cvm but not supported by the terminal capabilities.
        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetOnlinePinEntered();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(new CvmCode(2), new CvmConditionCode(0), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.OnlinePin, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCodeCvmCodeIsRecognizedAndTerminalSupportsIt_SuccessfulSignatureSelection()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            30, //cvm code -> Signature Paper.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 14 set -> supports signature paper.
        TerminalCapabilities terminalCapabilities = new(0b0110_1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(new CvmCode(30), new CvmConditionCode(3), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.ObtainSignature, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCodeCvmCodeIsRecognizedAndTerminalSupportsIt_SuccessfullNoCvmSelection()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            63, //cvm code -> None.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 12 set -> supports signature paper.
        TerminalCapabilities terminalCapabilities = new(0b1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(new CvmCode(63), new CvmConditionCode(3), CvmResultCodes.Successful);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCodeCvmCodeIsRecognizedAndTerminalSupportsIt_SuccessfulProprietarySelection()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            1, //cvm code -> OfflinePlaintextPin.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 16 set -> supports offline plain text.
        TerminalCapabilities terminalCapabilities = new(0b1000_1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(new CvmCode(1), new CvmConditionCode(3), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    //OfflinePlaintextPinAndSignature
    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCodeCvmCodeIsOfflinePlaintextPinAndSignature_SuccessfulProprietarySelection()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            3, //cvm code -> OfflinePlaintextPin.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 16 & 14 are set -> supports PlaintextPinForIccVerificationSupported SignaturePaperSupported;
        TerminalCapabilities terminalCapabilities = new(0b1010_1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(new CvmCode(3), new CvmConditionCode(3), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCodeCvmCodeIsOfflineEncipheredPinAndSignature_SuccessfulProprietarySelection()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            5, //cvm code -> OfflinePlaintextPin.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 14 & 13 are set -> supports EncipheredPinForOfflineVerificationSupported && SignaturePaperSupported
        TerminalCapabilities terminalCapabilities = new(0b1011_1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(new CvmCode(5), new CvmConditionCode(3), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCodeCvmCodeIsFailCode_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            0, //cvm code -> OfflinePlaintextPin.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 14 & 13 are set -> supports EncipheredPinForOfflineVerificationSupported && SignaturePaperSupported
        TerminalCapabilities terminalCapabilities = new(0b1011_1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetCardholderVerificationWasNotSuccessful();
        tvr.SetPinEntryRequiredAndPinPadNotPresentOrNotWorking();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(new CvmCode(0), new CvmConditionCode(3), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvmListWithMultipleCvmRulesCvmConditionSupportSuccedingRuleIfUnsuccessfull_OneRuleIsSelected()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            0b0100_1010, 3, 4, //cvm code -> OfflineEncipheredPin.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 14 & 13 are set -> supports EncipheredPinForOfflineVerificationSupported && SignaturePaperSupported
        TerminalCapabilities terminalCapabilities = new(0b1011_1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(new CvmCode(4), new CvmConditionCode(3), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvmListWithMultipleCvmRulesCvmConditionDoNotSupportSuccedingRuleIfUnsuccessfull_NoCVMIsSelected()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCvmDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCvmThresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[]
        {
            //xAmount
            0, 0, 12, 34,

            //yAmount
            0, 0, 13, 63,

            // CvmRules
            0b0000_1010, 3, 4, //cvm code -> OfflineEncipheredPin.
            3 //cvm condition code = SupportsCvmCondition
        };

        TerminalCapabilities terminalCapabilities = new(0b1111_1111_1111_1111);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetUnrecognizedCvm();
        tvr.SetPinEntryRequiredAndPinPadNotPresentOrNotWorking();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(new CvmCode(0b0000_1010), new CvmConditionCode(3), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    private void AssertExpectedTvr(TerminalVerificationResult tvr)
    {
        TerminalVerificationResults expectedTvr = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);
        Assert.Equal(tvr, (TerminalVerificationResult) expectedTvr);
    }

    #endregion
}