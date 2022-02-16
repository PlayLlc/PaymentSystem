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
public partial class AggregationTransaction3
{
    #region Instance Values

    private DateTime frstPmtDtTmField;
    private bool frstPmtDtTmFieldSpecified;
    private DateTime lastPmtDtTmField;
    private bool lastPmtDtTmFieldSpecified;
    private decimal nbOfPmtsField;
    private bool nbOfPmtsFieldSpecified;
    private DetailedAmount21[] indvPmtField;

    /// <remarks />
    public DateTime FrstPmtDtTm
    {
        get => this.frstPmtDtTmField;
        set => this.frstPmtDtTmField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool FrstPmtDtTmSpecified
    {
        get => this.frstPmtDtTmFieldSpecified;
        set => this.frstPmtDtTmFieldSpecified = value;
    }

    /// <remarks />
    public DateTime LastPmtDtTm
    {
        get => this.lastPmtDtTmField;
        set => this.lastPmtDtTmField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool LastPmtDtTmSpecified
    {
        get => this.lastPmtDtTmFieldSpecified;
        set => this.lastPmtDtTmFieldSpecified = value;
    }

    /// <remarks />
    public decimal NbOfPmts
    {
        get => this.nbOfPmtsField;
        set => this.nbOfPmtsField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool NbOfPmtsSpecified
    {
        get => this.nbOfPmtsFieldSpecified;
        set => this.nbOfPmtsFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement("IndvPmt")]
    public DetailedAmount21[] IndvPmt
    {
        get => this.indvPmtField;
        set => this.indvPmtField = value;
    }

    #endregion
}