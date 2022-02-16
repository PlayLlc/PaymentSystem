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
public partial class PlainCardData17
{
    #region Instance Values

    private string pANField;
    private string trck1Field;
    private string trck2Field;
    private string trck3Field;
    private string[] addtlCardDataField;
    private CardDataReading5Code ntryMdField;
    private bool ntryMdFieldSpecified;

    /// <remarks />
    public string PAN
    {
        get => this.pANField;
        set => this.pANField = value;
    }

    /// <remarks />
    public string Trck1
    {
        get => this.trck1Field;
        set => this.trck1Field = value;
    }

    /// <remarks />
    public string Trck2
    {
        get => this.trck2Field;
        set => this.trck2Field = value;
    }

    /// <remarks />
    public string Trck3
    {
        get => this.trck3Field;
        set => this.trck3Field = value;
    }

    /// <remarks />
    [XmlElement("AddtlCardData")]
    public string[] AddtlCardData
    {
        get => this.addtlCardDataField;
        set => this.addtlCardDataField = value;
    }

    /// <remarks />
    public CardDataReading5Code NtryMd
    {
        get => this.ntryMdField;
        set => this.ntryMdField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool NtryMdSpecified
    {
        get => this.ntryMdFieldSpecified;
        set => this.ntryMdFieldSpecified = value;
    }

    #endregion
}