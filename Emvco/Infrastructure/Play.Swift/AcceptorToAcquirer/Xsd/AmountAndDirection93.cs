using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

/// <remarks />
[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:iso:std:iso:20022:tech:xsd:caaa.002.001.09")]
public partial class AmountAndDirection93
{
    #region Instance Values

    private decimal amtField;
    private string ccyField;
    private bool sgnField;
    private bool sgnFieldSpecified;

    /// <remarks />
    public decimal Amt
    {
        get => this.amtField;
        set => this.amtField = value;
    }

    /// <remarks />
    public string Ccy
    {
        get => this.ccyField;
        set => this.ccyField = value;
    }

    /// <remarks />
    public bool Sgn
    {
        get => this.sgnField;
        set => this.sgnField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool SgnSpecified
    {
        get => this.sgnFieldSpecified;
        set => this.sgnFieldSpecified = value;
    }

    #endregion
}