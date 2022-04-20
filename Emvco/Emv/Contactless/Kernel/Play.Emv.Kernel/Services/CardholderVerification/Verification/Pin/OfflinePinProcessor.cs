using System;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.Verification;

// TODO: Book 3 Section 10.5.1 Offline PIN Processing
internal class OfflinePinProcessor : IVerifyCardholderPinOffline
{
    #region Instance Values

    private readonly TerminalCapabilities _TerminalCapabilities;
    private int _RemainingRetries = 2;

    #endregion

    #region Constructor

    public OfflinePinProcessor(TerminalCapabilities terminalCapabilities)
    {
        _TerminalCapabilities = terminalCapabilities;
    }

    #endregion

    #region Instance Members

    /*  TODO: The following was specified in EMV Book 3 Section 10.5.1, but I read online this is contact specific
     *  The terminal bypassed PIN entry at the direction of either the merchant or 
        the cardholder.12 In this case, the terminal shall set the ‘PIN entry required, 
        PIN pad present, but PIN was not entered’ bit in the TVR to 1. The terminal 
        shall consider this CVM unsuccessful and shall continue cardholder 
        verification processing in accordance with the card’s CVM List.
     */
    /*  TODO:The following was specified in EMV Book 3 Section 10.5.1
     *  The PIN is blocked upon initial use of the VERIFY command or if recovery of 
        the enciphered PIN Block has failed (the ICC returns SW1 SW2 = '6983' or 
        '6984' in response to the VERIFY command). In this case, the terminal shall 
        set the ‘PIN Try Limit exceeded’ bit in the TVR to 1.
     */
    /*  TODO:The following was specified in EMV Book 3 Section 10.5.1
     *  The number of remaining PIN tries is reduced to zero (indicated by an 
        SW1 SW2 of '63C0' in the response to the VERIFY command). In this case, 
        the terminal shall set the ‘PIN Try Limit exceeded’ bit in the TVR to 1.
     */

    /// <summary>
    ///     Process
    /// </summary>
    /// <param name="database"></param>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    public CvmCode Process(KernelDatabase database)
    {
        try
        { }
        catch (Exception exception)
        {
            // EMV Book 3 Section 10.5.1
            database.Set(TerminalVerificationResultCodes.PinEntryRequiredAndPinPadNotPresentOrNotWorking);
        }

        throw new NotImplementedException();
    }

    #endregion
}