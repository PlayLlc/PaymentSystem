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
public partial class CurrencyConversion19
{
    #region Instance Values

    private string ccyConvsIdField;
    private CurrencyDetails3 trgtCcyField;
    private decimal rsltgAmtField;
    private decimal xchgRateField;
    private decimal nvrtdXchgRateField;
    private bool nvrtdXchgRateFieldSpecified;
    private DateTime qtnDtField;
    private bool qtnDtFieldSpecified;
    private DateTime vldUntilField;
    private bool vldUntilFieldSpecified;
    private CurrencyDetails2 srcCcyField;
    private OriginalAmountDetails1 orgnlAmtField;
    private Commission19[] comssnDtlsField;
    private Commission18[] mrkUpDtlsField;
    private ActionMessage7[] dclrtnDtlsField;

    /// <remarks />
    public string CcyConvsId
    {
        get => this.ccyConvsIdField;
        set => this.ccyConvsIdField = value;
    }

    /// <remarks />
    public CurrencyDetails3 TrgtCcy
    {
        get => this.trgtCcyField;
        set => this.trgtCcyField = value;
    }

    /// <remarks />
    public decimal RsltgAmt
    {
        get => this.rsltgAmtField;
        set => this.rsltgAmtField = value;
    }

    /// <remarks />
    public decimal XchgRate
    {
        get => this.xchgRateField;
        set => this.xchgRateField = value;
    }

    /// <remarks />
    public decimal NvrtdXchgRate
    {
        get => this.nvrtdXchgRateField;
        set => this.nvrtdXchgRateField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool NvrtdXchgRateSpecified
    {
        get => this.nvrtdXchgRateFieldSpecified;
        set => this.nvrtdXchgRateFieldSpecified = value;
    }

    /// <remarks />
    public DateTime QtnDt
    {
        get => this.qtnDtField;
        set => this.qtnDtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool QtnDtSpecified
    {
        get => this.qtnDtFieldSpecified;
        set => this.qtnDtFieldSpecified = value;
    }

    /// <remarks />
    public DateTime VldUntil
    {
        get => this.vldUntilField;
        set => this.vldUntilField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool VldUntilSpecified
    {
        get => this.vldUntilFieldSpecified;
        set => this.vldUntilFieldSpecified = value;
    }

    /// <remarks />
    public CurrencyDetails2 SrcCcy
    {
        get => this.srcCcyField;
        set => this.srcCcyField = value;
    }

    /// <remarks />
    public OriginalAmountDetails1 OrgnlAmt
    {
        get => this.orgnlAmtField;
        set => this.orgnlAmtField = value;
    }

    /// <remarks />
    [XmlElement("ComssnDtls")]
    public Commission19[] ComssnDtls
    {
        get => this.comssnDtlsField;
        set => this.comssnDtlsField = value;
    }

    /// <remarks />
    [XmlElement("MrkUpDtls")]
    public Commission18[] MrkUpDtls
    {
        get => this.mrkUpDtlsField;
        set => this.mrkUpDtlsField = value;
    }

    /// <remarks />
    [XmlElement("DclrtnDtls")]
    public ActionMessage7[] DclrtnDtls
    {
        get => this.dclrtnDtlsField;
        set => this.dclrtnDtlsField = value;
    }

    #endregion
}