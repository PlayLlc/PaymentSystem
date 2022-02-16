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
public partial class Parameter7
{
    #region Instance Values

    private byte[] initlstnVctrField;
    private BytePadding1Code bPddgField;
    private bool bPddgFieldSpecified;

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] InitlstnVctr
    {
        get => this.initlstnVctrField;
        set => this.initlstnVctrField = value;
    }

    /// <remarks />
    public BytePadding1Code BPddg
    {
        get => this.bPddgField;
        set => this.bPddgField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool BPddgSpecified
    {
        get => this.bPddgFieldSpecified;
        set => this.bPddgFieldSpecified = value;
    }

    #endregion
}