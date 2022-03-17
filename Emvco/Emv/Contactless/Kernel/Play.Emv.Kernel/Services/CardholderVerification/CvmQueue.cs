using System;
using System.Collections.Generic;
using System.Linq;

using Play.Emv.Database;
using Play.Emv.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services;

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

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public CvmQueue(IQueryTlvDatabase database, CvmList cvmList, ApplicationCurrencyCode currencyCode)
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

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
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

            if (CvmCondition.IsConditionSatisfied(cvmCondition!.GetConditionCode(), database))
                continue;

            if (!_Rules[_Offset].GetCvmCode().IsRecognized())
            {
                // CVM.16
                HandleUnrecognizedRule(database);

                if (IsContinueOnFailureAllowed(_Rules[_Offset].GetCvmCode()))
                    continue;

                HandleInvalidRule(database, _Rules[_Offset].GetCvmCode(), _Rules[_Offset].GetCvmConditionCode());
            }

            if (!_Rules[_Offset].GetCvmCode().IsSupported(terminalCapabilities))
            {
                _Offset = Count;

                continue;
            }

            if (!_Rules[_Offset].GetCvmCode().IsTryNextIfUnsuccessfulSet())
            {
                _Offset = Count;

                continue;
            }

            HandleSuccessfulSelect(database, _Rules[_Offset++]);

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

    #region CVM.18

    public void HandleSuccessfulSelect(KernelDatabase database, CvmRule rule)
    {
        database.Update(new CvmResults(_Rules[_Offset].GetCvmCode(), _Rules[_Offset].GetCvmConditionCode(),
                                       CardholderVerificationMethodResultCodes.Unknown));

        // 
        // TODO: CVM.18 -> if(code..blah balh math == whatever)
        database.Set(TerminalVerificationResultCodes.OnlinePinEntered);
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
        CvmResults cvmResults = new(cvmCode, cvmConditionCode, CardholderVerificationMethodResultCodes.Failed);
        database.Update(cvmResults);
    }

    #endregion

    #region CVM.25

    public void SetCvmProcessedToNone(KernelDatabase database)
    {
        CvmResults cvmResults = new(CardholderVerificationMethodCodes.None, new CvmConditionCode(0),
                                    CardholderVerificationMethodResultCodes.Failed);
        database.Update(cvmResults);
    }

    #endregion

    #endregion
}