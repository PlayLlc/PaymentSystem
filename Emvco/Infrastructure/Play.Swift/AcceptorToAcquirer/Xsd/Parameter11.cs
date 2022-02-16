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
public partial class Parameter11
{
    #region Instance Values

    private Algorithm16Code dgstAlgoField;
    private AlgorithmIdentification12 mskGnrtrAlgoField;
    private decimal saltLngthField;
    private decimal trlrFldField;
    private bool trlrFldFieldSpecified;

    /// <remarks />
    public Algorithm16Code DgstAlgo
    {
        get => this.dgstAlgoField;
        set => this.dgstAlgoField = value;
    }

    /// <remarks />
    public AlgorithmIdentification12 MskGnrtrAlgo
    {
        get => this.mskGnrtrAlgoField;
        set => this.mskGnrtrAlgoField = value;
    }

    /// <remarks />
    public decimal SaltLngth
    {
        get => this.saltLngthField;
        set => this.saltLngthField = value;
    }

    /// <remarks />
    public decimal TrlrFld
    {
        get => this.trlrFldField;
        set => this.trlrFldField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool TrlrFldSpecified
    {
        get => this.trlrFldFieldSpecified;
        set => this.trlrFldFieldSpecified = value;
    }

    #endregion
}