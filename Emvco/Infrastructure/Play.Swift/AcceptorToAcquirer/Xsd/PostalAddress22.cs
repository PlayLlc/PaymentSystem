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
public partial class PostalAddress22
{
    #region Instance Values

    private AddressType2Code adrTpField;
    private bool adrTpFieldSpecified;
    private string deptField;
    private string subDeptField;
    private string[] adrLineField;
    private string strtNmField;
    private string bldgNbField;
    private string pstCdField;
    private string twnNmField;
    private string[] ctrySubDvsnField;
    private string ctryCdField;

    /// <remarks />
    public AddressType2Code AdrTp
    {
        get => this.adrTpField;
        set => this.adrTpField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool AdrTpSpecified
    {
        get => this.adrTpFieldSpecified;
        set => this.adrTpFieldSpecified = value;
    }

    /// <remarks />
    public string Dept
    {
        get => this.deptField;
        set => this.deptField = value;
    }

    /// <remarks />
    public string SubDept
    {
        get => this.subDeptField;
        set => this.subDeptField = value;
    }

    /// <remarks />
    [XmlElement("AdrLine")]
    public string[] AdrLine
    {
        get => this.adrLineField;
        set => this.adrLineField = value;
    }

    /// <remarks />
    public string StrtNm
    {
        get => this.strtNmField;
        set => this.strtNmField = value;
    }

    /// <remarks />
    public string BldgNb
    {
        get => this.bldgNbField;
        set => this.bldgNbField = value;
    }

    /// <remarks />
    public string PstCd
    {
        get => this.pstCdField;
        set => this.pstCdField = value;
    }

    /// <remarks />
    public string TwnNm
    {
        get => this.twnNmField;
        set => this.twnNmField = value;
    }

    /// <remarks />
    [XmlElement("CtrySubDvsn")]
    public string[] CtrySubDvsn
    {
        get => this.ctrySubDvsnField;
        set => this.ctrySubDvsnField = value;
    }

    /// <remarks />
    public string CtryCd
    {
        get => this.ctryCdField;
        set => this.ctryCdField = value;
    }

    #endregion
}