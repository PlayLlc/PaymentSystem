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
public partial class MobileData2
{
    #region Instance Values

    private string mobCtryCdField;
    private string mobNtwkCdField;
    private string mobMskdMSISDNField;
    private Geolocation1 glctnField;
    private SensitiveMobileData1 snstvMobDataField;
    private ContentInformationType22 prtctdMobDataField;

    /// <remarks />
    public string MobCtryCd
    {
        get => this.mobCtryCdField;
        set => this.mobCtryCdField = value;
    }

    /// <remarks />
    public string MobNtwkCd
    {
        get => this.mobNtwkCdField;
        set => this.mobNtwkCdField = value;
    }

    /// <remarks />
    public string MobMskdMSISDN
    {
        get => this.mobMskdMSISDNField;
        set => this.mobMskdMSISDNField = value;
    }

    /// <remarks />
    public Geolocation1 Glctn
    {
        get => this.glctnField;
        set => this.glctnField = value;
    }

    /// <remarks />
    public SensitiveMobileData1 SnstvMobData
    {
        get => this.snstvMobDataField;
        set => this.snstvMobDataField = value;
    }

    /// <remarks />
    public ContentInformationType22 PrtctdMobData
    {
        get => this.prtctdMobDataField;
        set => this.prtctdMobDataField = value;
    }

    #endregion
}