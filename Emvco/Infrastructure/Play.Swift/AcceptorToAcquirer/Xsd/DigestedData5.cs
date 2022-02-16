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
public partial class DigestedData5
{
    #region Instance Values

    private decimal vrsnField;
    private bool vrsnFieldSpecified;
    private AlgorithmIdentification21 dgstAlgoField;
    private EncapsulatedContent3 ncpsltdCnttField;
    private byte[] dgstField;

    /// <remarks />
    public decimal Vrsn
    {
        get => this.vrsnField;
        set => this.vrsnField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool VrsnSpecified
    {
        get => this.vrsnFieldSpecified;
        set => this.vrsnFieldSpecified = value;
    }

    /// <remarks />
    public AlgorithmIdentification21 DgstAlgo
    {
        get => this.dgstAlgoField;
        set => this.dgstAlgoField = value;
    }

    /// <remarks />
    public EncapsulatedContent3 NcpsltdCntt
    {
        get => this.ncpsltdCnttField;
        set => this.ncpsltdCnttField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] Dgst
    {
        get => this.dgstField;
        set => this.dgstField = value;
    }

    #endregion
}