﻿using Play.Core.Exceptions;
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
        if (!CardholderVerificationMethods.Exists(_Value))
            return false;

        if (_Value == CardholderVerificationMethods.Fail)
            return true;

        if (_Value == CardholderVerificationMethods.NoCvmRequired)
            return terminalCapabilities.IsNoCardVerificationMethodRequiredSupported();
        if (_Value == CardholderVerificationMethods.OfflineEncipheredPin)
            return terminalCapabilities.IsEncipheredPinForOfflineVerificationSupported();

        if (_Value == CardholderVerificationMethods.OfflineEncipheredPinAndSignature)
        {
            return terminalCapabilities.IsEncipheredPinForOfflineVerificationSupported()
                && terminalCapabilities.IsSignaturePaperSupported();
        }

        if (_Value == CardholderVerificationMethods.OfflinePlaintextPin)
            return terminalCapabilities.IsPlaintextPinForIccVerificationSupported();
        if (_Value == CardholderVerificationMethods.OfflinePlaintextPinAndSignature)
            return terminalCapabilities.IsPlaintextPinForIccVerificationSupported() && terminalCapabilities.IsSignaturePaperSupported();
        if (_Value == CardholderVerificationMethods.OnlineEncipheredPin)
            return terminalCapabilities.IsEncipheredPinForOnlineVerificationSupported();
        if (_Value == CardholderVerificationMethods.SignaturePaper)
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