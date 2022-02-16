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
public partial class ContentInformationType22
{
    #region Instance Values

    private ContentType2Code cnttTpField;
    private EnvelopedData7 envlpdDataField;

    /// <remarks />
    public ContentType2Code CnttTp
    {
        get => this.cnttTpField;
        set => this.cnttTpField = value;
    }

    /// <remarks />
    public EnvelopedData7 EnvlpdData
    {
        get => this.envlpdDataField;
        set => this.envlpdDataField = value;
    }

    #endregion
}