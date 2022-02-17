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
public partial class AcceptorAuthorisationResponseV09
{
    #region Instance Values

    private Header58 hdrField;
    private AcceptorAuthorisationResponse9 authstnRspnField;
    private ContentInformationType24 sctyTrlrField;

    /// <remarks />
    public Header58 Hdr
    {
        get => hdrField;
        set => hdrField = value;
    }

    /// <remarks />
    public AcceptorAuthorisationResponse9 AuthstnRspn
    {
        get => authstnRspnField;
        set => authstnRspnField = value;
    }

    /// <remarks />
    public ContentInformationType24 SctyTrlr
    {
        get => sctyTrlrField;
        set => sctyTrlrField = value;
    }

    #endregion
}