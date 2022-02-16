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
public partial class OriginalAmountDetails1
{
    #region Instance Values

    private decimal actlAmtField;
    private bool actlAmtFieldSpecified;
    private decimal minAmtField;
    private bool minAmtFieldSpecified;
    private decimal maxAmtField;
    private bool maxAmtFieldSpecified;

    /// <remarks />
    public decimal ActlAmt
    {
        get => this.actlAmtField;
        set => this.actlAmtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool ActlAmtSpecified
    {
        get => this.actlAmtFieldSpecified;
        set => this.actlAmtFieldSpecified = value;
    }

    /// <remarks />
    public decimal MinAmt
    {
        get => this.minAmtField;
        set => this.minAmtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool MinAmtSpecified
    {
        get => this.minAmtFieldSpecified;
        set => this.minAmtFieldSpecified = value;
    }

    /// <remarks />
    public decimal MaxAmt
    {
        get => this.maxAmtField;
        set => this.maxAmtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool MaxAmtSpecified
    {
        get => this.maxAmtFieldSpecified;
        set => this.maxAmtFieldSpecified = value;
    }

    #endregion
}