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
public partial class DetailedAmount21
{
    #region Instance Values

    private decimal amtField;
    private DateTime dtTmField;
    private CardDataReading8Code cardDataNtryMdField;
    private bool cardDataNtryMdFieldSpecified;
    private byte[] iCCRltdDataField;
    private string lablField;

    /// <remarks />
    public decimal Amt
    {
        get => this.amtField;
        set => this.amtField = value;
    }

    /// <remarks />
    public DateTime DtTm
    {
        get => this.dtTmField;
        set => this.dtTmField = value;
    }

    /// <remarks />
    public CardDataReading8Code CardDataNtryMd
    {
        get => this.cardDataNtryMdField;
        set => this.cardDataNtryMdField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool CardDataNtryMdSpecified
    {
        get => this.cardDataNtryMdFieldSpecified;
        set => this.cardDataNtryMdFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] ICCRltdData
    {
        get => this.iCCRltdDataField;
        set => this.iCCRltdDataField = value;
    }

    /// <remarks />
    public string Labl
    {
        get => this.lablField;
        set => this.lablField = value;
    }

    #endregion
}