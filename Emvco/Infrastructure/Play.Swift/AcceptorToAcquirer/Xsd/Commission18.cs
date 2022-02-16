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
public partial class Commission18
{
    #region Instance Values

    private decimal rateField;
    private string addtlInfField;

    /// <remarks />
    public decimal Rate
    {
        get => this.rateField;
        set => this.rateField = value;
    }

    /// <remarks />
    public string AddtlInf
    {
        get => this.addtlInfField;
        set => this.addtlInfField = value;
    }

    #endregion
}