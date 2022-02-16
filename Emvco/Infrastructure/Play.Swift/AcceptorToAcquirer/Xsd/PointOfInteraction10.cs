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
public partial class PointOfInteraction10
{
    #region Instance Values

    private GenericIdentification177 idField;
    private string sysNmField;
    private string grpIdField;
    private PointOfInteractionCapabilities9 cpbltiesField;
    private string tmZoneField;
    private LocationCategory3Code termnlIntgtnField;
    private bool termnlIntgtnFieldSpecified;
    private PointOfInteractionComponent10[] cmpntField;

    /// <remarks />
    public GenericIdentification177 Id
    {
        get => this.idField;
        set => this.idField = value;
    }

    /// <remarks />
    public string SysNm
    {
        get => this.sysNmField;
        set => this.sysNmField = value;
    }

    /// <remarks />
    public string GrpId
    {
        get => this.grpIdField;
        set => this.grpIdField = value;
    }

    /// <remarks />
    public PointOfInteractionCapabilities9 Cpblties
    {
        get => this.cpbltiesField;
        set => this.cpbltiesField = value;
    }

    /// <remarks />
    public string TmZone
    {
        get => this.tmZoneField;
        set => this.tmZoneField = value;
    }

    /// <remarks />
    public LocationCategory3Code TermnlIntgtn
    {
        get => this.termnlIntgtnField;
        set => this.termnlIntgtnField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool TermnlIntgtnSpecified
    {
        get => this.termnlIntgtnFieldSpecified;
        set => this.termnlIntgtnFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement("Cmpnt")]
    public PointOfInteractionComponent10[] Cmpnt
    {
        get => this.cmpntField;
        set => this.cmpntField = value;
    }

    #endregion
}