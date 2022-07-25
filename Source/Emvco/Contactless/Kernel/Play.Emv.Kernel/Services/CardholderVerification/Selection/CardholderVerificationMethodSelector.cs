﻿using System;

using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services.Selection;

public class CardholderVerificationMethodSelector : ISelectCardholderVerificationMethod
{
    #region Instance Members

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void Process(KernelDatabase database)
    {
        ApplicationInterchangeProfile applicationInterchangeProfile = database.Get<ApplicationInterchangeProfile>(ApplicationInterchangeProfile.Tag);

        AmountAuthorizedNumeric amountAuthorizedNumeric = database.Get<AmountAuthorizedNumeric>(AmountAuthorizedNumeric.Tag);

        ReaderCvmRequiredLimit readerCvmRequiredLimit = database.Get<ReaderCvmRequiredLimit>(ReaderCvmRequiredLimit.Tag);

        NumericCurrencyCode currencyCode = GetCurrencyCode(database);

        if (IsOfflineVerificationSupported(applicationInterchangeProfile, database))
        {
            CreateResultForOfflineVerification(database, currencyCode, amountAuthorizedNumeric, readerCvmRequiredLimit);

            return;
        }

        if (!IsCardholderVerificationSupported(database))
        {
            CreateResultForCardholderVerificationNotSupported(database);

            return;
        }

        if (IsCvmListEmpty(database, out CvmList? cvmList))
        {
            CreateIccDataMissingCvmResult(database);

            return;
        }

        Select(database, cvmList!, currencyCode);
    }

    // HACK: I'm not sure if this is correct. We're attempting to ensure we're using the same base currency when we add, subtract, and compare money values. If the application currency and transaction currency are different then we're using the reference currency. Check to see what the actual logic should be
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private NumericCurrencyCode GetCurrencyCode(KernelDatabase database)
    {
        TransactionCurrencyCode transactionCurrencyCode = database.Get<TransactionCurrencyCode>(TransactionCurrencyCode.Tag);

        ApplicationCurrencyCode applicationCurrencyCode = database.Get<ApplicationCurrencyCode>(ApplicationCurrencyCode.Tag);

        if ((NumericCurrencyCode) transactionCurrencyCode == (NumericCurrencyCode) applicationCurrencyCode)
            return (NumericCurrencyCode) transactionCurrencyCode;

        TransactionReferenceCurrencyCode transactionReferenceCurrencyCode =
            database.Get<TransactionReferenceCurrencyCode>(TransactionReferenceCurrencyCode.Tag);

        return (NumericCurrencyCode) transactionReferenceCurrencyCode;
    }

    #region CVM.1

    /// <summary>
    ///     IsOfflineVerificationSupported
    /// </summary>
    /// <param name="aip"></param>
    /// <param name="database"></param>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    public static bool IsOfflineVerificationSupported(ApplicationInterchangeProfile aip, KernelDatabase database) =>
        aip.IsOnDeviceCardholderVerificationSupported() && database.IsOnDeviceCardholderVerificationSupported();

    #endregion

    #region CVM.2 - CVM.4

    /// <notes>
    ///     Notes: In addition, the terminal shall set the ‘PIN entry required and PIN pad not present or not working’ bit(b5
    ///     of byte 3) of the TVR to 1 for the following cases:
    ///     o The CVM was online PIN and online PIN was not supported
    ///     o The CVM included any form of offline PIN, and neither form of offline PIN was supported
    /// </notes>
    /// <remarks>EMV Book C-2 Section CVM.2 - CVM.4 & EMV Book 3 Section 10.5</remarks>
    /// <exception cref="TerminalDataException"></exception>
    public void CreateResultForOfflineVerification(
        KernelDatabase database, NumericCurrencyCode currencyCode, AmountAuthorizedNumeric transactionAmount, ReaderCvmRequiredLimit readerCvmThreshold)
    {
        if (!database!.IsPlaintextPinForIccVerificationSupported())
            database.Update(TerminalVerificationResultCodes.PinEntryRequiredAndPinPadNotPresentOrNotWorking);

        if (transactionAmount.AsMoney(currencyCode) > readerCvmThreshold.AsMoney(currencyCode))
        {
            SetOfflinePlaintextPinCvmResults(database);

            return;
        }

        SetNoCvmResults(database);
    }

    #endregion

    #region CVM.3

    /// <exception cref="TerminalDataException"></exception>
    public static void SetNoCvmResults(KernelDatabase database)
    {
        database.Update(CvmPerformedOutcome.NoCvm);
        CvmResults results = new(CvmCodes.None, new CvmConditionCode(0), CvmResultCodes.Successful);
        database.Update(results);
    }

    #endregion

    #region CVM.4

    /// <exception cref="TerminalDataException"></exception>
    public static void SetOfflinePlaintextPinCvmResults(KernelDatabase database)
    {
        database.Update(CvmPerformedOutcome.ConfirmationCodeVerified);
        CvmResults results = new(CvmCodes.OfflinePlaintextPin, new CvmConditionCode(0), CvmResultCodes.Successful);
        database.Update(results);
    }

    #endregion

    #region CVM.5

    public static bool IsCardholderVerificationSupported(KernelDatabase database) =>
        database.Get<ApplicationInterchangeProfile>(ApplicationInterchangeProfile.Tag).IsOnDeviceCardholderVerificationSupported();

    #endregion

    #region CVM.6

    /// <exception cref="TerminalDataException"></exception>
    public static void CreateResultForCardholderVerificationNotSupported(KernelDatabase database)
    {
        database.Update(CvmPerformedOutcome.NoCvm);
        CvmResults results = new(CvmCodes.None, new CvmConditionCode(0), CvmResultCodes.Unknown);
        database.Update(results);
    }

    #endregion

    #region CVM.7

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static bool IsCvmListEmpty(KernelDatabase database, out CvmList? cvmList)
    {
        if (!database.IsPresentAndNotEmpty(CvmList.Tag))
        {
            cvmList = null;

            return true;
        }

        cvmList = database.Get<CvmList>(CvmList.Tag);

        if (!cvmList.AreCardholderVerificationRulesPresent())
            return true;

        return false;
    }

    #endregion

    #region CVM.8

    /// <exception cref="TerminalDataException"></exception>
    public static void CreateIccDataMissingCvmResult(KernelDatabase database)
    {
        database.Update(CvmPerformedOutcome.NoCvm);
        CvmResults results = new(CvmCodes.None, new CvmConditionCode(0), CvmResultCodes.Unknown);
        database.Update(results);
        database.Update(TerminalVerificationResultCodes.IccDataMissing);
    }

    #endregion

    #region CVM.9 - CVM.25 - Selection Loop

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static void Select(KernelDatabase database, CvmList cvmList, NumericCurrencyCode currencyCode)
    {
        CvmQueue cvmQueue = new(cvmList, currencyCode);

        for (int i = 0; i < cvmQueue.Count; i++)
        {
            if (cvmQueue.TrySelect(database))
                return;
        }
    }

    #endregion

    #endregion
}