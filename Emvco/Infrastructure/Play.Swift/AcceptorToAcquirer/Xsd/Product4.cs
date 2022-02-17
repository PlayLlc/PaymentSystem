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
public partial class Product4
{
    #region Instance Values

    private string pdctCdField;
    private string addtlPdctCdField;

    /// <remarks />
    public string PdctCd
    {
        get => this.pdctCdField;
        set => this.pdctCdField = value;
    }

    /// <remarks />
    public string AddtlPdctCd
    {
        get => this.addtlPdctCdField;
        set => this.addtlPdctCdField = value;
    }

    #endregion
}