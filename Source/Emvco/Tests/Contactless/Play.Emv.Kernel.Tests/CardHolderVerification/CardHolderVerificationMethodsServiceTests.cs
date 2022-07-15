using Play.Testing.BaseTestClasses;

namespace Play.Emv.Kernel.Tests.CardHolderVerification;

public class CardHolderVerificationMethodsServiceTests : TestBase
{
    private readonly IVerifyCardholderPinOffline _OfflinePinAuthentication;
    private readonly IVerifyCardholderPinOnline _OnlinePinAuthentication;
    //not implemented
    private readonly IVerifyCardholderSignature _CardholderSignatureVerification;
}
