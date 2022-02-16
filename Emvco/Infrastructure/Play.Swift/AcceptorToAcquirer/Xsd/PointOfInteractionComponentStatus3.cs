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
public partial class PointOfInteractionComponentStatus3
{
    #region Instance Values

    private string vrsnNbField;
    private POIComponentStatus1Code stsField;
    private bool stsFieldSpecified;
    private DateTime xpryDtField;
    private bool xpryDtFieldSpecified;

    /// <remarks />
    public string VrsnNb
    {
        get => this.vrsnNbField;
        set => this.vrsnNbField = value;
    }

    /// <remarks />
    public POIComponentStatus1Code Sts
    {
        get => this.stsField;
        set => this.stsField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool StsSpecified
    {
        get => this.stsFieldSpecified;
        set => this.stsFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement(DataType = "date")]
    public DateTime XpryDt
    {
        get => this.xpryDtField;
        set => this.xpryDtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool XpryDtSpecified
    {
        get => this.xpryDtFieldSpecified;
        set => this.xpryDtFieldSpecified = value;
    }

    #endregion
}