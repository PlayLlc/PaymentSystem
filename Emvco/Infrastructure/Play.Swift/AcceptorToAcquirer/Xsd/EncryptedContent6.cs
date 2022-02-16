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
public partial class EncryptedContent6
{
    #region Instance Values

    private ContentType2Code cnttTpField;
    private AlgorithmIdentification29 cnttNcrptnAlgoField;
    private byte[] ncrptdDataField;

    /// <remarks />
    public ContentType2Code CnttTp
    {
        get => this.cnttTpField;
        set => this.cnttTpField = value;
    }

    /// <remarks />
    public AlgorithmIdentification29 CnttNcrptnAlgo
    {
        get => this.cnttNcrptnAlgoField;
        set => this.cnttNcrptnAlgoField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] NcrptdData
    {
        get => this.ncrptdDataField;
        set => this.ncrptdDataField = value;
    }

    #endregion
}