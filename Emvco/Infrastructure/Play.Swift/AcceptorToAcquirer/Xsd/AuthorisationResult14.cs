using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

/// <remarks />
[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:iso:std:iso:20022:tech:xsd:caaa.002.001.09")]
public partial class AuthorisationResult14
{
    #region Instance Values

    private GenericIdentification90 authstnNttyField;
    private ResponseType10 rspnToAuthstnField;
    private string authstnCdField;
    private bool cmpltnReqrdField;
    private bool cmpltnReqrdFieldSpecified;
    private TMSTrigger1 tMSTrggrField;

    /// <remarks />
    public GenericIdentification90 AuthstnNtty
    {
        get => this.authstnNttyField;
        set => this.authstnNttyField = value;
    }

    /// <remarks />
    public ResponseType10 RspnToAuthstn
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

    /// <remarks />
    public bool CmpltnReqrd
    {
        get => this.cmpltnReqrdField;
        set => this.cmpltnReqrdField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool CmpltnReqrdSpecified
    {
        get => this.cmpltnReqrdFieldSpecified;
        set => this.cmpltnReqrdFieldSpecified = value;
    }

    /// <remarks />
    public TMSTrigger1 TMSTrggr
    {
        get => this.tMSTrggrField;
        set => this.tMSTrggrField = value;
    }

    #endregion
}