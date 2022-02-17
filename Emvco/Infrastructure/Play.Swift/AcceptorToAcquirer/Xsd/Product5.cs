using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

/// <remarks />
[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:iso:std:iso:20022:tech:xsd:caaa.002.001.09")]
public partial class Product5
{
    #region Instance Values

    private string pdctCdField;
    private string addtlPdctCdField;
    private decimal amtLmtField;
    private bool amtLmtFieldSpecified;
    private decimal qtyLmtField;
    private bool qtyLmtFieldSpecified;
    private UnitOfMeasure6Code unitOfMeasrField;
    private bool unitOfMeasrFieldSpecified;

    /// <remarks />
    public string PdctCd
    {
        get => this.pdctCdField;
        set => this.pdctCdField = value;
    }

    /// <remarks />
    public string AddtlPdctCd
    {
        get => this.addtlPdctCdField;
        set => this.addtlPdctCdField = value;
    }

    /// <remarks />
    public decimal AmtLmt
    {
        get => this.amtLmtField;
        set => this.amtLmtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool AmtLmtSpecified
    {
        get => this.amtLmtFieldSpecified;
        set => this.amtLmtFieldSpecified = value;
    }

    /// <remarks />
    public decimal QtyLmt
    {
        get => this.qtyLmtField;
        set => this.qtyLmtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool QtyLmtSpecified
    {
        get => this.qtyLmtFieldSpecified;
        set => this.qtyLmtFieldSpecified = value;
    }

    /// <remarks />
    public UnitOfMeasure6Code UnitOfMeasr
    {
        get => this.unitOfMeasrField;
        set => this.unitOfMeasrField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool UnitOfMeasrSpecified
    {
        get => this.unitOfMeasrFieldSpecified;
        set => this.unitOfMeasrFieldSpecified = value;
    }

    #endregion
}