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

    public bool IsRecognized(TerminalCapabilities terminalCapabilities)
    {
        if (!CvmCodes.Exists(_Value))
            return false;

        if (_Value == CvmCodes.Fail)
            return true;

        if (_Value == CvmCodes.NoCvmRequired)
            return terminalCapabilities.IsNoCardVerificationMethodRequiredSupported();
        if (_Value == CvmCodes.OfflineEncipheredPin)
            return terminalCapabilities.IsEncipheredPinForOfflineVerificationSupported();

        if (_Value == CvmCodes.OfflineEncipheredPinAndSignature)
        {
            return terminalCapabilities.IsEncipheredPinForOfflineVerificationSupported()
                && terminalCapabilities.IsSignaturePaperSupported();
        }

        if (_Value == CvmCodes.OfflinePlaintextPin)
            return terminalCapabilities.IsPlaintextPinForIccVerificationSupported();
        if (_Value == CvmCodes.OfflinePlaintextPinAndSignature)
            return terminalCapabilities.IsPlaintextPinForIccVerificationSupported() && terminalCapabilities.IsSignaturePaperSupported();
        if (_Value == CvmCodes.OnlineEncipheredPin)
            return terminalCapabilities.IsEncipheredPinForOnlineVerificationSupported();
        if (_Value == CvmCodes.SignaturePaper)
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