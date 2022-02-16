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
public partial class TransactionVerificationResult4
{
    #region Instance Values

    private AuthenticationMethod6Code mtdField;
    private AuthenticationEntity2Code vrfctnNttyField;
    private bool vrfctnNttyFieldSpecified;
    private Verification1Code rsltField;
    private bool rsltFieldSpecified;
    private string addtlRsltField;

    /// <remarks />
    public AuthenticationMethod6Code Mtd
    {
        get => this.mtdField;
        set => this.mtdField = value;
    }

    /// <remarks />
    public AuthenticationEntity2Code VrfctnNtty
    {
        get => this.vrfctnNttyField;
        set => this.vrfctnNttyField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool VrfctnNttySpecified
    {
        get => this.vrfctnNttyFieldSpecified;
        set => this.vrfctnNttyFieldSpecified = value;
    }

    /// <remarks />
    public Verification1Code Rslt
    {
        get => this.rsltField;
        set => this.rsltField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool RsltSpecified
    {
        get => this.rsltFieldSpecified;
        set => this.rsltFieldSpecified = value;
    }

    /// <remarks />
    public string AddtlRslt
    {
        get => this.addtlRsltField;
        set => this.addtlRsltField = value;
    }

    #endregion
}