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
public partial class GenericInformation1
{
    #region Instance Values

    private string nmField;
    private string valField;

    /// <remarks />
    public string Nm
    {
        get => this.nmField;
        set => this.nmField = value;
    }

    /// <remarks />
    public string Val
    {
        get => this.valField;
        set => this.valField = value;
    }

    #endregion
}