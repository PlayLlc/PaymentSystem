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
public partial class PlainCardData15
{
    #region Instance Values

    private string pANField;
    private string cardSeqNbField;
    private string fctvDtField;
    private string xpryDtField;
    private string svcCdField;
    private string trck1Field;
    private string trck2Field;
    private string trck3Field;
    private string crdhldrNmField;

    /// <remarks />
    public string PAN
    {
        get => this.pANField;
        set => this.pANField = value;
    }

    /// <remarks />
    public string CardSeqNb
    {
        get => this.cardSeqNbField;
        set => this.cardSeqNbField = value;
    }

    /// <remarks />
    public string FctvDt
    {
        get => this.fctvDtField;
        set => this.fctvDtField = value;
    }

    /// <remarks />
    public string XpryDt
    {
        get => this.xpryDtField;
        set => this.xpryDtField = value;
    }

    /// <remarks />
    public string SvcCd
    {
        get => this.svcCdField;
        set => this.svcCdField = value;
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
    public string CrdhldrNm
    {
        get => this.crdhldrNmField;
        set => this.crdhldrNmField = value;
    }

    #endregion
}