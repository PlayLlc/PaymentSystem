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
public partial class GenericIdentification90
{
    #region Instance Values

    private string idField;
    private PartyType14Code tpField;
    private PartyType4Code issrField;
    private bool issrFieldSpecified;
    private string ctryField;
    private string shrtNmField;

    /// <remarks />
    public string Id
    {
        get => this.idField;
        set => this.idField = value;
    }

    /// <remarks />
    public PartyType14Code Tp
    {
        get => this.tpField;
        set => this.tpField = value;
    }

    /// <remarks />
    public PartyType4Code Issr
    {
        get => this.issrField;
        set => this.issrField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool IssrSpecified
    {
        get => this.issrFieldSpecified;
        set => this.issrFieldSpecified = value;
    }

    /// <remarks />
    public string Ctry
    {
        get => this.ctryField;
        set => this.ctryField = value;
    }

    /// <remarks />
    public string ShrtNm
    {
        get => this.shrtNmField;
        set => this.shrtNmField = value;
    }

    #endregion
}