using AutoFixture;

using Moq;

using Play.Emv.Ber;
using Play.Emv.Kernel.Services.Verification;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

namespace Play.Emv.Kernel.Tests.CardholderVerificationMethodService;

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

    #region Constructors

    public CardholderVerificationMethodServiceTests()
    {
        _Fixture = new ContactlessFixture().Create();
        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);

        _OfflinePinProcessor = new Mock<IVerifyCardholderPinOffline>();
        _OnlinePinProcessor = new Mock<IVerifyCardholderPinOnline>();
        _CardholderSignatureVerification = new Mock<IVerifyCardholderSignature>();

        _SystemUnderTest = new CardholderVerificationService(_OfflinePinProcessor.Object, _OnlinePinProcessor.Object, _CardholderSignatureVerification.Object);
    }

    #endregion

    #region Instance Members

    [Fact]
    public void CvmList_NoCvRulesPresent_IccDataMissingIsSetInTvr()
    {

    }

    [Fact]
    public void CvmListWithCVMForOfflineProcessing_TerminalDoesNotSupportOfflinePinProcessing_PinEntryRequiredAndPinPadNotPresentAreSetInTheTvr()
    {

    }

    [Fact]
    public void CvmListWithCVMForOfflineProcessing_PinPadMalfunctioning_PinEntryRequiredAndPinPadNotPresentAreSetInTheTvr()
    {

    }

    [Fact]
    public void CvmListWithCVMForOfflineProcessing_PinIsBypassedButPadIsPresent_PinRequiredAndPinPadPresentSetInTheTvr()
    {

    }

    //(the ICC returns SW1 SW2 = '6983' or '6984' in response to the VERIFY command)
    [Fact]
    public void CvmListWithCVMForOfflineProcessing_PinIsBlockedUponInitialVerify_PinTryLimitExceededSetInTheTvr()
    {

    }

    // (indicated by an SW1 SW2 of '63C0' in the response to the VERIFY command)
    [Fact]
    public void CvmListWithCVMForOfflineProcessing_PinTriesReducedToZero_PinTryLimitExceededSetInTheTvr()
    {

    }

    //PIN processing is considered successful is when the ICC returns an SW1 SW2 of '9000' in response to the VERIFY command
        [Fact]
    public void CvmListWithCVMForOfflineProcessing_OfflinePinProcessingConsideredSuccesfull_OfflinePinEnteredSetInTheTvr()
    {

    }

    [Fact]
    public void CvmListWithCVMForOnlinePinProcessing_TerminalDoesNotSupportOnlinePIN_PinEntryRequiredAndPinPadNotPresentIsSetInTheTVR()
    {

    }

    [Fact]
    public void CvmListWithCVMForOnlinePinProcessing_TerminalSupportOnlinePINButPinPadIsMalfunctioning_PinEntryRequiredAndPinPadNotPresentIsSetInTheTVR()
    {

    }

    [Fact]
    public void CvmListWithCVMForOnlinePinProcessing_TerminalBypassedOnlinePIN_PinEntryRequiredAndPinPadPresentButPinNotEnteredIsSetInTheTVR()
    {

    }

    [Fact]
    public void CvmListWithCVMForOnlinePinProcessing_PinSuccessfullyEntered_OnlinePinEnteredIsSetInTheTvr()
    {

    }

    [Fact]
    public void CvmListWithCVMForSignatureProcessing_TerminalSupportsSignatureProcessing_CardHolderVerificationIsConsideredSuccesfull()
    {

    }

    #endregion
}
