using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Kernel.Services.Verification;

// TODO: Book 3 Section 10.5.3 Offline PIN Processing
internal class SignatureProcessor : IVerifyCardholderSignature
{
    public CvmCode Process() => throw new System.NotImplementedException();
}