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
public partial class SensitiveMobileData1
{
    #region Instance Values

    private string mSISDNField;
    private string iMSIField;
    private string iMEIField;

    /// <remarks />
    public string MSISDN
    {
        get => this.mSISDNField;
        set => this.mSISDNField = value;
    }

    /// <remarks />
    public string IMSI
    {
        get => this.iMSIField;
        set => this.iMSIField = value;
    }

    /// <remarks />
    public string IMEI
    {
        get => this.iMEIField;
        set => this.iMEIField = value;
    }

    #endregion
}