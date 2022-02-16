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
public partial class CustomerDevice1
{
    #region Instance Values

    private string idField;
    private string tpField;
    private string prvdrField;

    /// <remarks />
    public string Id
    {
        get => this.idField;
        set => this.idField = value;
    }

    /// <remarks />
    public string Tp
    {
        get => this.tpField;
        set => this.tpField = value;
    }

    /// <remarks />
    public string Prvdr
    {
        get => this.prvdrField;
        set => this.prvdrField = value;
    }

    #endregion
}