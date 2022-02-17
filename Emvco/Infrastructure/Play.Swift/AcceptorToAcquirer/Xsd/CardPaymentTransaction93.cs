using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

/// <remarks />
[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:iso:std:iso:20022:tech:xsd:caaa.002.001.09")]
public partial class CardPaymentTransaction93
{
    #region Instance Values

    private string saleRefIdField;
    private TransactionIdentifier1 txIdField;
    private string initrTxIdField;
    private string rcptTxIdField;
    private string rcncltnIdField;
    private string intrchngDataField;
    private CardPaymentTransactionDetails48 txDtlsField;
    private string mrchntRefDataField;

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
    public string RcncltnId
    {
        get => this.rcncltnIdField;
        set => this.rcncltnIdField = value;
    }

    /// <remarks />
    public string IntrchngData
    {
        get => this.intrchngDataField;
        set => this.intrchngDataField = value;
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

    #endregion
}