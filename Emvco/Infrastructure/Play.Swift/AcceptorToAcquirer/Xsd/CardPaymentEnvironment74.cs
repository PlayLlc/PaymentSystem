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
public partial class CardPaymentEnvironment74
{
    #region Instance Values

    private Acquirer10 acqrrField;
    private Organisation32 mrchntField;
    private PointOfInteraction10 pOIField;
    private PaymentCard30 cardField;
    private CustomerDevice1 cstmrDvcField;
    private CustomerDevice1 wlltField;
    private CardPaymentToken5 pmtTknField;
    private Cardholder16 crdhldrField;
    private ContentInformationType22 prtctdCrdhldrDataField;

    /// <remarks />
    public Acquirer10 Acqrr
    {
        get => this.acqrrField;
        set => this.acqrrField = value;
    }

    /// <remarks />
    public Organisation32 Mrchnt
    {
        get => this.mrchntField;
        set => this.mrchntField = value;
    }

    /// <remarks />
    public PointOfInteraction10 POI
    {
        get => this.pOIField;
        set => this.pOIField = value;
    }

    /// <remarks />
    public PaymentCard30 Card
    {
        get => this.cardField;
        set => this.cardField = value;
    }

    /// <remarks />
    public CustomerDevice1 CstmrDvc
    {
        get => this.cstmrDvcField;
        set => this.cstmrDvcField = value;
    }

    /// <remarks />
    public CustomerDevice1 Wllt
    {
        get => this.wlltField;
        set => this.wlltField = value;
    }

    /// <remarks />
    public CardPaymentToken5 PmtTkn
    {
        get => this.pmtTknField;
        set => this.pmtTknField = value;
    }

    /// <remarks />
    public Cardholder16 Crdhldr
    {
        get => this.crdhldrField;
        set => this.crdhldrField = value;
    }

    /// <remarks />
    public ContentInformationType22 PrtctdCrdhldrData
    {
        get => this.prtctdCrdhldrDataField;
        set => this.prtctdCrdhldrDataField = value;
    }

    #endregion
}