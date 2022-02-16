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
public partial class Geolocation1
{
    #region Instance Values

    private GeolocationGeographicCoordinates1 geogcCordintsField;
    private GeolocationUTMCoordinates1 uTMCordintsField;

    /// <remarks />
    public GeolocationGeographicCoordinates1 GeogcCordints
    {
        get => this.geogcCordintsField;
        set => this.geogcCordintsField = value;
    }

    /// <remarks />
    public GeolocationUTMCoordinates1 UTMCordints
    {
        get => this.uTMCordintsField;
        set => this.uTMCordintsField = value;
    }

    #endregion
}