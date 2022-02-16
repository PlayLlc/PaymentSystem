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
public partial class Product6
{
    #region Instance Values

    private string itmIdField;
    private string pdctCdField;
    private string addtlPdctCdField;
    private UnitOfMeasure6Code unitOfMeasrField;
    private bool unitOfMeasrFieldSpecified;
    private decimal pdctQtyField;
    private bool pdctQtyFieldSpecified;
    private decimal unitPricField;
    private bool unitPricFieldSpecified;
    private bool unitPricSgnField;
    private bool unitPricSgnFieldSpecified;
    private decimal pdctAmtField;
    private bool pdctAmtSgnField;
    private bool pdctAmtSgnFieldSpecified;
    private decimal valAddedTaxField;
    private bool valAddedTaxFieldSpecified;
    private string taxTpField;
    private string pdctDescField;
    private string dlvryLctnField;
    private AttendanceContext2Code dlvrySvcField;
    private bool dlvrySvcFieldSpecified;
    private string saleChanlField;
    private string addtlPdctDescField;

    /// <remarks />
    public string ItmId
    {
        get => this.itmIdField;
        set => this.itmIdField = value;
    }

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

    /// <remarks />
    public decimal PdctQty
    {
        get => this.pdctQtyField;
        set => this.pdctQtyField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool PdctQtySpecified
    {
        get => this.pdctQtyFieldSpecified;
        set => this.pdctQtyFieldSpecified = value;
    }

    /// <remarks />
    public decimal UnitPric
    {
        get => this.unitPricField;
        set => this.unitPricField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool UnitPricSpecified
    {
        get => this.unitPricFieldSpecified;
        set => this.unitPricFieldSpecified = value;
    }

    /// <remarks />
    public bool UnitPricSgn
    {
        get => this.unitPricSgnField;
        set => this.unitPricSgnField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool UnitPricSgnSpecified
    {
        get => this.unitPricSgnFieldSpecified;
        set => this.unitPricSgnFieldSpecified = value;
    }

    /// <remarks />
    public decimal PdctAmt
    {
        get => this.pdctAmtField;
        set => this.pdctAmtField = value;
    }

    /// <remarks />
    public bool PdctAmtSgn
    {
        get => this.pdctAmtSgnField;
        set => this.pdctAmtSgnField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool PdctAmtSgnSpecified
    {
        get => this.pdctAmtSgnFieldSpecified;
        set => this.pdctAmtSgnFieldSpecified = value;
    }

    /// <remarks />
    public decimal ValAddedTax
    {
        get => this.valAddedTaxField;
        set => this.valAddedTaxField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool ValAddedTaxSpecified
    {
        get => this.valAddedTaxFieldSpecified;
        set => this.valAddedTaxFieldSpecified = value;
    }

    /// <remarks />
    public string TaxTp
    {
        get => this.taxTpField;
        set => this.taxTpField = value;
    }

    /// <remarks />
    public string PdctDesc
    {
        get => this.pdctDescField;
        set => this.pdctDescField = value;
    }

    /// <remarks />
    public string DlvryLctn
    {
        get => this.dlvryLctnField;
        set => this.dlvryLctnField = value;
    }

    /// <remarks />
    public AttendanceContext2Code DlvrySvc
    {
        get => this.dlvrySvcField;
        set => this.dlvrySvcField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool DlvrySvcSpecified
    {
        get => this.dlvrySvcFieldSpecified;
        set => this.dlvrySvcFieldSpecified = value;
    }

    /// <remarks />
    public string SaleChanl
    {
        get => this.saleChanlField;
        set => this.saleChanlField = value;
    }

    /// <remarks />
    public string AddtlPdctDesc
    {
        get => this.addtlPdctDescField;
        set => this.addtlPdctDescField = value;
    }

    #endregion
}