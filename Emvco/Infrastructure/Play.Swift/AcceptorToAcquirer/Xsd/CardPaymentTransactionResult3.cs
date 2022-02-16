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
public partial class CardPaymentTransactionResult3
{
    #region Instance Values

    private GenericIdentification90 authstnNttyField;
    private ResponseType5 rspnToAuthstnField;
    private string authstnCdField;

    /// <remarks />
    public GenericIdentification90 AuthstnNtty
    {
        get => this.authstnNttyField;
        set => this.authstnNttyField = value;
    }

    /// <remarks />
    public ResponseType5 RspnToAuthstn
    {
        get => this.rspnToAuthstnField;
        set => this.rspnToAuthstnField = value;
    }

    /// <remarks />
    public string AuthstnCd
    {
        get => this.authstnCdField;
        set => this.authstnCdField = value;
    }

    #endregion
}