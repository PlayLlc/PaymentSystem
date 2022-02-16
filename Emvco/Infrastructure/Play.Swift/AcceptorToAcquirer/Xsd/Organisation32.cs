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
public partial class Organisation32
{
    #region Instance Values

    private GenericIdentification32 idField;
    private string cmonNmField;
    private LocationCategory1Code lctnCtgyField;
    private bool lctnCtgyFieldSpecified;
    private CommunicationAddress9 lctnAndCtctField;
    private string schmeDataField;

    /// <remarks />
    public GenericIdentification32 Id
    {
        get => this.idField;
        set => this.idField = value;
    }

    /// <remarks />
    public string CmonNm
    {
        get => this.cmonNmField;
        set => this.cmonNmField = value;
    }

    /// <remarks />
    public LocationCategory1Code LctnCtgy
    {
        get => this.lctnCtgyField;
        set => this.lctnCtgyField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool LctnCtgySpecified
    {
        get => this.lctnCtgyFieldSpecified;
        set => this.lctnCtgyFieldSpecified = value;
    }

    /// <remarks />
    public CommunicationAddress9 LctnAndCtct
    {
        get => this.lctnAndCtctField;
        set => this.lctnAndCtctField = value;
    }

    /// <remarks />
    public string SchmeData
    {
        get => this.schmeDataField;
        set => this.schmeDataField = value;
    }

    #endregion
}