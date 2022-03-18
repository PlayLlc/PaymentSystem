using System;

using Play.Emv.DataElements;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.Verification;

// TODO: Book 3 Section 10.5.2 Offline PIN Processing
internal class OnlinePinProcessor : IVerifyCardholderPinOnline
{
    #region Instance Members

    /*  TODO: The following was specified in EMV Book 3 Section 10.5.2, but I read online this is contact specific
    *  The terminal bypassed PIN entry at the direction of either the merchant or 
       the cardholder.12 In this case, the terminal shall set the ‘PIN entry required, 
       PIN pad present, but PIN was not entered’ bit in the TVR to 1. The terminal 
       shall consider this CVM unsuccessful and shall continue cardholder 
       verification processing in accordance with the card’s CVM List.
    */
    public CvmCode Process(KernelDatabase database)
    {
        try
        { }
        catch (Exception exception)
        {
            // EMV Book 3 Section 10.5.2
            database.Set(TerminalVerificationResultCodes.PinEntryRequiredAndPinPadNotPresentOrNotWorking);
        }

        throw new NotImplementedException();
    }

    #endregion
}