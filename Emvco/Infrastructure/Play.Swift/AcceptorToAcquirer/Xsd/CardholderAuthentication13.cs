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
public partial class CardholderAuthentication13
{
    #region Instance Values

    private AuthenticationMethod8Code authntcnMtdField;
    private bool authntcnMtdFieldSpecified;
    private Exemption1Code authntcnXmptnField;
    private bool authntcnXmptnFieldSpecified;
    private byte[] authntcnValField;
    private ContentInformationType22 prtctdAuthntcnValField;
    private OnLinePIN7 crdhldrOnLinePINField;
    private PersonIdentification15 crdhldrIdField;
    private AddressVerification1 adrVrfctnField;
    private string authntcnTpField;
    private string authntcnLvlField;
    private AuthenticationResult1Code authntcnRsltField;
    private bool authntcnRsltFieldSpecified;
    private ExternallyDefinedData1 authntcnAddtlInfField;

    /// <remarks />
    public AuthenticationMethod8Code AuthntcnMtd
    {
        get => this.authntcnMtdField;
        set => this.authntcnMtdField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool AuthntcnMtdSpecified
    {
        get => this.authntcnMtdFieldSpecified;
        set => this.authntcnMtdFieldSpecified = value;
    }

    /// <remarks />
    public Exemption1Code AuthntcnXmptn
    {
        get => this.authntcnXmptnField;
        set => this.authntcnXmptnField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool AuthntcnXmptnSpecified
    {
        get => this.authntcnXmptnFieldSpecified;
        set => this.authntcnXmptnFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] AuthntcnVal
    {
        get => this.authntcnValField;
        set => this.authntcnValField = value;
    }

    /// <remarks />
    public ContentInformationType22 PrtctdAuthntcnVal
    {
        get => this.prtctdAuthntcnValField;
        set => this.prtctdAuthntcnValField = value;
    }

    /// <remarks />
    public OnLinePIN7 CrdhldrOnLinePIN
    {
        get => this.crdhldrOnLinePINField;
        set => this.crdhldrOnLinePINField = value;
    }

    /// <remarks />
    public PersonIdentification15 CrdhldrId
    {
        get => this.crdhldrIdField;
        set => this.crdhldrIdField = value;
    }

    /// <remarks />
    public AddressVerification1 AdrVrfctn
    {
        get => this.adrVrfctnField;
        set => this.adrVrfctnField = value;
    }

    /// <remarks />
    public string AuthntcnTp
    {
        get => this.authntcnTpField;
        set => this.authntcnTpField = value;
    }

    /// <remarks />
    public string AuthntcnLvl
    {
        get => this.authntcnLvlField;
        set => this.authntcnLvlField = value;
    }

    /// <remarks />
    public AuthenticationResult1Code AuthntcnRslt
    {
        get => this.authntcnRsltField;
        set => this.authntcnRsltField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool AuthntcnRsltSpecified
    {
        get => this.authntcnRsltFieldSpecified;
        set => this.authntcnRsltFieldSpecified = value;
    }

    /// <remarks />
    public ExternallyDefinedData1 AuthntcnAddtlInf
    {
        get => this.authntcnAddtlInfField;
        set => this.authntcnAddtlInfField = value;
    }

    #endregion
}