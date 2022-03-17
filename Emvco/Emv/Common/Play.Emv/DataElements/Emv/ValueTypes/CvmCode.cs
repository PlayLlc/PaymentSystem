using Play.Core.Exceptions;
using Play.Core.Extensions;

namespace Play.Emv.DataElements;

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
        if (!CardholderVerificationMethod.Exists(_Value))
            return false;

        if (_Value == CardholderVerificationMethod.Fail)
            return true;

        if (_Value == CardholderVerificationMethod.NoCvmRequired)
            return terminalCapabilities.IsNoCardVerificationMethodRequiredSupported();
        if (_Value == CardholderVerificationMethod.OfflineEncipheredPin)
            return terminalCapabilities.IsEncipheredPinForOfflineVerificationSupported();

        if (_Value == CardholderVerificationMethod.OfflineEncipheredPinAndSignature)
        {
            return terminalCapabilities.IsEncipheredPinForOfflineVerificationSupported()
                && terminalCapabilities.IsSignaturePaperSupported();
        }

        if (_Value == CardholderVerificationMethod.OfflinePlaintextPin)
            return terminalCapabilities.IsPlaintextPinForIccVerificationSupported();
        if (_Value == CardholderVerificationMethod.OfflinePlaintextPinAndSignature)
            return terminalCapabilities.IsPlaintextPinForIccVerificationSupported() && terminalCapabilities.IsSignaturePaperSupported();
        if (_Value == CardholderVerificationMethod.OnlineEncipheredPin)
            return terminalCapabilities.IsEncipheredPinForOnlineVerificationSupported();
        if (_Value == CardholderVerificationMethod.SignaturePaper)
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