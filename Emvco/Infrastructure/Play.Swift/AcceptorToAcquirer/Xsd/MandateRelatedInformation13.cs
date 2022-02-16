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
public partial class MandateRelatedInformation13
{
    #region Instance Values

    private string mndtIdField;
    private DateTime dtOfSgntrField;
    private bool dtOfSgntrFieldSpecified;
    private byte[] mndtImgField;

    /// <remarks />
    public string MndtId
    {
        get => this.mndtIdField;
        set => this.mndtIdField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "date")]
    public DateTime DtOfSgntr
    {
        get => this.dtOfSgntrField;
        set => this.dtOfSgntrField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool DtOfSgntrSpecified
    {
        get => this.dtOfSgntrFieldSpecified;
        set => this.dtOfSgntrFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] MndtImg
    {
        get => this.mndtImgField;
        set => this.mndtImgField = value;
    }

    #endregion
}