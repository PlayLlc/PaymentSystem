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
public partial class CurrencyAndAmount
{
    #region Instance Values

    private string ccyField;
    private decimal valueField;

    /// <remarks />
    [XmlAttribute()]
    public string Ccy
    {
        get => this.ccyField;
        set => this.ccyField = value;
    }

    /// <remarks />
    [XmlText()]
    public decimal Value
    {
        get => this.valueField;
        set => this.valueField = value;
    }

    #endregion
}