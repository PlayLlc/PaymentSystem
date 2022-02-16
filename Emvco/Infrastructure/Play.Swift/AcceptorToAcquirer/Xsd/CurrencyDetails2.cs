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
public partial class CurrencyDetails2
{
    #region Instance Values

    private string alphaCdField;
    private string nmrcCdField;
    private decimal dcmlField;
    private bool dcmlFieldSpecified;
    private string nmField;

    /// <remarks />
    public string AlphaCd
    {
        get => this.alphaCdField;
        set => this.alphaCdField = value;
    }

    /// <remarks />
    public string NmrcCd
    {
        get => this.nmrcCdField;
        set => this.nmrcCdField = value;
    }

    /// <remarks />
    public decimal Dcml
    {
        get => this.dcmlField;
        set => this.dcmlField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool DcmlSpecified
    {
        get => this.dcmlFieldSpecified;
        set => this.dcmlFieldSpecified = value;
    }

    /// <remarks />
    public string Nm
    {
        get => this.nmField;
        set => this.nmField = value;
    }

    #endregion
}