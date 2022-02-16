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
public partial class Vehicle1
{
    #region Instance Values

    private string vhclNbField;
    private string trlrNbField;
    private string vhclTagField;
    private CardDataReading5Code vhclTagNtryMdField;
    private bool vhclTagNtryMdFieldSpecified;
    private string unitNbField;
    private bool rplcmntCarField;
    private bool rplcmntCarFieldSpecified;
    private decimal odmtrField;
    private bool odmtrFieldSpecified;
    private decimal hbmtrField;
    private bool hbmtrFieldSpecified;
    private string trlrHrsField;
    private string refrHrsField;
    private string mntncIdField;
    private PlainCardData17 drvrOrVhclCardField;
    private Vehicle2[] addtlVhclDataField;

    /// <remarks />
    public string VhclNb
    {
        get => this.vhclNbField;
        set => this.vhclNbField = value;
    }

    /// <remarks />
    public string TrlrNb
    {
        get => this.trlrNbField;
        set => this.trlrNbField = value;
    }

    /// <remarks />
    public string VhclTag
    {
        get => this.vhclTagField;
        set => this.vhclTagField = value;
    }

    /// <remarks />
    public CardDataReading5Code VhclTagNtryMd
    {
        get => this.vhclTagNtryMdField;
        set => this.vhclTagNtryMdField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool VhclTagNtryMdSpecified
    {
        get => this.vhclTagNtryMdFieldSpecified;
        set => this.vhclTagNtryMdFieldSpecified = value;
    }

    /// <remarks />
    public string UnitNb
    {
        get => this.unitNbField;
        set => this.unitNbField = value;
    }

    /// <remarks />
    public bool RplcmntCar
    {
        get => this.rplcmntCarField;
        set => this.rplcmntCarField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool RplcmntCarSpecified
    {
        get => this.rplcmntCarFieldSpecified;
        set => this.rplcmntCarFieldSpecified = value;
    }

    /// <remarks />
    public decimal Odmtr
    {
        get => this.odmtrField;
        set => this.odmtrField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool OdmtrSpecified
    {
        get => this.odmtrFieldSpecified;
        set => this.odmtrFieldSpecified = value;
    }

    /// <remarks />
    public decimal Hbmtr
    {
        get => this.hbmtrField;
        set => this.hbmtrField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool HbmtrSpecified
    {
        get => this.hbmtrFieldSpecified;
        set => this.hbmtrFieldSpecified = value;
    }

    /// <remarks />
    public string TrlrHrs
    {
        get => this.trlrHrsField;
        set => this.trlrHrsField = value;
    }

    /// <remarks />
    public string RefrHrs
    {
        get => this.refrHrsField;
        set => this.refrHrsField = value;
    }

    /// <remarks />
    public string MntncId
    {
        get => this.mntncIdField;
        set => this.mntncIdField = value;
    }

    /// <remarks />
    public PlainCardData17 DrvrOrVhclCard
    {
        get => this.drvrOrVhclCardField;
        set => this.drvrOrVhclCardField = value;
    }

    /// <remarks />
    [XmlElement("AddtlVhclData")]
    public Vehicle2[] AddtlVhclData
    {
        get => this.addtlVhclDataField;
        set => this.addtlVhclDataField = value;
    }

    #endregion
}