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
public partial class PostalAddress2
{
    #region Instance Values

    private string strtNmField;
    private string pstCdIdField;
    private string twnNmField;
    private string ctrySubDvsnField;
    private string ctryField;

    /// <remarks />
    public string StrtNm
    {
        get => this.strtNmField;
        set => this.strtNmField = value;
    }

    /// <remarks />
    public string PstCdId
    {
        get => this.pstCdIdField;
        set => this.pstCdIdField = value;
    }

    /// <remarks />
    public string TwnNm
    {
        get => this.twnNmField;
        set => this.twnNmField = value;
    }

    /// <remarks />
    public string CtrySubDvsn
    {
        get => this.ctrySubDvsnField;
        set => this.ctrySubDvsnField = value;
    }

    /// <remarks />
    public string Ctry
    {
        get => this.ctryField;
        set => this.ctryField = value;
    }

    #endregion
}