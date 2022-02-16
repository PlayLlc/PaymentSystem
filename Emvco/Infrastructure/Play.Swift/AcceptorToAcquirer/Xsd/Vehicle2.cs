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
public partial class Vehicle2
{
    #region Instance Values

    private string tpField;
    private CardDataReading5Code ntryMdField;
    private bool ntryMdFieldSpecified;
    private string dataField;

    /// <remarks />
    public string Tp
    {
        get => this.tpField;
        set => this.tpField = value;
    }

    /// <remarks />
    public CardDataReading5Code NtryMd
    {
        get => this.ntryMdField;
        set => this.ntryMdField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool NtryMdSpecified
    {
        get => this.ntryMdFieldSpecified;
        set => this.ntryMdFieldSpecified = value;
    }

    /// <remarks />
    public string Data
    {
        get => this.dataField;
        set => this.dataField = value;
    }

    #endregion
}