using System;
using System.Collections.Generic;
using System.Linq;

using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Services.Selection.CvmConditions;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services.Selection;

/// <summary>
///     This object acts as a store for the list of <see cref="CvmRule" /> objects derived from the <see cref="CvmList" />.
///     It iterates through each rule until it finds one that is compatible with the terminal
/// </summary>
/// <remarks>EMV Book C-2 Section CVM.9 - CVM.25</remarks>
internal class CvmQueue
{
    #region Instance Values

    public readonly int Count;
    private readonly List<CvmRule> _Rules;
    private readonly Money _XAmount;
    private readonly Money _YAmount;
    private int _Offset;

    #endregion

    #region Constructor

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public CvmQueue(CvmList cvmList, NumericCurrencyCode currencyCode)
    {
        if (!cvmList.TryGetCardholderVerificationRules(out CvmRule[]? cvmRules))
            throw new TerminalDataException($"An attempt was made to initialize the {nameof(CvmQueue)} with an empty {nameof(CvmList)}");

        _XAmount = cvmList.GetXAmount(currencyCode);
        _YAmount = cvmList.GetYAmount(currencyCode);
        _Rules = cvmRules!.ToList();
        Count = _Rules.Count;
    }

    #endregion

    #region Instance Members

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public bool TrySelect(KernelDatabase database)
    {
        if (_Rules.Count < (_Offset - 1))
            return false;

        TerminalCapabilities terminalCapabilities = database.Get<TerminalCapabilities>(TerminalCapabilities.Tag);

        for (; _Offset < _Rules.Count; _Offset++)
        {
            CvmRule currentCvmRule = _Rules[_Offset];

            //CVM.10
            if (!CvmCondition.TryGet(currentCvmRule.GetCvmConditionCode(), out CvmCondition? cvmCondition))
                continue;

            //CVM.11
            if (!CvmCondition.IsCvmSupported(database, cvmCondition!.GetConditionCode(), _XAmount, _YAmount))
                continue;

            //CVM.15
            if (!currentCvmRule.GetCvmCode().IsRecognized())
            {
                HandleUnrecognizedRule(database);

                if (IsContinueOnFailureAllowed(currentCvmRule.GetCvmCode()))
                    continue;

                HandleInvalidRule(database, currentCvmRule.GetCvmCode(), currentCvmRule.GetCvmConditionCode());

                return false;
            }

            //CVM.17
            if (!currentCvmRule.GetCvmCode().IsSupported(terminalCapabilities))
            {
                //  Book 3 Section 10.5
                if (IsPinRequiredButNotAvailable(currentCvmRule.GetCvmCode(), database))
                    SetPinRequiredButNotSupported(database);

                //CVM.19
                if (currentCvmRule.GetCvmCode().IsTryNextIfUnsuccessfulSet())
                    continue;

                //CVM.22
                HandleInvalidRule(database, currentCvmRule.GetCvmCode(), currentCvmRule.GetCvmConditionCode());

                return false;
            }

            //CVM.18
            HandleSuccessfulSelect(database, currentCvmRule.GetCvmCode(), currentCvmRule.GetCvmConditionCode());

            return true;
        }

        HandleInvalidRule(database, new CvmCode(0), new CvmConditionCode(0));

        return false;
    }

    #region CVM.16

    /// <exception cref="TerminalDataException"></exception>
    public void HandleUnrecognizedRule(KernelDatabase database)
    {
        database.Set(TerminalVerificationResultCodes.UnrecognizedCvm);
    }

    #endregion

    #region CVM.19

    public bool IsContinueOnFailureAllowed(CvmCode cvmCode) => cvmCode.IsTryNextIfUnsuccessfulSet();

    #endregion

    #region CVM.22 - CVM.25

    /// <summary>
    ///     HandleInvalidRule
    /// </summary>
    /// <param name="database"></param>
    /// <param name="cvmCode"></param>
    /// <param name="cvmConditionCode"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void HandleInvalidRule(KernelDatabase database, CvmCode cvmCode, CvmConditionCode cvmConditionCode)
    {
        SetCardholderVerificationWasNotSuccessful(database);

        // CVM.23
        if (!cvmCode.IsFailureControlSupported())
            SetCvmProcessedToFailedCvm(database, cvmCode, cvmConditionCode); //24
        else
            SetCvmProcessedToNone(database); //25
    }

    #endregion

    #region CVM.22

    /// <summary>
    ///     SetCardholderVerificationWasNotSuccessful
    /// </summary>
    /// <param name="database"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void SetCardholderVerificationWasNotSuccessful(KernelDatabase database)
    {
        database.Set(TerminalVerificationResultCodes.CardholderVerificationWasNotSuccessful);
    }

    #endregion

    #region CVM.24

    /// <summary>
    ///     SetCvmProcessedToFailedCvm
    /// </summary>
    /// <param name="database"></param>
    /// <param name="cvmCode"></param>
    /// <param name="cvmConditionCode"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void SetCvmProcessedToFailedCvm(KernelDatabase database, CvmCode cvmCode, CvmConditionCode cvmConditionCode)
    {
        CvmResults cvmResults = new(cvmCode, cvmConditionCode, CvmResultCodes.Failed);
        database.Update(cvmResults);
    }

    #endregion

    #region CVM.25

    /// <summary>
    ///     SetCvmProcessedToNone
    /// </summary>
    /// <param name="database"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void SetCvmProcessedToNone(KernelDatabase database)
    {
        CvmResults cvmResults = new(CvmCodes.None, new CvmConditionCode(0), CvmResultCodes.Failed);
        database.Update(cvmResults);
    }

    #endregion

    #endregion

    #region Book 3 Section 10.5

    /// <summary>
    ///     In addition, the terminal shall set the ‘PIN entry required and PIN pad not present or not working’ bit(b5 of byte
    ///     3) of the TVR to 1 for the following cases:
    ///     o The CVM was online PIN and online PIN was not supported
    ///     o The CVM included any form of offline PIN, and neither form of offline PIN was supported
    /// </summary>
    /// <remarks>EMV Book 3 Section 10.5</remarks>
    private bool IsPinRequiredButNotAvailable(CvmCode cvmCode, KernelDatabase database)
    {
        if (cvmCode == CvmCodes.OfflineEncipheredPin)
            return false;

        if (cvmCode == CvmCodes.OfflineEncipheredPinAndSignature)
            return false;

        if (cvmCode == CvmCodes.OfflinePlaintextPinAndSignature)
            return false;

        if (cvmCode == CvmCodes.OfflinePlaintextPin)
            return false;

        if (cvmCode == CvmCodes.OnlineEncipheredPin)
            return false;

        return true;
    }

    /// <summary>
    ///     In addition, the terminal shall set the ‘PIN entry required and PIN pad not present or not working’ bit(b5 of byte
    ///     3) of the TVR to 1 for the following cases:
    ///     o The CVM was online PIN and online PIN was not supported
    ///     o The CVM included any form of offline PIN, and neither form of offline PIN was supported
    /// </summary>
    /// <remarks>EMV Book 3 Section 10.5</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void SetPinRequiredButNotSupported(KernelDatabase database)
    {
        database.Set(TerminalVerificationResultCodes.PinEntryRequiredAndPinPadNotPresentOrNotWorking);
    }

    #endregion

    #region CVM.18

    /// <summary>
    ///     HandleSuccessfulSelect
    /// </summary>
    /// <param name="database"></param>
    /// <param name="cvmCode"></param>
    /// <param name="cvmConditionCode"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void HandleSuccessfulSelect(KernelDatabase database, CvmCode cvmCode, CvmConditionCode cvmConditionCode)
    {
        database.Update(new CvmResults(_Rules[_Offset].GetCvmCode(), _Rules[_Offset].GetCvmConditionCode(), CvmResultCodes.Unknown));

        if (cvmCode == CvmCodes.OnlineEncipheredPin)
        {
            HandleSuccessfulOnlineEncipheredPinSelection(database, cvmCode);

            return;
        }

        if (cvmCode == CvmCodes.SignaturePaper)
        {
            HandleSuccessfulSignatureSelection(database, cvmCode, cvmConditionCode);

            return;
        }

        if (cvmCode == CvmCodes.None)
        {
            HandleSuccessfulNoneSelection(database, cvmCode, cvmConditionCode);

            return;
        }

        HandleSuccessfulProprietarySelect(database, cvmCode, cvmConditionCode);
    }

    /// <summary>
    ///     HandleSuccessfulOnlineEncipheredPinSelection
    /// </summary>
    /// <param name="database"></param>
    /// <param name="cvmCode"></param>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleSuccessfulOnlineEncipheredPinSelection(KernelDatabase database, CvmCode cvmCode)
    {
        CvmResults results = new(cvmCode, new CvmConditionCode(0), CvmResultCodes.Unknown);
        database.Set(TerminalVerificationResultCodes.OnlinePinEntered);
        database.Update(results);
        database.Update(CvmPerformedOutcome.OnlinePin);
    }

    /// <summary>
    ///     HandleSuccessfulSignatureSelection
    /// </summary>
    /// <param name="database"></param>
    /// <param name="cvmCode"></param>
    /// <param name="cvmConditionCode"></param>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleSuccessfulSignatureSelection(KernelDatabase database, CvmCode cvmCode, CvmConditionCode cvmConditionCode)
    {
        CvmResults results = new(cvmCode, cvmConditionCode, CvmResultCodes.Unknown);
        database.SetIsReceiptPresent(true);
        database.Update(CvmPerformedOutcome.ObtainSignature);
        database.Update(results);
    }

    /// <summary>
    ///     HandleSuccessfulNoneSelection
    /// </summary>
    /// <param name="database"></param>
    /// <param name="cvmCode"></param>
    /// <param name="cvmConditionCode"></param>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleSuccessfulNoneSelection(KernelDatabase database, CvmCode cvmCode, CvmConditionCode cvmConditionCode)
    {
        CvmResults results = new(cvmCode, cvmConditionCode, CvmResultCodes.Successful);
        database.Update(CvmPerformedOutcome.NoCvm);
        database.Update(results);
    }

    /// <summary>
    ///     HandleSuccessfulProprietarySelect
    /// </summary>
    /// <param name="database"></param>
    /// <param name="cvmCode"></param>
    /// <param name="cvmConditionCode"></param>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleSuccessfulProprietarySelect(KernelDatabase database, CvmCode cvmCode, CvmConditionCode cvmConditionCode)
    {
        CvmResults results = new(cvmCode, cvmConditionCode, CvmResultCodes.Unknown);
        database.Update(CvmPerformedOutcome.NoCvm);
        database.Update(results);
    }

    #endregion
}