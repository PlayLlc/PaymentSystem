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
public partial class DateAndPlaceOfBirth1
{
    #region Instance Values

    private DateTime birthDtField;
    private string prvcOfBirthField;
    private string cityOfBirthField;
    private string ctryOfBirthField;

    /// <remarks />
    [XmlElement(DataType = "date")]
    public DateTime BirthDt
    {
        get => this.birthDtField;
        set => this.birthDtField = value;
    }

    /// <remarks />
    public string PrvcOfBirth
    {
        get => this.prvcOfBirthField;
        set => this.prvcOfBirthField = value;
    }

    /// <remarks />
    public string CityOfBirth
    {
        get => this.cityOfBirthField;
        set => this.cityOfBirthField = value;
    }

    /// <remarks />
    public string CtryOfBirth
    {
        get => this.ctryOfBirthField;
        set => this.ctryOfBirthField = value;
    }

    #endregion
}