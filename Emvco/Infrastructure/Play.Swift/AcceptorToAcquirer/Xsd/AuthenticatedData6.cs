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
public partial class AuthenticatedData6
{
    #region Instance Values

    private decimal vrsnField;
    private bool vrsnFieldSpecified;
    private Recipient8Choice[] rcptField;
    private AlgorithmIdentification22 mACAlgoField;
    private EncapsulatedContent3 ncpsltdCnttField;
    private byte[] mACField;

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
    [XmlElement("Rcpt")]
    public Recipient8Choice[] Rcpt
    {
        get => this.rcptField;
        set => this.rcptField = value;
    }

    /// <remarks />
    public AlgorithmIdentification22 MACAlgo
    {
        get => this.mACAlgoField;
        set => this.mACAlgoField = value;
    }

    /// <remarks />
    public EncapsulatedContent3 NcpsltdCntt
    {
        get => this.ncpsltdCnttField;
        set => this.ncpsltdCnttField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] MAC
    {
        get => this.mACField;
        set => this.mACField = value;
    }

    #endregion
}