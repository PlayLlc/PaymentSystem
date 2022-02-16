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
public partial class ContentInformationType23
{
    #region Instance Values

    private ContentType2Code cnttTpField;
    private EnvelopedData7 envlpdDataField;
    private AuthenticatedData6 authntcdDataField;
    private SignedData5 sgndDataField;
    private DigestedData5 dgstdDataField;

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

    /// <remarks />
    public AuthenticatedData6 AuthntcdData
    {
        get => this.authntcdDataField;
        set => this.authntcdDataField = value;
    }

    /// <remarks />
    public SignedData5 SgndData
    {
        get => this.sgndDataField;
        set => this.sgndDataField = value;
    }

    /// <remarks />
    public DigestedData5 DgstdData
    {
        get => this.dgstdDataField;
        set => this.dgstdDataField = value;
    }

    #endregion
}