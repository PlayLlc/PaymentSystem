using System.Collections.Generic;

using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services.Selection.CvmConditions;

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

    /// <summary>
    /// IsCvmSupported
    /// </summary>
    /// <param name="database"></param>
    /// <param name="code"></param>
    /// <param name="xAmount"></param>
    /// <param name="yAmount"></param>
    /// <returns></returns>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    public static bool IsCvmSupported(KernelDatabase database, CvmConditionCode code, Money xAmount, Money yAmount)
    {
        if (!_Conditions.ContainsKey(code))
            return false;

        if (!_Conditions[code].IsRequiredDataPresent(database))
            return false;

        if (!_Conditions[code].IsConditionSatisfied(database, xAmount, yAmount))
            return false;

        return true;
    }

    public abstract CvmConditionCode GetConditionCode();

    /// <summary>
    /// IsRequiredDataPresent
    /// </summary>
    /// <param name="database"></param>
    /// <returns></returns>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    private bool IsRequiredDataPresent(IReadTlvDatabase database)
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

    protected abstract bool IsConditionSatisfied(KernelDatabase database, Money xAmount, Money yAmount);

    #endregion

    #region Operator Overrides

    public static implicit operator byte(CvmCondition value) => (byte) value.GetConditionCode();
    public static implicit operator CvmConditionCode(CvmCondition value) => value.GetConditionCode();

    #endregion
}