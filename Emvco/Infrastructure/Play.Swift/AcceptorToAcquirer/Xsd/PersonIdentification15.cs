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
public partial class PersonIdentification15
{
    #region Instance Values

    private string drvrLicNbField;
    private string drvrLicLctnField;
    private string drvrLicNmField;
    private string drvrIdField;
    private string cstmrNbField;
    private string sclSctyNbField;
    private string alnRegnNbField;
    private string psptNbField;
    private string taxIdNbField;
    private string idntyCardNbField;
    private string mplyrIdNbField;
    private string mplyeeIdNbField;
    private string jobNbField;
    private string deptField;
    private string emailAdrField;
    private DateAndPlaceOfBirth1 dtAndPlcOfBirthField;
    private GenericIdentification4[] othrField;

    /// <remarks />
    public string DrvrLicNb
    {
        get => this.drvrLicNbField;
        set => this.drvrLicNbField = value;
    }

    /// <remarks />
    public string DrvrLicLctn
    {
        get => this.drvrLicLctnField;
        set => this.drvrLicLctnField = value;
    }

    /// <remarks />
    public string DrvrLicNm
    {
        get => this.drvrLicNmField;
        set => this.drvrLicNmField = value;
    }

    /// <remarks />
    public string DrvrId
    {
        get => this.drvrIdField;
        set => this.drvrIdField = value;
    }

    /// <remarks />
    public string CstmrNb
    {
        get => this.cstmrNbField;
        set => this.cstmrNbField = value;
    }

    /// <remarks />
    public string SclSctyNb
    {
        get => this.sclSctyNbField;
        set => this.sclSctyNbField = value;
    }

    /// <remarks />
    public string AlnRegnNb
    {
        get => this.alnRegnNbField;
        set => this.alnRegnNbField = value;
    }

    /// <remarks />
    public string PsptNb
    {
        get => this.psptNbField;
        set => this.psptNbField = value;
    }

    /// <remarks />
    public string TaxIdNb
    {
        get => this.taxIdNbField;
        set => this.taxIdNbField = value;
    }

    /// <remarks />
    public string IdntyCardNb
    {
        get => this.idntyCardNbField;
        set => this.idntyCardNbField = value;
    }

    /// <remarks />
    public string MplyrIdNb
    {
        get => this.mplyrIdNbField;
        set => this.mplyrIdNbField = value;
    }

    /// <remarks />
    public string MplyeeIdNb
    {
        get => this.mplyeeIdNbField;
        set => this.mplyeeIdNbField = value;
    }

    /// <remarks />
    public string JobNb
    {
        get => this.jobNbField;
        set => this.jobNbField = value;
    }

    /// <remarks />
    public string Dept
    {
        get => this.deptField;
        set => this.deptField = value;
    }

    /// <remarks />
    public string EmailAdr
    {
        get => this.emailAdrField;
        set => this.emailAdrField = value;
    }

    /// <remarks />
    public DateAndPlaceOfBirth1 DtAndPlcOfBirth
    {
        get => this.dtAndPlcOfBirthField;
        set => this.dtAndPlcOfBirthField = value;
    }

    /// <remarks />
    [XmlElement("Othr")]
    public GenericIdentification4[] Othr
    {
        get => this.othrField;
        set => this.othrField = value;
    }

    #endregion
}