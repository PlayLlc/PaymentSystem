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
public partial class Parameter10
{
    #region Instance Values

    private EncryptionFormat2Code ncrptnFrmtField;
    private bool ncrptnFrmtFieldSpecified;
    private Algorithm16Code dgstAlgoField;
    private bool dgstAlgoFieldSpecified;
    private AlgorithmIdentification18 mskGnrtrAlgoField;

    /// <remarks />
    public EncryptionFormat2Code NcrptnFrmt
    {
        get => this.ncrptnFrmtField;
        set => this.ncrptnFrmtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool NcrptnFrmtSpecified
    {
        get => this.ncrptnFrmtFieldSpecified;
        set => this.ncrptnFrmtFieldSpecified = value;
    }

    /// <remarks />
    public Algorithm16Code DgstAlgo
    {
        get => this.dgstAlgoField;
        set => this.dgstAlgoField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool DgstAlgoSpecified
    {
        get => this.dgstAlgoFieldSpecified;
        set => this.dgstAlgoFieldSpecified = value;
    }

    /// <remarks />
    public AlgorithmIdentification18 MskGnrtrAlgo
    {
        get => this.mskGnrtrAlgoField;
        set => this.mskGnrtrAlgoField = value;
    }

    #endregion
}