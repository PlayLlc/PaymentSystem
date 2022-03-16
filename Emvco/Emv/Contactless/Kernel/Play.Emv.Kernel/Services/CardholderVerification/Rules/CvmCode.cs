using Play.Core.Exceptions;
using Play.Core.Extensions;
using Play.Emv.DataElements;

namespace Play.Emv.Kernel.Services.Conditions;

public readonly struct CvmCode
{
    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public CvmCode(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static bool IsRecognized(CvmCode cvmCode, TerminalCapabilities terminalCapabilities)
    {
        if (!CvmCodes.Exists(cvmCode))
            return false;

        if (cvmCode == CvmCodes.Fail)
            return true;

        if (cvmCode == CvmCodes.NoCvmRequired)
            return terminalCapabilities.IsNoCardVerificationMethodRequiredSupported();
        if (cvmCode == CvmCodes.OfflineEncipheredPin)
            return terminalCapabilities.IsEncipheredPinForOfflineVerificationSupported();

        if (cvmCode == CvmCodes.OfflineEncipheredPinAndSignature)
        {
            return terminalCapabilities.IsEncipheredPinForOfflineVerificationSupported()
                && terminalCapabilities.IsSignaturePaperSupported();
        }

        if (cvmCode == CvmCodes.OfflinePlaintextPin)
            return terminalCapabilities.IsPlaintextPinForIccVerificationSupported();
        if (cvmCode == CvmCodes.OfflinePlaintextPinAndSignature)
            return terminalCapabilities.IsPlaintextPinForIccVerificationSupported() && terminalCapabilities.IsSignaturePaperSupported();
        if (cvmCode == CvmCodes.OnlineEncipheredPin)
            return terminalCapabilities.IsEncipheredPinForOnlineVerificationSupported();
        if (cvmCode == CvmCodes.SignaturePaper)
            return terminalCapabilities.IsSignaturePaperSupported();

        throw new PlayInternalException("We should never reach this point");
    }

    public bool IsFailIfUnsuccessfulSet() => !_Value.IsBitSet(Bits.Seven);
    public bool IsTryNextIfUnsuccessfulSet() => _Value.IsBitSet(Bits.Seven);

    #endregion

    #region Operator Overrides

    public static explicit operator byte(CvmCode value) => value._Value;

    #endregion
}