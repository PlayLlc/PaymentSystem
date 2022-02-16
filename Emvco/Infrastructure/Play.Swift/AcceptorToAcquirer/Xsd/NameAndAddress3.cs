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
public partial class NameAndAddress3
{
    #region Instance Values

    private string nmField;
    private PostalAddress1 adrField;

    /// <remarks />
    public string Nm
    {
        get => this.nmField;
        set => this.nmField = value;
    }

    /// <remarks />
    public PostalAddress1 Adr
    {
        get => this.adrField;
        set => this.adrField = value;
    }

    #endregion
}