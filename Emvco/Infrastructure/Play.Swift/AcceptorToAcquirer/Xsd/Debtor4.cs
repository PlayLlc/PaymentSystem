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
public partial class Debtor4
{
    #region Instance Values

    private PartyIdentification178Choice dbtrField;
    private CashAccountIdentification7Choice acctIdField;

    /// <remarks />
    public PartyIdentification178Choice Dbtr
    {
        get => this.dbtrField;
        set => this.dbtrField = value;
    }

    /// <remarks />
    public CashAccountIdentification7Choice AcctId
    {
        get => this.acctIdField;
        set => this.acctIdField = value;
    }

    #endregion
}