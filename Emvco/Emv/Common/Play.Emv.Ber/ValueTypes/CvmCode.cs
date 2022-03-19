using Play.Core.Extensions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber;

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

    public bool IsRecognized() => CvmCodes.Exists(_Value);

    /// <remarks>EMV Book C-2 Section CVM.17</remarks>
    public bool IsFailureControlSupported() => _Value.GetMaskedValue(0b00111111) != 0;

    /// <exception cref="TerminalDataException"></exception>
    /// <remarks>EMV Book C-2 Section CVM.17</remarks>
    public bool IsSupported(TerminalCapabilities terminalCapabilities)
    {
        if (IsFailureControlSupported())
            return false;

        if (!CvmCodes.Exists(_Value))
        {
            throw new
                TerminalDataException($"An unrecognized {nameof(CvmCode)} cannot be processed by the terminal. Please check if the {nameof(CvmCode)} is recognized by the terminal before verifying support");
        }

        if (_Value == CvmCodes.Fail)
            return false;

        if (_Value == CvmCodes.None)
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

        return false;
    }

    public bool IsFailIfUnsuccessfulSet() => !_Value.IsBitSet(Bits.Eight);
    public bool IsTryNextIfUnsuccessfulSet() => _Value.IsBitSet(Bits.Seven);

    #endregion

    #region Operator Overrides

    public static explicit operator byte(CvmCode value) => value._Value;

    #endregion
}