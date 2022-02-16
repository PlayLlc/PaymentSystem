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
public partial class PointOfInteractionComponentAssessment1
{
    #region Instance Values

    private POIComponentAssessment1Code tpField;
    private string[] assgnrField;
    private DateTime dlvryDtField;
    private bool dlvryDtFieldSpecified;
    private DateTime xprtnDtField;
    private bool xprtnDtFieldSpecified;
    private string nbField;

    /// <remarks />
    public POIComponentAssessment1Code Tp
    {
        get => this.tpField;
        set => this.tpField = value;
    }

    /// <remarks />
    [XmlElement("Assgnr")]
    public string[] Assgnr
    {
        get => this.assgnrField;
        set => this.assgnrField = value;
    }

    /// <remarks />
    public DateTime DlvryDt
    {
        get => this.dlvryDtField;
        set => this.dlvryDtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool DlvryDtSpecified
    {
        get => this.dlvryDtFieldSpecified;
        set => this.dlvryDtFieldSpecified = value;
    }

    /// <remarks />
    public DateTime XprtnDt
    {
        get => this.xprtnDtField;
        set => this.xprtnDtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool XprtnDtSpecified
    {
        get => this.xprtnDtFieldSpecified;
        set => this.xprtnDtFieldSpecified = value;
    }

    /// <remarks />
    public string Nb
    {
        get => this.nbField;
        set => this.nbField = value;
    }

    #endregion
}