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
public partial class KEK7
{
    #region Instance Values

    private decimal vrsnField;
    private bool vrsnFieldSpecified;
    private KEKIdentifier2 kEKIdField;
    private AlgorithmIdentification29 keyNcrptnAlgoField;
    private byte[] ncrptdKeyField;

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
    public KEKIdentifier2 KEKId
    {
        get => this.kEKIdField;
        set => this.kEKIdField = value;
    }

    /// <remarks />
    public AlgorithmIdentification29 KeyNcrptnAlgo
    {
        get => this.keyNcrptnAlgoField;
        set => this.keyNcrptnAlgoField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] NcrptdKey
    {
        get => this.ncrptdKeyField;
        set => this.ncrptdKeyField = value;
    }

    #endregion
}