namespace Play.Emv.DataElements.Emv.ValueTypes;

public readonly struct CvmConditionCodes
{
    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public CvmConditionCodes(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public bool Always() => _Value == 0;
    public bool IfUnattendedCash() => _Value == 0x01;
    public bool IfNotUnattendedCashAndNotManualCashAndNotPurchaseWithCashback() => _Value == 0x02;
    public bool IfTerminalSupportsCvm() => _Value == 0x03;
    public bool IfManualCash() => _Value == 0x04;
    public bool IfPurchaseWithCashback() => _Value == 0x05;
    public bool IfTransactionIsInTheApplicationCurrencyAndIsUnderXValue() => _Value == 0x06;
    public bool IfTransactionIsInTheApplicationCurrencyAndIsOverXValue() => _Value == 0x07;
    public bool IfTransactionIsInTheApplicationCurrencyAndIsUnderYValue() => _Value == 0x08;
    public bool IfTransactionIsInTheApplicationCurrencyAndIsOverYValue() => _Value == 0x09;

    #endregion

    #region Operator Overrides

    public static explicit operator byte(CvmConditionCodes value) => value._Value;

    #endregion
}