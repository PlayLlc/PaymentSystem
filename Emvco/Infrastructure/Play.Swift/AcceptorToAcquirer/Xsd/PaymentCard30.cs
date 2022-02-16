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
public partial class PaymentCard30
{
    #region Instance Values

    private ContentInformationType22 prtctdCardDataField;
    private byte[] prvtCardDataField;
    private PlainCardData15 plainCardDataField;
    private string pmtAcctRefField;
    private string mskdPANField;
    private string issrBINField;
    private string cardCtryCdField;
    private string cardCcyCdField;
    private string cardPdctPrflField;
    private string cardBrndField;
    private CardProductType1Code cardPdctTpField;
    private bool cardPdctTpFieldSpecified;
    private string cardPdctSubTpField;
    private bool intrnlCardField;
    private bool intrnlCardFieldSpecified;
    private string[] allwdPdctField;
    private string svcOptnField;
    private string addtlCardDataField;

    /// <remarks />
    public ContentInformationType22 PrtctdCardData
    {
        get => this.prtctdCardDataField;
        set => this.prtctdCardDataField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] PrvtCardData
    {
        get => this.prvtCardDataField;
        set => this.prvtCardDataField = value;
    }

    /// <remarks />
    public PlainCardData15 PlainCardData
    {
        get => this.plainCardDataField;
        set => this.plainCardDataField = value;
    }

    /// <remarks />
    public string PmtAcctRef
    {
        get => this.pmtAcctRefField;
        set => this.pmtAcctRefField = value;
    }

    /// <remarks />
    public string MskdPAN
    {
        get => this.mskdPANField;
        set => this.mskdPANField = value;
    }

    /// <remarks />
    public string IssrBIN
    {
        get => this.issrBINField;
        set => this.issrBINField = value;
    }

    /// <remarks />
    public string CardCtryCd
    {
        get => this.cardCtryCdField;
        set => this.cardCtryCdField = value;
    }

    /// <remarks />
    public string CardCcyCd
    {
        get => this.cardCcyCdField;
        set => this.cardCcyCdField = value;
    }

    /// <remarks />
    public string CardPdctPrfl
    {
        get => this.cardPdctPrflField;
        set => this.cardPdctPrflField = value;
    }

    /// <remarks />
    public string CardBrnd
    {
        get => this.cardBrndField;
        set => this.cardBrndField = value;
    }

    /// <remarks />
    public CardProductType1Code CardPdctTp
    {
        get => this.cardPdctTpField;
        set => this.cardPdctTpField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool CardPdctTpSpecified
    {
        get => this.cardPdctTpFieldSpecified;
        set => this.cardPdctTpFieldSpecified = value;
    }

    /// <remarks />
    public string CardPdctSubTp
    {
        get => this.cardPdctSubTpField;
        set => this.cardPdctSubTpField = value;
    }

    /// <remarks />
    public bool IntrnlCard
    {
        get => this.intrnlCardField;
        set => this.intrnlCardField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool IntrnlCardSpecified
    {
        get => this.intrnlCardFieldSpecified;
        set => this.intrnlCardFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement("AllwdPdct")]
    public string[] AllwdPdct
    {
        get => this.allwdPdctField;
        set => this.allwdPdctField = value;
    }

    /// <remarks />
    public string SvcOptn
    {
        get => this.svcOptnField;
        set => this.svcOptnField = value;
    }

    /// <remarks />
    public string AddtlCardData
    {
        get => this.addtlCardDataField;
        set => this.addtlCardDataField = value;
    }

    #endregion
}