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
public partial class PackageType1
{
    #region Instance Values

    private GenericIdentification176 packgIdField;
    private decimal packgLngthField;
    private bool packgLngthFieldSpecified;
    private decimal offsetStartField;
    private bool offsetStartFieldSpecified;
    private decimal offsetEndField;
    private bool offsetEndFieldSpecified;
    private ExternallyDefinedData1[] packgBlckField;

    /// <remarks />
    public GenericIdentification176 PackgId
    {
        get => this.packgIdField;
        set => this.packgIdField = value;
    }

    /// <remarks />
    public decimal PackgLngth
    {
        get => this.packgLngthField;
        set => this.packgLngthField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool PackgLngthSpecified
    {
        get => this.packgLngthFieldSpecified;
        set => this.packgLngthFieldSpecified = value;
    }

    /// <remarks />
    public decimal OffsetStart
    {
        get => this.offsetStartField;
        set => this.offsetStartField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool OffsetStartSpecified
    {
        get => this.offsetStartFieldSpecified;
        set => this.offsetStartFieldSpecified = value;
    }

    /// <remarks />
    public decimal OffsetEnd
    {
        get => this.offsetEndField;
        set => this.offsetEndField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool OffsetEndSpecified
    {
        get => this.offsetEndFieldSpecified;
        set => this.offsetEndFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement("PackgBlck")]
    public ExternallyDefinedData1[] PackgBlck
    {
        get => this.packgBlckField;
        set => this.packgBlckField = value;
    }

    #endregion
}