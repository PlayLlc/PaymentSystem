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
public partial class TransactionIdentifier1
{
    #region Instance Values

    private DateTime txDtTmField;
    private string txRefField;

    /// <remarks />
    public DateTime TxDtTm
    {
        get => this.txDtTmField;
        set => this.txDtTmField = value;
    }

    /// <remarks />
    public string TxRef
    {
        get => this.txRefField;
        set => this.txRefField = value;
    }

    #endregion
}