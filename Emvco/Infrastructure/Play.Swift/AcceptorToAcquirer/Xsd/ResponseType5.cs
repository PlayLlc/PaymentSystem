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
public partial class ResponseType5
{
    #region Instance Values

    private Response4Code rspnField;
    private string rspnRsnField;
    private string addtlRspnInfField;

    /// <remarks />
    public Response4Code Rspn
    {
        get => this.rspnField;
        set => this.rspnField = value;
    }

    /// <remarks />
    public string RspnRsn
    {
        get => this.rspnRsnField;
        set => this.rspnRsnField = value;
    }

    /// <remarks />
    public string AddtlRspnInf
    {
        get => this.addtlRspnInfField;
        set => this.addtlRspnInfField = value;
    }

    #endregion
}