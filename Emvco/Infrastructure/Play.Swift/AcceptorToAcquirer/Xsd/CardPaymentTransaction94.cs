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
public partial class CardPaymentTransaction94
{
    #region Instance Values

    private AuthorisationResult14 authstnRsltField;
    private TransactionVerificationResult4[] txVrfctnRsltField;
    private Product4[] allwdPdctCdField;
    private Product4[] notAllwdPdctCdField;
    private Product5[] addtlAvlblPdctField;
    private AmountAndDirection93 balField;
    private ContentInformationType22 prtctdBalField;
    private Action10[] actnField;
    private CurrencyConversion19 ccyConvsElgbltyField;

    /// <remarks />
    public AuthorisationResult14 AuthstnRslt
    {
        get => this.authstnRsltField;
        set => this.authstnRsltField = value;
    }

    /// <remarks />
    [XmlElement("TxVrfctnRslt")]
    public TransactionVerificationResult4[] TxVrfctnRslt
    {
        get => this.txVrfctnRsltField;
        set => this.txVrfctnRsltField = value;
    }

    /// <remarks />
    [XmlElement("AllwdPdctCd")]
    public Product4[] AllwdPdctCd
    {
        get => this.allwdPdctCdField;
        set => this.allwdPdctCdField = value;
    }

    /// <remarks />
    [XmlElement("NotAllwdPdctCd")]
    public Product4[] NotAllwdPdctCd
    {
        get => this.notAllwdPdctCdField;
        set => this.notAllwdPdctCdField = value;
    }

    /// <remarks />
    [XmlElement("AddtlAvlblPdct")]
    public Product5[] AddtlAvlblPdct
    {
        get => this.addtlAvlblPdctField;
        set => this.addtlAvlblPdctField = value;
    }

    /// <remarks />
    public AmountAndDirection93 Bal
    {
        get => this.balField;
        set => this.balField = value;
    }

    /// <remarks />
    public ContentInformationType22 PrtctdBal
    {
        get => this.prtctdBalField;
        set => this.prtctdBalField = value;
    }

    /// <remarks />
    [XmlElement("Actn")]
    public Action10[] Actn
    {
        get => this.actnField;
        set => this.actnField = value;
    }

    /// <remarks />
    public CurrencyConversion19 CcyConvsElgblty
    {
        get => this.ccyConvsElgbltyField;
        set => this.ccyConvsElgbltyField = value;
    }

    #endregion
}