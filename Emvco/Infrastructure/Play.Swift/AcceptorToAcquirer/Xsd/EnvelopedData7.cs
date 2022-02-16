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
public partial class EnvelopedData7
{
    #region Instance Values

    private decimal vrsnField;
    private bool vrsnFieldSpecified;
    private byte[][] orgtrInfField;
    private Recipient8Choice[] rcptField;
    private EncryptedContent6 ncrptdCnttField;

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
    [XmlArrayItem("Cert", DataType = "base64Binary", IsNullable = false)]
    public byte[][] OrgtrInf
    {
        get => this.orgtrInfField;
        set => this.orgtrInfField = value;
    }

    /// <remarks />
    [XmlElement("Rcpt")]
    public Recipient8Choice[] Rcpt
    {
        get => this.rcptField;
        set => this.rcptField = value;
    }

    /// <remarks />
    public EncryptedContent6 NcrptdCntt
    {
        get => this.ncrptdCnttField;
        set => this.ncrptdCnttField = value;
    }

    #endregion
}