﻿using AutoFixture;

using Moq;

using Play.Emv.Ber;
using Play.Emv.Kernel.Services.Verification;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

namespace Play.Emv.Kernel.Tests.CardholderVerificationServices.Verification;

public class CardholderVerificationMethodServiceTests : TestBase
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly ITlvReaderAndWriter _Database;
    private readonly Mock<IVerifyCardholderPinOffline> _OfflinePinProcessor;
    private readonly Mock<IVerifyCardholderPinOnline> _OnlinePinProcessor;
    private readonly Mock<IVerifyCardholderSignature> _CardholderSignatureVerification;
    private readonly IVerifyCardholder _SystemUnderTest;

    #endregion

    #region Constructor

    public CardholderVerificationMethodServiceTests()
    {
        _Fixture = new ContactlessFixture().Create();
        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);

        _OfflinePinProcessor = new Mock<IVerifyCardholderPinOffline>();
        _OnlinePinProcessor = new Mock<IVerifyCardholderPinOnline>();
        _CardholderSignatureVerification = new Mock<IVerifyCardholderSignature>();

        _SystemUnderTest =
            new CardholderVerificationMethodService(_OfflinePinProcessor.Object, _OnlinePinProcessor.Object, _CardholderSignatureVerification.Object);
    }

    #endregion

    #region Instance Members

    [Fact]
    public void CvmList_NoCvRulesPresent_IccDataMissingIsSetInTvr()
    { }

    [Fact]
    public void CvmListWithCVMForOfflineProcessing_TerminalDoesNotSupportOfflinePinProcessing_PinEntryRequiredAndPinPadNotPresentAreSetInTheTvr()
    { }

    [Fact]
    public void CvmListWithCVMForOfflineProcessing_PinPadMalfunctioning_PinEntryRequiredAndPinPadNotPresentAreSetInTheTvr()
    { }

    [Fact]
    public void CvmListWithCVMForOfflineProcessing_PinIsBypassedButPadIsPresent_PinRequiredAndPinPadPresentSetInTheTvr()
    { }

    [Fact]
    public void CvmListWithCVMForOfflineProcessing_PinIsBlockedUponInitialVerify_PinTryLimitExceededSetInTheTvr()
    { }

    [Fact]
    public void CvmListWithCVMForOfflineProcessing_PinTriesReducedToZero_PinTryLimitExceededSetInTheTvr()
    { }

    [Fact]
    public void CvmListWithCVMForOfflineProcessing_OfflinePinProcessingConsideredSuccesfull_OfflinePinEnteredSetInTheTvr()
    { }

    [Fact]
    public void CvmListWithCVMForOnlinePinProcessing_TerminalDoesNotSupportOnlinePIN_PinEntryRequiredAndPinPadNotPresentIsSetInTheTVR()
    { }

    [Fact]
    public void CvmListWithCVMForOnlinePinProcessing_TerminalSupportOnlinePINButPinPadIsMalfunctioning_PinEntryRequiredAndPinPadNotPresentIsSetInTheTVR()
    { }

    [Fact]
    public void CvmListWithCVMForOnlinePinProcessing_TerminalBypassedOnlinePIN_PinEntryRequiredAndPinPadPresentButPinNotEnteredIsSetInTheTVR()
    { }

    [Fact]
    public void CvmListWithCVMForOnlinePinProcessing_PinSuccessfullyEntered_OnlinePinEnteredIsSetInTheTvr()
    { }

    [Fact]
    public void CvmListWithCVMForSignatureProcessing_TerminalSupportsSignatureProcessing_CardHolderVerificationIsConsideredSuccesfull()
    { }

    #endregion
}