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
public partial class Signer4
{
    #region Instance Values

    private decimal vrsnField;
    private bool vrsnFieldSpecified;
    private Recipient5Choice sgnrIdField;
    private AlgorithmIdentification21 dgstAlgoField;
    private GenericInformation1[] sgndAttrbtsField;
    private AlgorithmIdentification20 sgntrAlgoField;
    private byte[] sgntrField;

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
    public Recipient5Choice SgnrId
    {
        get => this.sgnrIdField;
        set => this.sgnrIdField = value;
    }

    /// <remarks />
    public AlgorithmIdentification21 DgstAlgo
    {
        get => this.dgstAlgoField;
        set => this.dgstAlgoField = value;
    }

    /// <remarks />
    [XmlElement("SgndAttrbts")]
    public GenericInformation1[] SgndAttrbts
    {
        get => this.sgndAttrbtsField;
        set => this.sgndAttrbtsField = value;
    }

    /// <remarks />
    public AlgorithmIdentification20 SgntrAlgo
    {
        get => this.sgntrAlgoField;
        set => this.sgntrAlgoField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] Sgntr
    {
        get => this.sgntrField;
        set => this.sgntrField = value;
    }

    #endregion
}