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
public partial class Creditor4
{
    #region Instance Values

    private PartyIdentification178Choice cdtrField;
    private string regnIdField;

    /// <remarks />
    public PartyIdentification178Choice Cdtr
    {
        get => this.cdtrField;
        set => this.cdtrField = value;
    }

    /// <remarks />
    public string RegnId
    {
        get => this.regnIdField;
        set => this.regnIdField = value;
    }

    #endregion
}