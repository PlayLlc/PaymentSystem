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

    #region Constructors

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
        CardholderVerificationMethodSelectorConfigSetup.RegisterDisabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 123, 140);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(CvmCodes.None, new CvmConditionCode(0), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWith_KernelConfigurationDoesNotSupportOnDeviceCardholderVerificationSupported_NoCvmPerformed()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterDisabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 123, 140);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(CvmCodes.None, new CvmConditionCode(0), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWith_KernelConfigurationWithAIPAndConfigSupportCardholderVerification_AmmountAuthorizedGreaterThenTreshHold_ConfirmationCodeVerifiedOnDeviceCardholderCVMResult()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 140, 123);

        KernelConfiguration kernelConfiguration = new KernelConfiguration(0b0010_0000);
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
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 123, 140);

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

        CvmResults expectedResult = new(new(0), new(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CVRuleIsUnderstoodCVMConditionCodeIsManualButRequiredDataNotPresentInDB_IsLastRule_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
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
        };

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetCardholderVerificationWasNotSuccessful();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        AssertExpectedTvr(tvr);

        CvmResults expectedResult = new(new(0), new(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CVRuleWithCVMCondition_AmountInApplicationCurrencyAndOverXValueCondition_ConditionNotSatisfied_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 345, 470);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
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

        CvmResults expectedResult = new(new(0), new(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

        [Fact]
    public void CVMSelectorWithEnabledConfiguration_CVRuleWithCVMCondition_AmountInApplicationCurrencyAndOverYValueCondition_ConditionNotSatisfied_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 345, 470);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
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

        CvmResults expectedResult = new(new(0), new(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CVRuleWithCVMCondition_AmountInApplicationCurrencyAndUnderXValueCondition_ConditionNotSatisfied_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
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

        CvmResults expectedResult = new(new(0), new(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CVRuleWithCVMCondition_AmountInApplicationCurrencyAndUnderYValueCondition_ConditionNotSatisfied_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
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

        CvmResults expectedResult = new(new(0), new(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CVRuleWithCVMCondition_ManualCash_ConditionNotSatisfied_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
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

        CvmResults expectedResult = new(new(0), new(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    //NotUnattendedCashOrManualCashOrPurchaseWithCashback
    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CVRuleWithCVMCondition_NotUnattendedCashOrManualCashOrPurchaseWithCashback_ConditionNotSatisfied_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Validating CM.10
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
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

        CvmResults expectedResult = new(new(0), new(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CVRuleWithCVMCondition_PurchaseWithCashbackCondition_ConditionNotSatisfied_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
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

        CvmResults expectedResult = new(new(0), new(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCode_CvmCodeIsNotRecognized_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
            //yAmount
            0, 0, 13, 63,
            // CvmRules
            0b0100_1000, //cvm code -> invalid cvm code but with b7 set.
            3 //cvm condition code = SupportsCvmCondition
        };

        TerminalCapabilities terminalCapabilities = new TerminalCapabilities(0b1100_1100_1100);
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

        CvmResults expectedResult = new(new(0), new(0), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCode_CvmCodeIsRecognized_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
            //yAmount
            0, 0, 13, 63,
            // CvmRules
            2, //cvm code -> Online Enciphered pin.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 15 not set -> does not support online enciphered pin.
        TerminalCapabilities terminalCapabilities = new TerminalCapabilities(0b1100_1100_1100);
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

        CvmResults expectedResult = new(new(2), new(3), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCode_CvmCodeIsRecognizedAndTerminalSupportsIt_SuccessfulOnlineEncipheredPinSelection()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
            //yAmount
            0, 0, 13, 63,
            // CvmRules
            2, //cvm code -> Online Enciphered pin.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 15 set -> supports online enciphered pin.
        TerminalCapabilities terminalCapabilities = new TerminalCapabilities(0b0100_1100_1100_1100);
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

        CvmResults expectedResult = new(new(2), new(0), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.OnlinePin, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCode_CvmCodeIsRecognizedAndTerminalSupportsIt_SuccessfulSignatureSelection()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
            //yAmount
            0, 0, 13, 63,
            // CvmRules
            30, //cvm code -> Signature Paper.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 14 set -> supports signature paper.
        TerminalCapabilities terminalCapabilities = new TerminalCapabilities(0b0110_1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(new(30), new(3), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.ObtainSignature, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCode_CvmCodeIsRecognizedAndTerminalSupportsIt_SuccessfullNoCvmSelection()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
            //yAmount
            0, 0, 13, 63,
            // CvmRules
            63, //cvm code -> None.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 12 set -> supports signature paper.
        TerminalCapabilities terminalCapabilities = new TerminalCapabilities(0b1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(new(63), new(3), CvmResultCodes.Successful);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCode_CvmCodeIsRecognizedAndTerminalSupportsIt_SuccessfulProprietarySelection()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
            //yAmount
            0, 0, 13, 63,
            // CvmRules
            1, //cvm code -> OfflinePlaintextPin.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 16 set -> supports offline plain text.
        TerminalCapabilities terminalCapabilities = new TerminalCapabilities(0b1000_1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(new(1), new(3), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    //OfflinePlaintextPinAndSignature
    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCode_CvmCodeIsOfflinePlaintextPinAndSignature_SuccessfulProprietarySelection()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
            //yAmount
            0, 0, 13, 63,
            // CvmRules
            3, //cvm code -> OfflinePlaintextPin.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 16 & 14 are set -> supports PlaintextPinForIccVerificationSupported SignaturePaperSupported;
        TerminalCapabilities terminalCapabilities = new TerminalCapabilities(0b1010_1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(new(3), new(3), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCode_CvmCodeIsOfflineEncipheredPinAndSignature_SuccessfulProprietarySelection()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
            //yAmount
            0, 0, 13, 63,
            // CvmRules
            5, //cvm code -> OfflinePlaintextPin.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 14 & 13 are set -> supports EncipheredPinForOfflineVerificationSupported && SignaturePaperSupported
        TerminalCapabilities terminalCapabilities = new TerminalCapabilities(0b1011_1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(new(5), new(3), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

        [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvRuleWithValidCvmConditionCode_CvmCodeIsFailCode_NoCvmAndCardholderVerificationWasNotSuccessfullSetInTvr()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
            //yAmount
            0, 0, 13, 63,
            // CvmRules
            0, //cvm code -> OfflinePlaintextPin.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 14 & 13 are set -> supports EncipheredPinForOfflineVerificationSupported && SignaturePaperSupported
        TerminalCapabilities terminalCapabilities = new TerminalCapabilities(0b1011_1100_1100_1100);
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

        CvmResults expectedResult = new(new(0), new(3), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvmListWithMultipleCvmRules_CvmConditionSupportSuccedingRuleIfUnsuccessfull_OneRuleIsSelected()
    {
                //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
            //yAmount
            0, 0, 13, 63,
            // CvmRules
            0b0100_1010,
            3,
            4, //cvm code -> OfflineEncipheredPin.
            3 //cvm condition code = SupportsCvmCondition
        };

        //TerminalCap. : Bit 14 & 13 are set -> supports EncipheredPinForOfflineVerificationSupported && SignaturePaperSupported
        TerminalCapabilities terminalCapabilities = new TerminalCapabilities(0b1011_1100_1100_1100);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(new(4), new(3), CvmResultCodes.Unknown);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NoCvm, outcome.GetCvmPerformed());
    }

    [Fact]
    public void CVMSelectorWithEnabledConfiguration_CvmListWithMultipleCvmRules_CvmConditionDoNotSupportSuccedingRuleIfUnsuccessfull_NoCVMIsSelected()
    {
        //Arrange
        CardholderVerificationMethodSelectorConfigSetup.RegisterEnabledConfigurationCVMDefaults(_Fixture, _Database);
        CardholderVerificationMethodSelectorConfigSetup.RegisterTransactionAmountAndCVMTresholdValues(_Database, 4000, 4500);

        ReadOnlySpan<byte> cvmListEncodedContent = stackalloc byte[] {
            //xAmount
            0,0, 12, 34,
            //yAmount
            0, 0, 13, 63,
            // CvmRules
            0b0000_1010,
            3,
            4, //cvm code -> OfflineEncipheredPin.
            3 //cvm condition code = SupportsCvmCondition
        };

        TerminalCapabilities terminalCapabilities = new TerminalCapabilities(0b1111_1111_1111_1111);
        _Database.Update(terminalCapabilities);

        CvmList cvmList = CvmList.Decode(cvmListEncodedContent);
        _Database.Update(cvmList);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetUnrecognizedCvm();
        tvr.SetPinEntryRequiredAndPinPadNotPresentOrNotWorking();

        //Act
        _SystemUnderTest.Process(_Database);

        //Assert
        CvmResults expectedResult = new(new(0b0000_1010), new(3), CvmResultCodes.Failed);
        Assert.Equal(expectedResult, _Database.Get(CvmResults.Tag));

        OutcomeParameterSet outcome = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(CvmPerformedOutcome.NotAvailable, outcome.GetCvmPerformed());
    }

    private void AssertExpectedTvr(TerminalVerificationResult tvr)
    {
        TerminalVerificationResults expectedTvr = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);
        Assert.Equal(tvr, (TerminalVerificationResult)expectedTvr);
    }

    #endregion
}
