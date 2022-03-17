using Play.Core.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Exceptions;

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

    public bool IsRecognized() => CardholderVerificationMethodCodes.Exists(_Value);

    /// <remarks>EMV Book C-2 Section CVM.17</remarks>
    public bool IsFailureControlSupported() => _Value.GetMaskedValue(0b00111111) != 0;

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <remarks>EMV Book C-2 Section CVM.17</remarks>
    public bool IsSupported(TerminalCapabilities terminalCapabilities)
    {
        if (IsFailureControlSupported())
            return false;

        if (!CardholderVerificationMethodCodes.Exists(_Value))
        {
            throw new
                TerminalDataException($"An unrecognized {nameof(CvmCode)} cannot be processed by the terminal. Please check if the {nameof(CvmCode)} is recognized by the terminal before verifying support");
        }

        if (_Value == CardholderVerificationMethodCodes.Fail)
            return false;

        if (_Value == CardholderVerificationMethodCodes.None)
            return terminalCapabilities.IsNoCardVerificationMethodRequiredSupported();
        if (_Value == CardholderVerificationMethodCodes.OfflineEncipheredPin)
            return terminalCapabilities.IsEncipheredPinForOfflineVerificationSupported();

        if (_Value == CardholderVerificationMethodCodes.OfflineEncipheredPinAndSignature)
        {
            return terminalCapabilities.IsEncipheredPinForOfflineVerificationSupported()
                && terminalCapabilities.IsSignaturePaperSupported();
        }

        if (_Value == CardholderVerificationMethodCodes.OfflinePlaintextPin)
            return terminalCapabilities.IsPlaintextPinForIccVerificationSupported();
        if (_Value == CardholderVerificationMethodCodes.OfflinePlaintextPinAndSignature)
            return terminalCapabilities.IsPlaintextPinForIccVerificationSupported() && terminalCapabilities.IsSignaturePaperSupported();
        if (_Value == CardholderVerificationMethodCodes.OnlineEncipheredPin)
            return terminalCapabilities.IsEncipheredPinForOnlineVerificationSupported();
        if (_Value == CardholderVerificationMethodCodes.SignaturePaper)
            return terminalCapabilities.IsSignaturePaperSupported();

        return false;
    }

    public bool IsFailIfUnsuccessfulSet() => !_Value.IsBitSet(Bits.Eight);
    public bool IsTryNextIfUnsuccessfulSet() => _Value.IsBitSet(Bits.Seven);

    #endregion

    #region Operator Overrides

    public static explicit operator byte(CvmCode value) => value._Value;

    #endregion
}