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
public partial class GenericIdentification48
{
    #region Instance Values

    private string idField;
    private string vrsnField;
    private string issrField;

    /// <remarks />
    public string Id
    {
        get => this.idField;
        set => this.idField = value;
    }

    /// <remarks />
    public string Vrsn
    {
        get => this.vrsnField;
        set => this.vrsnField = value;
    }

    /// <remarks />
    public string Issr
    {
        get => this.issrField;
        set => this.issrField = value;
    }

    #endregion
}