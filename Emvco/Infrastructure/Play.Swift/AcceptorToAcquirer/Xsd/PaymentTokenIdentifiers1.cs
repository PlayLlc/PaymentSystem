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
public partial class PaymentTokenIdentifiers1
{
    #region Instance Values

    private string prvdrIdField;
    private string rqstrIdField;

    /// <remarks />
    public string PrvdrId
    {
        get => this.prvdrIdField;
        set => this.prvdrIdField = value;
    }

    /// <remarks />
    public string RqstrId
    {
        get => this.rqstrIdField;
        set => this.rqstrIdField = value;
    }

    #endregion
}