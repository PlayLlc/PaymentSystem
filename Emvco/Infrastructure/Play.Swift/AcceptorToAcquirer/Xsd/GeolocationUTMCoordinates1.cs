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
public partial class GeolocationUTMCoordinates1
{
    #region Instance Values

    private string uTMZoneField;
    private string uTMEstwrdField;
    private string uTMNrthwrdField;

    /// <remarks />
    public string UTMZone
    {
        get => this.uTMZoneField;
        set => this.uTMZoneField = value;
    }

    /// <remarks />
    public string UTMEstwrd
    {
        get => this.uTMEstwrdField;
        set => this.uTMEstwrdField = value;
    }

    /// <remarks />
    public string UTMNrthwrd
    {
        get => this.uTMNrthwrdField;
        set => this.uTMNrthwrdField = value;
    }

    #endregion
}