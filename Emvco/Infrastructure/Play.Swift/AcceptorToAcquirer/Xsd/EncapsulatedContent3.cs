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
public partial class EncapsulatedContent3
{
    #region Instance Values

    private ContentType2Code cnttTpField;
    private byte[] cnttField;

    /// <remarks />
    public ContentType2Code CnttTp
    {
        get => this.cnttTpField;
        set => this.cnttTpField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] Cntt
    {
        get => this.cnttField;
        set => this.cnttField = value;
    }

    #endregion
}