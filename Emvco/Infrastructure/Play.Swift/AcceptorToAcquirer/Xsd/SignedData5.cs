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
public partial class SignedData5
{
    #region Instance Values

    private decimal vrsnField;
    private bool vrsnFieldSpecified;
    private AlgorithmIdentification21[] dgstAlgoField;
    private EncapsulatedContent3 ncpsltdCnttField;
    private byte[][] certField;
    private Signer4[] sgnrField;

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
    [XmlElement("DgstAlgo")]
    public AlgorithmIdentification21[] DgstAlgo
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
    [XmlElement("Cert", DataType = "base64Binary")]
    public byte[][] Cert
    {
        get => this.certField;
        set => this.certField = value;
    }

    /// <remarks />
    [XmlElement("Sgnr")]
    public Signer4[] Sgnr
    {
        get => this.sgnrField;
        set => this.sgnrField = value;
    }

    #endregion
}