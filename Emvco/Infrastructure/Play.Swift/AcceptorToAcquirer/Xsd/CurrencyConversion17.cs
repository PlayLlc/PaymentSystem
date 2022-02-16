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
public partial class CurrencyConversion17
{
    #region Instance Values

    private bool accptdByCrdhldrField;
    private bool accptdByCrdhldrFieldSpecified;
    private CurrencyConversion19 convsField;

    /// <remarks />
    public bool AccptdByCrdhldr
    {
        get => this.accptdByCrdhldrField;
        set => this.accptdByCrdhldrField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool AccptdByCrdhldrSpecified
    {
        get => this.accptdByCrdhldrFieldSpecified;
        set => this.accptdByCrdhldrFieldSpecified = value;
    }

    /// <remarks />
    public CurrencyConversion19 Convs
    {
        get => this.convsField;
        set => this.convsField = value;
    }

    #endregion
}