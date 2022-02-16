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
public partial class PointOfInteractionComponentCharacteristics6
{
    #region Instance Values

    private MemoryCharacteristics1[] mmryField;
    private CommunicationCharacteristics5[] comField;
    private decimal sctyAccsMdlsField;
    private bool sctyAccsMdlsFieldSpecified;
    private decimal sbcbrIdntyMdlsField;
    private bool sbcbrIdntyMdlsFieldSpecified;
    private CryptographicKey14[] sctyElmtField;

    /// <remarks />
    [XmlElement("Mmry")]
    public MemoryCharacteristics1[] Mmry
    {
        get => this.mmryField;
        set => this.mmryField = value;
    }

    /// <remarks />
    [XmlElement("Com")]
    public CommunicationCharacteristics5[] Com
    {
        get => this.comField;
        set => this.comField = value;
    }

    /// <remarks />
    public decimal SctyAccsMdls
    {
        get => this.sctyAccsMdlsField;
        set => this.sctyAccsMdlsField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool SctyAccsMdlsSpecified
    {
        get => this.sctyAccsMdlsFieldSpecified;
        set => this.sctyAccsMdlsFieldSpecified = value;
    }

    /// <remarks />
    public decimal SbcbrIdntyMdls
    {
        get => this.sbcbrIdntyMdlsField;
        set => this.sbcbrIdntyMdlsField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool SbcbrIdntyMdlsSpecified
    {
        get => this.sbcbrIdntyMdlsFieldSpecified;
        set => this.sbcbrIdntyMdlsFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement("SctyElmt")]
    public CryptographicKey14[] SctyElmt
    {
        get => this.sctyElmtField;
        set => this.sctyElmtField = value;
    }

    #endregion
}