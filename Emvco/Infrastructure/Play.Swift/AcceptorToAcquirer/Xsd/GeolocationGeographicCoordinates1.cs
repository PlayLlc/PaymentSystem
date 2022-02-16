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
public partial class GeolocationGeographicCoordinates1
{
    #region Instance Values

    private string latField;
    private string longField;

    /// <remarks />
    public string Lat
    {
        get => this.latField;
        set => this.latField = value;
    }

    /// <remarks />
    public string Long
    {
        get => this.longField;
        set => this.longField = value;
    }

    #endregion
}