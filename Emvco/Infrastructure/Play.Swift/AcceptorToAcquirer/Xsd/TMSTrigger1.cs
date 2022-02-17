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
public partial class TMSTrigger1
{
    #region Instance Values

    private TMSContactLevel1Code tMSCtctLvlField;
    private string tMSIdField;
    private DateTime tMSCtctDtTmField;
    private bool tMSCtctDtTmFieldSpecified;

    /// <remarks />
    public TMSContactLevel1Code TMSCtctLvl
    {
        get => this.tMSCtctLvlField;
        set => this.tMSCtctLvlField = value;
    }

    /// <remarks />
    public string TMSId
    {
        get => this.tMSIdField;
        set => this.tMSIdField = value;
    }

    /// <remarks />
    public DateTime TMSCtctDtTm
    {
        get => this.tMSCtctDtTmField;
        set => this.tMSCtctDtTmField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool TMSCtctDtTmSpecified
    {
        get => this.tMSCtctDtTmFieldSpecified;
        set => this.tMSCtctDtTmFieldSpecified = value;
    }

    #endregion
}