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
public partial class PointOfInteractionCapabilities9
{
    #region Instance Values

    private CardDataReading8Code[] cardRdngCpbltiesField;
    private CardholderVerificationCapability4Code[] crdhldrVrfctnCpbltiesField;
    private decimal pINLngthCpbltiesField;
    private bool pINLngthCpbltiesFieldSpecified;
    private decimal apprvlCdLngthField;
    private bool apprvlCdLngthFieldSpecified;
    private decimal mxScrptLngthField;
    private bool mxScrptLngthFieldSpecified;
    private bool cardCaptrCpblField;
    private bool cardCaptrCpblFieldSpecified;
    private OnLineCapability1Code onLineCpbltiesField;
    private bool onLineCpbltiesFieldSpecified;
    private DisplayCapabilities4[] msgCpbltiesField;

    /// <remarks />
    [XmlElement("CardRdngCpblties")]
    public CardDataReading8Code[] CardRdngCpblties
    {
        get => this.cardRdngCpbltiesField;
        set => this.cardRdngCpbltiesField = value;
    }

    /// <remarks />
    [XmlElement("CrdhldrVrfctnCpblties")]
    public CardholderVerificationCapability4Code[] CrdhldrVrfctnCpblties
    {
        get => this.crdhldrVrfctnCpbltiesField;
        set => this.crdhldrVrfctnCpbltiesField = value;
    }

    /// <remarks />
    public decimal PINLngthCpblties
    {
        get => this.pINLngthCpbltiesField;
        set => this.pINLngthCpbltiesField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool PINLngthCpbltiesSpecified
    {
        get => this.pINLngthCpbltiesFieldSpecified;
        set => this.pINLngthCpbltiesFieldSpecified = value;
    }

    /// <remarks />
    public decimal ApprvlCdLngth
    {
        get => this.apprvlCdLngthField;
        set => this.apprvlCdLngthField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool ApprvlCdLngthSpecified
    {
        get => this.apprvlCdLngthFieldSpecified;
        set => this.apprvlCdLngthFieldSpecified = value;
    }

    /// <remarks />
    public decimal MxScrptLngth
    {
        get => this.mxScrptLngthField;
        set => this.mxScrptLngthField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool MxScrptLngthSpecified
    {
        get => this.mxScrptLngthFieldSpecified;
        set => this.mxScrptLngthFieldSpecified = value;
    }

    /// <remarks />
    public bool CardCaptrCpbl
    {
        get => this.cardCaptrCpblField;
        set => this.cardCaptrCpblField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool CardCaptrCpblSpecified
    {
        get => this.cardCaptrCpblFieldSpecified;
        set => this.cardCaptrCpblFieldSpecified = value;
    }

    /// <remarks />
    public OnLineCapability1Code OnLineCpblties
    {
        get => this.onLineCpbltiesField;
        set => this.onLineCpbltiesField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool OnLineCpbltiesSpecified
    {
        get => this.onLineCpbltiesFieldSpecified;
        set => this.onLineCpbltiesFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement("MsgCpblties")]
    public DisplayCapabilities4[] MsgCpblties
    {
        get => this.msgCpbltiesField;
        set => this.msgCpbltiesField = value;
    }

    #endregion
}