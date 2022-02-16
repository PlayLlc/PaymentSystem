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
public partial class CardPaymentContext28
{
    #region Instance Values

    private PaymentContext27 pmtCntxtField;
    private SaleContext4 saleCntxtField;
    private CardDirectDebit2 drctDbtCntxtField;

    /// <remarks />
    public PaymentContext27 PmtCntxt
    {
        get => this.pmtCntxtField;
        set => this.pmtCntxtField = value;
    }

    /// <remarks />
    public SaleContext4 SaleCntxt
    {
        get => this.saleCntxtField;
        set => this.saleCntxtField = value;
    }

    /// <remarks />
    public CardDirectDebit2 DrctDbtCntxt
    {
        get => this.drctDbtCntxtField;
        set => this.drctDbtCntxtField = value;
    }

    #endregion
}