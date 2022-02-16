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
public partial class PhysicalInterfaceParameter1
{
    #region Instance Values

    private string intrfcNmField;
    private POICommunicationType2Code intrfcTpField;
    private bool intrfcTpFieldSpecified;
    private string usrNmField;
    private byte[] accsCdField;
    private string sctyPrflField;
    private byte[] addtlParamsField;

    /// <remarks />
    public string IntrfcNm
    {
        get => this.intrfcNmField;
        set => this.intrfcNmField = value;
    }

    /// <remarks />
    public POICommunicationType2Code IntrfcTp
    {
        get => this.intrfcTpField;
        set => this.intrfcTpField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool IntrfcTpSpecified
    {
        get => this.intrfcTpFieldSpecified;
        set => this.intrfcTpFieldSpecified = value;
    }

    /// <remarks />
    public string UsrNm
    {
        get => this.usrNmField;
        set => this.usrNmField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] AccsCd
    {
        get => this.accsCdField;
        set => this.accsCdField = value;
    }

    /// <remarks />
    public string SctyPrfl
    {
        get => this.sctyPrflField;
        set => this.sctyPrflField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] AddtlParams
    {
        get => this.addtlParamsField;
        set => this.addtlParamsField = value;
    }

    #endregion
}