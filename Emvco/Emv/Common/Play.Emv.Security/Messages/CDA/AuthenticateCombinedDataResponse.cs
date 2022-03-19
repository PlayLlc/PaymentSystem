using System;

using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;

namespace Play.Emv.Security.Messages.CDA;

public class AuthenticateCombinedDataResponse
{
    #region Instance Values

    private readonly TerminalVerificationResult _TerminalVerificationResult;
    private readonly ErrorIndication _ErrorIndication;
    private readonly IccDynamicNumber? _IccDynamicNumber;
    private readonly ApplicationCryptogram? _ApplicationCryptogram;

    #endregion

    #region Constructor

    public AuthenticateCombinedDataResponse(
        TerminalVerificationResult terminalVerificationResult,
        ErrorIndication errorIndication,
        ApplicationCryptogram? applicationCryptogram,
        IccDynamicNumber? iccDynamicNumber)
    {
        _TerminalVerificationResult = terminalVerificationResult;
        _ErrorIndication = errorIndication;
        _ApplicationCryptogram = applicationCryptogram;
        _IccDynamicNumber = iccDynamicNumber;
    }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="terminalVerificationResult"></param>
    /// <param name="errorIndication"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public AuthenticateCombinedDataResponse(TerminalVerificationResult terminalVerificationResult, ErrorIndication errorIndication)
    {
        if (!terminalVerificationResult.CombinationDataAuthenticationFailed())
        {
            throw new
                InvalidOperationException($"The object {nameof(AuthenticateCombinedData1Command)} could not be initialized. The {nameof(TerminalVerificationResult)} was expected to indicate the authentication failed but did not. If the authentication passed then please use the other constructor");
        }

        _TerminalVerificationResult = terminalVerificationResult;
        _ErrorIndication = errorIndication;
        _ApplicationCryptogram = null;
        _IccDynamicNumber = null;
    }

    #endregion

    #region Instance Members

    public TerminalVerificationResult GetTerminalVerificationResult() => _TerminalVerificationResult;
    public ErrorIndication GetErrorIndication() => _ErrorIndication;

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