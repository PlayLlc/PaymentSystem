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
public partial class DetailedAmount15
{
    #region Instance Values

    private decimal amtGoodsAndSvcsField;
    private bool amtGoodsAndSvcsFieldSpecified;
    private decimal cshBckField;
    private bool cshBckFieldSpecified;
    private decimal grttyField;
    private bool grttyFieldSpecified;
    private DetailedAmount4[] feesField;
    private DetailedAmount4[] rbtField;
    private DetailedAmount4[] valAddedTaxField;
    private DetailedAmount4[] srchrgField;

    /// <remarks />
    public decimal AmtGoodsAndSvcs
    {
        get => this.amtGoodsAndSvcsField;
        set => this.amtGoodsAndSvcsField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool AmtGoodsAndSvcsSpecified
    {
        get => this.amtGoodsAndSvcsFieldSpecified;
        set => this.amtGoodsAndSvcsFieldSpecified = value;
    }

    /// <remarks />
    public decimal CshBck
    {
        get => this.cshBckField;
        set => this.cshBckField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool CshBckSpecified
    {
        get => this.cshBckFieldSpecified;
        set => this.cshBckFieldSpecified = value;
    }

    /// <remarks />
    public decimal Grtty
    {
        get => this.grttyField;
        set => this.grttyField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool GrttySpecified
    {
        get => this.grttyFieldSpecified;
        set => this.grttyFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement("Fees")]
    public DetailedAmount4[] Fees
    {
        get => this.feesField;
        set => this.feesField = value;
    }

    /// <remarks />
    [XmlElement("Rbt")]
    public DetailedAmount4[] Rbt
    {
        get => this.rbtField;
        set => this.rbtField = value;
    }

    /// <remarks />
    [XmlElement("ValAddedTax")]
    public DetailedAmount4[] ValAddedTax
    {
        get => this.valAddedTaxField;
        set => this.valAddedTaxField = value;
    }

    /// <remarks />
    [XmlElement("Srchrg")]
    public DetailedAmount4[] Srchrg
    {
        get => this.srchrgField;
        set => this.srchrgField = value;
    }

    #endregion
}