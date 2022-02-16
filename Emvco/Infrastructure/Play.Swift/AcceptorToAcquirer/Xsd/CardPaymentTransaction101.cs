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
public partial class CardPaymentTransaction101
{
    #region Instance Values

    private string saleRefIdField;
    private TransactionIdentifier1 txIdField;
    private GenericIdentification32 pOIIdField;
    private string initrTxIdField;
    private string rcptTxIdField;
    private CardPaymentServiceType12Code txTpField;
    private CardPaymentServiceType9Code[] addtlSvcField;
    private CardPaymentServiceType3Code svcAttrField;
    private bool svcAttrFieldSpecified;
    private CardDataReading8Code cardDataNtryMdField;
    private bool cardDataNtryMdFieldSpecified;
    private CardPaymentTransactionResult3 txRsltField;

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
    public GenericIdentification32 POIId
    {
        get => this.pOIIdField;
        set => this.pOIIdField = value;
    }

    /// <remarks />
    public string InitrTxId
    {
        get => this.initrTxIdField;
        set => this.initrTxIdField = value;
    }

    /// <remarks />
    public string RcptTxId
    {
        get => this.rcptTxIdField;
        set => this.rcptTxIdField = value;
    }

    /// <remarks />
    public CardPaymentServiceType12Code TxTp
    {
        get => this.txTpField;
        set => this.txTpField = value;
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
    public CardDataReading8Code CardDataNtryMd
    {
        get => this.cardDataNtryMdField;
        set => this.cardDataNtryMdField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool CardDataNtryMdSpecified
    {
        get => this.cardDataNtryMdFieldSpecified;
        set => this.cardDataNtryMdFieldSpecified = value;
    }

    /// <remarks />
    public CardPaymentTransactionResult3 TxRslt
    {
        get => this.txRsltField;
        set => this.txRsltField = value;
    }

    #endregion
}