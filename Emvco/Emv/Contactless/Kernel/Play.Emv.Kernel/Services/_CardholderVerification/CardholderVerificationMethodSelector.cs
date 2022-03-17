﻿using System;

using Play.Emv.DataElements;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services._CardholderVerification;

public class CardholderVerificationMethodSelector : ISelectCardholderVerificationMethod
{
    #region Instance Members

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public void Process(KernelDatabase database)
    {
        ApplicationInterchangeProfile applicationInterchangeProfile =
            ApplicationInterchangeProfile.Decode(database.Get(ApplicationInterchangeProfile.Tag).EncodeValue().AsSpan());
        TransactionCurrencyCode transactionCurrencyCode =
            TransactionCurrencyCode.Decode(database.Get(TransactionCurrencyCode.Tag).EncodeValue().AsSpan());
        AmountAuthorizedNumeric amountAuthorizedNumeric =
            AmountAuthorizedNumeric.Decode(database.Get(AmountAuthorizedNumeric.Tag).EncodeValue().AsSpan());
        ReaderCvmRequiredLimit readerCvmRequiredLimit =
            ReaderCvmRequiredLimit.Decode(database.Get(ReaderCvmRequiredLimit.Tag).EncodeValue().AsSpan());

        if (IsOfflineVerificationSupported(applicationInterchangeProfile, database))
        {
            CreateResultForOfflineVerification(database, transactionCurrencyCode, amountAuthorizedNumeric, readerCvmRequiredLimit);

            return;
        }

        if (!IsCardholderVerificationSupported(database))
        {
            CreateResultForCardholderVerificationNotSupported(database);

            return;
        }

        if (IsCvmListEmpty(database, out CvmList? cvmList))
            CreateIccDataMissingCvmResult(database);

        throw new NotImplementedException();
    }

    #region CVM.1

    public static bool IsOfflineVerificationSupported(ApplicationInterchangeProfile aip, KernelDatabase database)
    {
        if (!aip.IsOnDeviceCardholderVerificationSupported())
            return false;

        return database.IsOnDeviceCardholderVerificationSupported();
    }

    #endregion

    #region CVM.2 - CVM.4

    // BUG: What if the transaction's currency is different than the base currency of the application? We need to make sure we're using TransactionReferenceCurrencyCode if needed. Go back and check the logic to implement that for terminals that support more than one currency
    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    public void CreateResultForOfflineVerification(
        KernelDatabase database,
        TransactionCurrencyCode currencyCode,
        AmountAuthorizedNumeric transactionAmount,
        ReaderCvmRequiredLimit readerCvmThreshold)
    {
        if (transactionAmount.AsMoney(currencyCode) > readerCvmThreshold.AsMoney(currencyCode))
        {
            SetOfflinePlaintextPinCvmResults(database);

            return;
        }

        SetNoCvmResults(database);
    }

    #endregion

    #region CVM.3

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    public static void SetNoCvmResults(KernelDatabase database)
    {
        database.Update(CvmPerformedOutcome.NoCvm);
        CvmResults results = new(CardholderVerificationMethodCodes.None, new CvmConditionCode(0),
                                 CardholderVerificationMethodResultCodes.Successful);
        database.Update(results);
    }

    #endregion

    #region CVM.4

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    public static void SetOfflinePlaintextPinCvmResults(KernelDatabase database)
    {
        database.Update(CvmPerformedOutcome.ConfirmationCodeVerified);
        CvmResults results = new(CardholderVerificationMethodCodes.OfflinePlaintextPin, new CvmConditionCode(0),
                                 CardholderVerificationMethodResultCodes.Successful);
        database.Update(results);
    }

    #endregion

    #region CVM.5

    public static bool IsCardholderVerificationSupported(KernelDatabase database) => database.IsOnDeviceCardholderVerificationSupported();

    #endregion

    #region CVM.6

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    public static void CreateResultForCardholderVerificationNotSupported(KernelDatabase database)
    {
        database.Update(CvmPerformedOutcome.NoCvm);
        CvmResults results = new(CardholderVerificationMethodCodes.None, new CvmConditionCode(0),
                                 CardholderVerificationMethodResultCodes.Unknown);
        database.Update(results);
    }

    #endregion

    #region CVM.7

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    public static bool IsCvmListEmpty(KernelDatabase database, out CvmList? cvmList)
    {
        if (!database.IsPresentAndNotEmpty(CvmList.Tag))
        {
            cvmList = null;

            return true;
        }

        cvmList = CvmList.Decode(database.Get(CvmList.Tag).EncodeValue().AsSpan());

        if (!cvmList.AreCardholderVerificationRulesPresent())
            return true;

        return false;
    }

    #endregion

    #region CVM.8

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    public static void CreateIccDataMissingCvmResult(KernelDatabase database)
    {
        database.Update(CvmPerformedOutcome.NoCvm);
        CvmResults results = new(CardholderVerificationMethodCodes.None, new CvmConditionCode(0),
                                 CardholderVerificationMethodResultCodes.Unknown);
        database.Update(results);
        database.Set(TerminalVerificationResultCodes.IccDataMissing);
    }

    #endregion

    #region CVM.9 - Selection Loop

    public static void Select(KernelDatabase database, CvmList cvmList, ApplicationCurrencyCode currencyCode)
    {
        CvmQueue cvmQueue = new(database, cvmList, currencyCode);

        for (int i = 0; i < cvmQueue.Count; i++)
        {
            if (!cvmQueue.TrySelect(database, out CvmRule? rule))
            {
                CVM22();

                return;
            }
        }
    }

    #endregion

    private static void CVM22()
    { }

    #endregion
}