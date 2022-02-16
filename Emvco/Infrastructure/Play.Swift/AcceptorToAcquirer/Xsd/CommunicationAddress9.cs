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
public partial class CommunicationAddress9
{
    #region Instance Values

    private PostalAddress22 pstlAdrField;
    private string emailField;
    private string uRLAdrField;
    private string phneField;
    private string cstmrSvcField;
    private string addtlCtctInfField;

    /// <remarks />
    public PostalAddress22 PstlAdr
    {
        get => this.pstlAdrField;
        set => this.pstlAdrField = value;
    }

    /// <remarks />
    public string Email
    {
        get => this.emailField;
        set => this.emailField = value;
    }

    /// <remarks />
    public string URLAdr
    {
        get => this.uRLAdrField;
        set => this.uRLAdrField = value;
    }

    /// <remarks />
    public string Phne
    {
        get => this.phneField;
        set => this.phneField = value;
    }

    /// <remarks />
    public string CstmrSvc
    {
        get => this.cstmrSvcField;
        set => this.cstmrSvcField = value;
    }

    /// <remarks />
    public string AddtlCtctInf
    {
        get => this.addtlCtctInfField;
        set => this.addtlCtctInfField = value;
    }

    #endregion
}