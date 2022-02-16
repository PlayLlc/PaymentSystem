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
public partial class Organisation26
{
    #region Instance Values

    private string cmonNmField;
    private string adrField;
    private string ctryCdField;
    private string mrchntCtgyCdField;
    private string regdIdrField;

    /// <remarks />
    public string CmonNm
    {
        get => this.cmonNmField;
        set => this.cmonNmField = value;
    }

    /// <remarks />
    public string Adr
    {
        get => this.adrField;
        set => this.adrField = value;
    }

    /// <remarks />
    public string CtryCd
    {
        get => this.ctryCdField;
        set => this.ctryCdField = value;
    }

    /// <remarks />
    public string MrchntCtgyCd
    {
        get => this.mrchntCtgyCdField;
        set => this.mrchntCtgyCdField = value;
    }

    /// <remarks />
    public string RegdIdr
    {
        get => this.regdIdrField;
        set => this.regdIdrField = value;
    }

    #endregion
}