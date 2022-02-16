using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

/// <remarks />
[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:iso:std:iso:20022:tech:xsd:caaa.001.001.09")]
public partial class CardPaymentTransaction92
{
    #region Instance Values

    private bool txCaptrField;
    private CardPaymentServiceType12Code txTpField;
    private bool txTpFieldSpecified;
    private CardPaymentServiceType9Code[] addtlSvcField;
    private CardPaymentServiceType3Code svcAttrField;
    private bool svcAttrFieldSpecified;
    private bool lastTxFlgField;
    private bool lastTxFlgFieldSpecified;
    private string mrchntCtgyCdField;
    private bool[] cstmrCnsntField;
    private string[] cardPrgrmmPropsdField;
    private string cardPrgrmmApldField;
    private string saleRefIdField;
    private TransactionIdentifier1 txIdField;
    private CardPaymentTransaction101 orgnlTxField;
    private string initrTxIdField;
    private string rcncltnIdField;
    private CardPaymentTransactionDetails48 txDtlsField;
    private string mrchntRefDataField;
    private CardAccount15 acctFrField;
    private CardAccount15 acctToField;
    private string[] addtlTxDataField;

    /// <remarks />
    public bool TxCaptr
    {
        get => this.txCaptrField;
        set => this.txCaptrField = value;
    }

    /// <remarks />
    public CardPaymentServiceType12Code TxTp
    {
        get => this.txTpField;
        set => this.txTpField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool TxTpSpecified
    {
        get => this.txTpFieldSpecified;
        set => this.txTpFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement("AddtlSvc")]
    public CardPaymentServiceType9Code[] AddtlSvc
    {
        get => this.addtlSvcField;
        set => this.addtlSvcField = value;
    }

    /// <remarks />
    public CardPaymentServiceType3Code SvcAttr
    {
        get => this.svcAttrField;
        set => this.svcAttrField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool SvcAttrSpecified
    {
        get => this.svcAttrFieldSpecified;
        set => this.svcAttrFieldSpecified = value;
    }

    /// <remarks />
    public bool LastTxFlg
    {
        get => this.lastTxFlgField;
        set => this.lastTxFlgField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool LastTxFlgSpecified
    {
        get => this.lastTxFlgFieldSpecified;
        set => this.lastTxFlgFieldSpecified = value;
    }

    /// <remarks />
    public string MrchntCtgyCd
    {
        get => this.mrchntCtgyCdField;
        set => this.mrchntCtgyCdField = value;
    }

    /// <remarks />
    [XmlElement("CstmrCnsnt")]
    public bool[] CstmrCnsnt
    {
        get => this.cstmrCnsntField;
        set => this.cstmrCnsntField = value;
    }

    /// <remarks />
    [XmlElement("CardPrgrmmPropsd")]
    public string[] CardPrgrmmPropsd
    {
        get => this.cardPrgrmmPropsdField;
        set => this.cardPrgrmmPropsdField = value;
    }

    /// <remarks />
    public string CardPrgrmmApld
    {
        get => this.cardPrgrmmApldField;
        set => this.cardPrgrmmApldField = value;
    }

    /// <remarks />
    public string SaleRefId
    {
        get => this.saleRefIdField;
        set => this.saleRefIdField = value;
    }

    /// <remarks />
    public TransactionIdentifier1 TxId
    {
        get => this.txIdField;
        set => this.txIdField = value;
    }

    /// <remarks />
    public CardPaymentTransaction101 OrgnlTx
    {
        get => this.orgnlTxField;
        set => this.orgnlTxField = value;
    }

    /// <remarks />
    public string InitrTxId
    {
        get => this.initrTxIdField;
        set => this.initrTxIdField = value;
    }

    /// <remarks />
    public string RcncltnId
    {
        get => this.rcncltnIdField;
        set => this.rcncltnIdField = value;
    }

    /// <remarks />
    public CardPaymentTransactionDetails48 TxDtls
    {
        get => this.txDtlsField;
        set => this.txDtlsField = value;
    }

    /// <remarks />
    public string MrchntRefData
    {
        get => this.mrchntRefDataField;
        set => this.mrchntRefDataField = value;
    }

    /// <remarks />
    public CardAccount15 AcctFr
    {
        get => this.acctFrField;
        set => this.acctFrField = value;
    }

    /// <remarks />
    public CardAccount15 AcctTo
    {
        get => this.acctToField;
        set => this.acctToField = value;
    }

    /// <remarks />
    [XmlElement("AddtlTxData")]
    public string[] AddtlTxData
    {
        get => this.addtlTxDataField;
        set => this.addtlTxDataField = value;
    }

    #endregion
}