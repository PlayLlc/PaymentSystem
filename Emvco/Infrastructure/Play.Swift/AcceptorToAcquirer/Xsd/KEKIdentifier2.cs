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
public partial class KEKIdentifier2
{
    #region Instance Values

    private string keyIdField;
    private string keyVrsnField;
    private decimal seqNbField;
    private bool seqNbFieldSpecified;
    private byte[] derivtnIdField;

    /// <remarks />
    public string KeyId
    {
        get => this.keyIdField;
        set => this.keyIdField = value;
    }

    /// <remarks />
    public string KeyVrsn
    {
        get => this.keyVrsnField;
        set => this.keyVrsnField = value;
    }

    /// <remarks />
    public decimal SeqNb
    {
        get => this.seqNbField;
        set => this.seqNbField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool SeqNbSpecified
    {
        get => this.seqNbFieldSpecified;
        set => this.seqNbFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] DerivtnId
    {
        get => this.derivtnIdField;
        set => this.derivtnIdField = value;
    }

    #endregion
}