using System;

using Play.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Security.Cryptograms;
using Play.Emv.Security.Messaging;

namespace Play.Emv.Security.Authentications;

public class AuthenticateCombinedDataResponse : SecurityResponse
{
    #region Instance Values

    private readonly IccDynamicNumber? _IccDynamicNumber;
    private readonly ApplicationCryptogram? _ApplicationCryptogram;

    #endregion

    #region Constructor

    public AuthenticateCombinedDataResponse(
        TerminalVerificationResult terminalVerificationResult,
        ErrorIndication errorIndication,
        ApplicationCryptogram? applicationCryptogram,
        IccDynamicNumber? iccDynamicNumber) : base(terminalVerificationResult, errorIndication)
    {
        _ApplicationCryptogram = applicationCryptogram;
        _IccDynamicNumber = iccDynamicNumber;
    }

    public AuthenticateCombinedDataResponse(TerminalVerificationResult terminalVerificationResult, ErrorIndication errorIndication) : base(
        terminalVerificationResult, errorIndication)
    {
        if (!terminalVerificationResult.IsCombinationDataAuthenticationFailedSet())
        {
            throw new InvalidOperationException(
                $"The object {nameof(AuthenticateCombinedData1Command)} could not be initialized. The {nameof(TerminalVerificationResult)} was expected to indicate the authentication failed but did not. If the authentication passed then please use the other constructor");
        }

        _ApplicationCryptogram = null;
        _IccDynamicNumber = null;
    }

    #endregion

    #region Instance Members

    public bool TryGetIccDynamicNumber(out IccDynamicNumber? result)
    {
        if (_IccDynamicNumber == null)
        {
            result = null;

            return false;
        }

        result = _IccDynamicNumber;

        return true;
    }

    public bool TryGetApplicationCryptogram(out ApplicationCryptogram? result)
    {
        if (_ApplicationCryptogram == null)
        {
            result = null;

            return false;
        }

        result = _ApplicationCryptogram;

        return true;
    }

    #endregion
}