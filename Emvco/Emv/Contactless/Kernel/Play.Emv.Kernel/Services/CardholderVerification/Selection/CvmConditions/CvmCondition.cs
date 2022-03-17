using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Play.Ber.Identifiers;
using Play.Emv.Database;
using Play.Emv.DataElements;
using Play.Emv.Exceptions;

namespace Play.Emv.Kernel.Services;

// HACK: We need to change this pattern to allow us to use dependency injection to allow adding proprietary CvmConditions from the Acquiring banks or Card Networks
internal abstract record CvmCondition
{
    #region Static Metadata

    private static readonly Dictionary<CvmConditionCode, CvmCondition> _Conditions;

    #endregion

    #region Instance Values

    protected abstract Tag[] _RequiredData { get; }

    #endregion

    #region Constructor

    static CvmCondition()
    {
        AlwaysCondition alwaysCondition = new();
        AmountInApplicationCurrencyAndOverXValueCondition amountInApplicationCurrencyAndOverXValueCondition = new();
        AmountInApplicationCurrencyAndOverYValueCondition amountInApplicationCurrencyAndOverYValueCondition = new();
        AmountInApplicationCurrencyAndUnderXValueCondition amountInApplicationCurrencyAndUnderXValueCondition = new();
        AmountInApplicationCurrencyAndUnderXValueCondition amountInApplicationCurrencyAndUnderYValueCondition = new();
        ManualCashCondition manualCashCondition = new();
        NotUnattendedCashOrManualCashOrPurchaseWithCashback notUnattendedCashOrManualCashOrPurchaseWithCashback = new();
        PurchaseWithCashbackCondition purchaseWithCashbackCondition = new();
        SupportsCvmCondition supportsCvmCondition = new();

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

    public static bool TryGet(CvmConditionCode code, out CvmCondition? result)
    {
        if (!Exists(code))
        {
            result = null;

            return false;
        }

        result = _Conditions[code];

        return true;
    }

    public static bool Exists(CvmConditionCode code) => _Conditions.ContainsKey(code);

    public static bool IsConditionSatisfied(CvmConditionCode code, IQueryTlvDatabase database)
    {
        if (!_Conditions.ContainsKey(code))
            return false;

        if (!_Conditions[code].IsCvmSupported(database))
            return false;

        if (!_Conditions[code].IsRequiredDataPresent(database))
            return false;

        return true;
    }

    public abstract CvmConditionCode GetConditionCode();

    private bool IsCvmSupported(IQueryTlvDatabase database)
    {
        if (!IsRequiredDataPresent(database))
            return false;

        if (!IsConditionSatisfied(database))
            return false;

        return true;
    }

    private bool IsRequiredDataPresent(IQueryTlvDatabase database)
    {
        if (_RequiredData.Length == 0)
            return true;

        for (int i = 0; i < _RequiredData.Length; i++)
        {
            if (!database.IsPresent(_RequiredData[i]))
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

    protected static bool IsUnattended(TerminalType terminalT
               terminalType.IsEnvironmeTerminalType.EnvironmentTypeentType.Unattended);

    protected static bool IsCashback(TransactionType transactionT
               (transactionType == TransactionTypes.CashAdvance) || (transactionType == TransactionTypes.FastCashDebit);

    protected static bool IsManual(PosEntryMode va
               (value == PosEntryModes.ManualEntry) || (value == PosEntryModes.ManualEntryFallback);

    protected static bool IsManualCash(PosEntryMode posEntryMode, TransactionType transactionT
               IsCashback(transactionType) && IsManual(posEntryMode);

    protected static bool IsPurchase(TransactionType transactionType)
    {
        if (transactionType == TransactionTypes.GoodsAndServicesDebit)
            return true;

        if (transactionType == TransactionTypes.GoodsAndServicesWithCashDisbursementDebit)
            return true;

        return false;
    }

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    protected static bool IsCashTransaction(IQueryTlvDatabase database)
    {
        TransactionType transactionType = TransactionType.Decode(database.Get(TransactionType.Tag).EncodeValue().AsSpan());

        if (transactionType == TransactionTypes.CashAdvance)
            return true;

        if (transactionType == TransactionTypes.FastCashDebit)
            return true;

        return false;

    #endregion

    #region Operator Overrideserrides

    public static implicit operator byte(CvmCondition value) => (byte) value.GetConditionCode();
    public static implicit operator CvmConditionCode(CvmCondition value) => value.GetCondition

    #endregion
egion
}