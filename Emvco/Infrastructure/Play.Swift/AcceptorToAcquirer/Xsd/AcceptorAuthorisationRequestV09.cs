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
public partial class AcceptorAuthorisationRequestV09
{
    #region Instance Values

    private Header58 hdrField;
    private AcceptorAuthorisationRequest9 authstnReqField;
    private ContentInformationType24 sctyTrlrField;

    /// <remarks />
    public Header58 Hdr
    {
        get => hdrField;
        set => hdrField = value;
    }

    /// <remarks />
    public AcceptorAuthorisationRequest9 AuthstnReq
    {
        get => authstnReqField;
        set => authstnReqField = value;
    }

    /// <remarks />
    public ContentInformationType24 SctyTrlr
    {
        get => sctyTrlrField;
        set => sctyTrlrField = value;
    }

    #endregion
}