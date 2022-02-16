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
public partial class CardDirectDebit2
{
    #region Instance Values

    private Debtor4 dbtrIdField;
    private Creditor4 cdtrIdField;
    private MandateRelatedInformation13 mndtRltdInfField;

    /// <remarks />
    public Debtor4 DbtrId
    {
        get => this.dbtrIdField;
        set => this.dbtrIdField = value;
    }

    /// <remarks />
    public Creditor4 CdtrId
    {
        get => this.cdtrIdField;
        set => this.cdtrIdField = value;
    }

    /// <remarks />
    public MandateRelatedInformation13 MndtRltdInf
    {
        get => this.mndtRltdInfField;
        set => this.mndtRltdInfField = value;
    }

    #endregion
}