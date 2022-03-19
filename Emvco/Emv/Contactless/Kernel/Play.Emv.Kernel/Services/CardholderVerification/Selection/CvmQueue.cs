using System;
using System.Collections.Generic;
using System.Linq;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
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
    private int _Offset = 0;

    #endregion

    #region Constructor

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
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
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public bool TrySelect(KernelDatabase database)
    {
        if (_Rules.Count < (_Offset - 1))
            return false;

        TerminalCapabilities terminalCapabilities =
            TerminalCapabilities.Decode(database.Get(TerminalCapabilities.Tag).EncodeValue().AsSpan());

        for (; _Offset < _Rules.Count; _Offset++)
        {
            if (!CvmCondition.TryGet(_Rules[_Offset].GetCvmConditionCode(), out CvmCondition? cvmCondition))
                continue;

            if (CvmCondition.IsCvmSupported(database, cvmCondition!.GetConditionCode(), _XAmount, _YAmount))
                continue;

            if (!_Rules[_Offset].GetCvmCode().IsRecognized())
            {
                HandleUnrecognizedRule(database);

                if (IsContinueOnFailureAllowed(_Rules[_Offset].GetCvmCode()))
                    continue;

                HandleInvalidRule(database, _Rules[_Offset].GetCvmCode(), _Rules[_Offset].GetCvmConditionCode());
            }

            if (!_Rules[_Offset].GetCvmCode().IsSupported(terminalCapabilities))
            {
                //  Book 3 Section 10.5
                if (IsPinRequiredButNotAvailable(_Rules[_Offset].GetCvmCode(), database))
                    SetPinRequiredButNotSupported(database);

                continue;
            }

            if (!_Rules[_Offset].GetCvmCode().IsTryNextIfUnsuccessfulSet())
                continue;

            HandleSuccessfulSelect(database, _Rules[_Offset++].GetCvmCode(), _Rules[_Offset++].GetCvmConditionCode());

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

    public void HandleInvalidRule(KernelDatabase database, CvmCode cvmCode, CvmConditionCode cvmConditionCode)
    {
        SetCardholderVerificationWasNotSuccessful(database);

        // CVM.23
        if (cvmCode.IsFailureControlSupported())
            SetCvmProcessedToFailedCvm(database, cvmCode, cvmConditionCode);

        SetCvmProcessedToNone(database);
    }

    #endregion

    #region CVM.22

    public void SetCardholderVerificationWasNotSuccessful(KernelDatabase database)
    {
        database.Set(TerminalVerificationResultCodes.CardholderVerificationWasNotSuccessful);
    }

    #endregion

    #region CVM.24

    public void SetCvmProcessedToFailedCvm(KernelDatabase database, CvmCode cvmCode, CvmConditionCode cvmConditionCode)
    {
        CvmResults cvmResults = new(cvmCode, cvmConditionCode, CvmResultCodes.Failed);
        database.Update(cvmResults);
    }

    #endregion

    #region CVM.25

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
    private void SetPinRequiredButNotSupported(KernelDatabase database)
    {
        database.Set(TerminalVerificationResultCodes.PinEntryRequiredAndPinPadNotPresentOrNotWorking);
    }

    #endregion

    #region CVM.18

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

    private void HandleSuccessfulOnlineEncipheredPinSelection(KernelDatabase database, CvmCode cvmCode)
    {
        CvmResults results = new(cvmCode, new CvmConditionCode(0), CvmResultCodes.Unknown);
        database.Set(TerminalVerificationResultCodes.OnlinePinEntered);
        database.Update(results);
        database.Update(CvmPerformedOutcome.OnlinePin);
    }

    private void HandleSuccessfulSignatureSelection(KernelDatabase database, CvmCode cvmCode, CvmConditionCode cvmConditionCode)
    {
        CvmResults results = new(cvmCode, cvmConditionCode, CvmResultCodes.Unknown);
        database.SetIsReceiptPresent(true);
        database.Update(CvmPerformedOutcome.ObtainSignature);
        database.Update(results);
    }

    private void HandleSuccessfulNoneSelection(KernelDatabase database, CvmCode cvmCode, CvmConditionCode cvmConditionCode)
    {
        CvmResults results = new(cvmCode, cvmConditionCode, CvmResultCodes.Successful);
        database.Update(CvmPerformedOutcome.NoCvm);
        database.Update(results);
    }

    private void HandleSuccessfulProprietarySelect(KernelDatabase database, CvmCode cvmCode, CvmConditionCode cvmConditionCode)
    {
        CvmResults results = new(cvmCode, cvmConditionCode, CvmResultCodes.Unknown);
        database.Update(CvmPerformedOutcome.NoCvm);
        database.Update(results);
    }

    #endregion
}