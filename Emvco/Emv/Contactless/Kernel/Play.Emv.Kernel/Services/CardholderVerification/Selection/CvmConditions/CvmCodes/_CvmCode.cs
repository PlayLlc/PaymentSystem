using Play.Emv.Database;

namespace Play.Emv.Kernel.Services.Selection.CvmConditions.CvmCodes;

internal class _CvmCode
{
    #region Instance Members


  

    /// <remarks>EMV Book 3 Section 10.5.2</remarks>
    public static void IsOnlinePinSupported()
    {
 




        /*
         * If online PIN processing is a required CVM as determined by the above process, 
the processing may not be successfully performed for any one of the following 
reasons: 


• The terminal supports online PIN, but the PIN pad is malfunctioning. In this 
case, the terminal shall set the ‘PIN entry required and PIN pad not present 
or not working’ bit in the TVR to 1.
• The terminal bypassed PIN entry at the direction of either the merchant or 
the cardholder. In this case, the terminal shall set the ‘PIN entry required, 
PIN pad present, but PIN was not entered’ bit in the TVR to 1.
If the online PIN is successfully entered, the terminal shall set the ‘Online PIN 
entered’ bit in the TVR to 1. In this case, cardholder verification is considered 
successful and complete.
         */
    }

    /// <remarks>EMV Book 3 Section 10.5.1</remarks>
    public static void IsOfflinePinSupported()
    {
        /*
         * This section applies to the verification by the ICC of a plaintext or enciphered 
PIN presented by the terminal.
If an offline PIN is the selected CVM as determined by the above process, offline 
PIN processing may not be successfully performed for any one of the following 
reasons:
• The terminal does not support offline PIN.11 In this case, the terminal shall 
set the ‘PIN entry required and PIN pad not present or not working’ bit in the 
TVR to 1.
• The terminal supports offline PIN, but the PIN pad is malfunctioning. In this 
case, the terminal shall set the ‘PIN entry required and PIN pad not present 
or not working’ bit in the TVR to 1.
• The terminal bypassed PIN entry at the direction of either the merchant or 
the cardholder.12 In this case, the terminal shall set the ‘PIN entry required, 
PIN pad present, but PIN was not entered’ bit in the TVR to 1. The terminal 
shall consider this CVM unsuccessful and shall continue cardholder 
verification processing in accordance with the card’s CVM List.
• The PIN is blocked upon initial use of the VERIFY command or if recovery of 
the enciphered PIN Block has failed (the ICC returns SW1 SW2 = '6983' or 
'6984' in response to the VERIFY command). In this case, the terminal shall 
set the ‘PIN Try Limit exceeded’ bit in the TVR to 1.
• The number of remaining PIN tries is reduced to zero (indicated by an 
SW1 SW2 of '63C0' in the response to the VERIFY command). In this case, 
the terminal shall set the ‘PIN Try Limit exceeded’ bit in the TVR to 1.
         */
    }

    /// <remarks>EMV Book 3 Section 10.5.1</remarks>
    public static bool IsSignatureSupported()
    {
        /*
         * If a (paper) signature is a required CVM as determined by the above process, the 
terminal shall determine success based upon the terminal’s capability to support 
the signature process (see complementary payment systems documentation for 
additional information). If the terminal is able to support signature, the process 
is considered successful, and cardholder verification is complete.
         */
    }

    #endregion

}