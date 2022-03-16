using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements.CvmConditions;

internal abstract record CvmCondition
{
    #region Static Metadata

    private static readonly Dictionary<CvmConditionCode, CvmCondition> _Conditions;

    #endregion

    #region Instance Values

    protected abstract Tag[] RequiredData { get; }

    #endregion

    #region Constructor

    static CvmCondition()
    {
        var alwaysCondition = new AlwaysCondition();
        var amountInApplicationCurrencyAndOverXValueCondition = new AmountInApplicationCurrencyAndOverXValueCondition();
        var amountInApplicationCurrencyAndOverYValueCondition = new AmountInApplicationCurrencyAndOverYValueCondition();
        var amountInApplicationCurrencyAndUnderXValueCondition = new AmountInApplicationCurrencyAndUnderXValueCondition();
        var amountInApplicationCurrencyAndUnderYValueCondition = new AmountInApplicationCurrencyAndUnderXValueCondition();
        var manualCashCondition = new ManualCashCondition();
        var notUnattendedCashOrManualCashOrPurchaseWithCashback = new NotUnattendedCashOrManualCashOrPurchaseWithCashback();
        var purchaseWithCashbackCondition = new PurchaseWithCashbackCondition();
        var supportsCvmCondition = new SupportsCvmCondition();

        _Conditions = new Dictionary<CvmConditionCode, CvmCondition>
        {
            {alwaysCondition.GetConditionCode(), alwaysCondition},
            {amountInApplicationCurrencyAndOverXValueCondition.GetConditionCode(), amountInApplicationCurrencyAndOverXValueCondition},
            {amountInApplicationCurrencyAndOverYValueCondition.GetConditionCode(), amountInApplicationCurrencyAndOverYValueCondition},
            {amountInApplicationCurrencyAndUnderXValueCondition.GetConditionCode(), amountInApplicationCurrencyAndUnderXValueCondition},
            {amountInApplicationCurrencyAndUnderYValueCondition.GetConditionCode(), amountInApplicationCurrencyAndUnderYValueCondition},
            {manualCashCondition.GetConditionCode(), manualCashCondition},
            {
                notUnattendedCashOrManualCashOrPurchaseWithCashback.GetConditionCode(),
                notUnattendedCashOrManualCashOrPurchaseWithCashback
            },
            {purchaseWithCashbackCondition.GetConditionCode(), purchaseWithCashbackCondition},
            {supportsCvmCondition.GetConditionCode(), supportsCvmCondition}
        };
    }

    #endregion

    #region Instance Members

    public bool IsConditionSatisfied(CvmConditionCode code, IQueryTlvDatabase database)
    {
        if (!_Conditions.ContainsKey(code))
            return false;

        if (!_Conditions[code].IsRequiredDataPresent(database))
            return false;

        if (!_Conditions[code].IsRequiredDataPresent(database))
            return false;

        return true;
    }

    public abstract CvmConditionCode GetConditionCode();

    protected bool IsCvmSupported(IQueryTlvDatabase database)
    {
        if (!IsRequiredDataPresent(database))
            return false;

        if (!IsConditionSatisfied(database))
            return false;

        return true;
    }

    private bool IsRequiredDataPresent(IQueryTlvDatabase database)
    {
        if (RequiredData.Length == 0)
            return true;

        for (int i = 0; i < RequiredData.Length; i++)
        {
            if (!database.IsPresent(RequiredData[i]))
                return false;
        }

        return true;
    }

    protected abstract bool IsConditionSatisfied(IQueryTlvDatabase database);

    //{
    //    TerminalCapabilities terminalCapabilities =
    //        TerminalCapabilities.Decode(database.Get(TerminalCapabilities.Tag).EncodeValue().AsSpan());

    //    TransactionType transactionType = TransactionType.Decode(database.Get(TransactionType.Tag).EncodeValue().AsSpan());
    //    TerminalType terminalType = TerminalType.Decode(database.Get(TerminalType.Tag).EncodeValue().AsSpan());
    //    PosEntryMode posEntryMode = PosEntryMode.Decode(database.Get(PosEntryMode.Tag).EncodeValue().AsSpan());
    //    AmountAuthorizedNumeric amountAuthorized =
    //        AmountAuthorizedNumeric.Decode(database.Get(AmountAuthorizedNumeric.Tag).EncodeValue().AsSpan());
    //    TransactionCurrencyCode transactionCurrencyCode =
    //        TransactionCurrencyCode.Decode(database.Get(TransactionCurrencyCode.Tag).EncodeValue().AsSpan());
    //    ApplicationCurrencyCode applicationCurrencyCode =
    //        ApplicationCurrencyCode.Decode(database.Get(ApplicationCurrencyCode.Tag).EncodeValue().AsSpan());
    //}

    protected static bool IsUnattended(TerminalType terminalType) =>
        terminalType.IsEnvironmentType(TerminalType.EnvironmentType.Unattended);

    protected static bool IsCashback(TransactionType transactionType) =>
        (transactionType == TransactionTypes.CashAdvance) || (transactionType == TransactionTypes.FastCashDebit);

    protected static bool IsManual(PosEntryMode value) =>
        (value == PosEntryModes.ManualEntry) || (value == PosEntryModes.ManualEntryFallback);

    protected static bool IsManualCash(PosEntryMode posEntryMode, TransactionType transactionType) =>
        IsCashback(transactionType) && IsManual(posEntryMode);

    protected static bool IsPurchase(TransactionType transactionType)
    {
        if (transactionType == TransactionTypes.GoodsAndServicesDebit)
            return true;

        if (transactionType == TransactionTypes.GoodsAndServicesWithCashDisbursementDebit)
            return true;

        return false;
    }

    /// <exception cref="Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    protected static bool IsCashTransaction(IQueryTlvDatabase database)
    {
        TransactionType transactionType = TransactionType.Decode(database.Get(TransactionType.Tag).EncodeValue().AsSpan());

        if (transactionType == TransactionTypes.CashAdvance)
            return true;

        if (transactionType == TransactionTypes.FastCashDebit)
            return true;

        return false;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator byte(CvmCondition value) => (byte) value.GetConditionCode();
    public static implicit operator CvmConditionCode(CvmCondition value) => value.GetConditionCode();

    #endregion
}