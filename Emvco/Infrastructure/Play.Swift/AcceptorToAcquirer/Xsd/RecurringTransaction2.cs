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
public partial class RecurringTransaction2
{
    #region Instance Values

    private InstalmentPlan1Code[] instlmtPlanField;
    private string planIdField;
    private decimal seqNbField;
    private bool seqNbFieldSpecified;
    private Frequency3Code prdUnitField;
    private bool prdUnitFieldSpecified;
    private decimal instlmtPrdField;
    private bool instlmtPrdFieldSpecified;
    private decimal ttlNbOfPmtsField;
    private bool ttlNbOfPmtsFieldSpecified;
    private DateTime frstPmtDtField;
    private bool frstPmtDtFieldSpecified;
    private CurrencyAndAmount ttlAmtField;
    private decimal frstAmtField;
    private bool frstAmtFieldSpecified;
    private decimal chrgsField;
    private bool chrgsFieldSpecified;

    /// <remarks />
    [XmlElement("InstlmtPlan")]
    public InstalmentPlan1Code[] InstlmtPlan
    {
        get => this.instlmtPlanField;
        set => this.instlmtPlanField = value;
    }

    /// <remarks />
    public string PlanId
    {
        get => this.planIdField;
        set => this.planIdField = value;
    }

    /// <remarks />
    public decimal SeqNb
    {
        get => this.seqNbField;
        set => this.seqNbField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool SeqNbSpecified
    {
        get => this.seqNbFieldSpecified;
        set => this.seqNbFieldSpecified = value;
    }

    /// <remarks />
    public Frequency3Code PrdUnit
    {
        get => this.prdUnitField;
        set => this.prdUnitField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool PrdUnitSpecified
    {
        get => this.prdUnitFieldSpecified;
        set => this.prdUnitFieldSpecified = value;
    }

    /// <remarks />
    public decimal InstlmtPrd
    {
        get => this.instlmtPrdField;
        set => this.instlmtPrdField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool InstlmtPrdSpecified
    {
        get => this.instlmtPrdFieldSpecified;
        set => this.instlmtPrdFieldSpecified = value;
    }

    /// <remarks />
    public decimal TtlNbOfPmts
    {
        get => this.ttlNbOfPmtsField;
        set => this.ttlNbOfPmtsField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool TtlNbOfPmtsSpecified
    {
        get => this.ttlNbOfPmtsFieldSpecified;
        set => this.ttlNbOfPmtsFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement(DataType = "date")]
    public DateTime FrstPmtDt
    {
        get => this.frstPmtDtField;
        set => this.frstPmtDtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool FrstPmtDtSpecified
    {
        get => this.frstPmtDtFieldSpecified;
        set => this.frstPmtDtFieldSpecified = value;
    }

    /// <remarks />
    public CurrencyAndAmount TtlAmt
    {
        get => this.ttlAmtField;
        set => this.ttlAmtField = value;
    }

    /// <remarks />
    public decimal FrstAmt
    {
        get => this.frstAmtField;
        set => this.frstAmtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool FrstAmtSpecified
    {
        get => this.frstAmtFieldSpecified;
        set => this.frstAmtFieldSpecified = value;
    }

    /// <remarks />
    public decimal Chrgs
    {
        get => this.chrgsField;
        set => this.chrgsField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool ChrgsSpecified
    {
        get => this.chrgsFieldSpecified;
        set => this.chrgsFieldSpecified = value;
    }

    #endregion
}