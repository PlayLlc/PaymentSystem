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
public partial class PaymentContext27
{
    #region Instance Values

    private bool cardPresField;
    private bool cardPresFieldSpecified;
    private bool crdhldrPresField;
    private bool crdhldrPresFieldSpecified;
    private bool onLineCntxtField;
    private bool onLineCntxtFieldSpecified;
    private AttendanceContext1Code attndncCntxtField;
    private bool attndncCntxtFieldSpecified;
    private TransactionEnvironment1Code txEnvtField;
    private bool txEnvtFieldSpecified;
    private TransactionChannel5Code txChanlField;
    private bool txChanlFieldSpecified;
    private bool attndntMsgCpblField;
    private bool attndntMsgCpblFieldSpecified;
    private string attndntLangField;
    private CardDataReading8Code cardDataNtryMdField;
    private bool cardDataNtryMdFieldSpecified;
    private CardFallback1Code fllbckIndField;
    private bool fllbckIndFieldSpecified;
    private SupportedPaymentOption1Code[] spprtdOptnField;

    /// <remarks />
    public bool CardPres
    {
        get => this.cardPresField;
        set => this.cardPresField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool CardPresSpecified
    {
        get => this.cardPresFieldSpecified;
        set => this.cardPresFieldSpecified = value;
    }

    /// <remarks />
    public bool CrdhldrPres
    {
        get => this.crdhldrPresField;
        set => this.crdhldrPresField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool CrdhldrPresSpecified
    {
        get => this.crdhldrPresFieldSpecified;
        set => this.crdhldrPresFieldSpecified = value;
    }

    /// <remarks />
    public bool OnLineCntxt
    {
        get => this.onLineCntxtField;
        set => this.onLineCntxtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool OnLineCntxtSpecified
    {
        get => this.onLineCntxtFieldSpecified;
        set => this.onLineCntxtFieldSpecified = value;
    }

    /// <remarks />
    public AttendanceContext1Code AttndncCntxt
    {
        get => this.attndncCntxtField;
        set => this.attndncCntxtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool AttndncCntxtSpecified
    {
        get => this.attndncCntxtFieldSpecified;
        set => this.attndncCntxtFieldSpecified = value;
    }

    /// <remarks />
    public TransactionEnvironment1Code TxEnvt
    {
        get => this.txEnvtField;
        set => this.txEnvtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool TxEnvtSpecified
    {
        get => this.txEnvtFieldSpecified;
        set => this.txEnvtFieldSpecified = value;
    }

    /// <remarks />
    public TransactionChannel5Code TxChanl
    {
        get => this.txChanlField;
        set => this.txChanlField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool TxChanlSpecified
    {
        get => this.txChanlFieldSpecified;
        set => this.txChanlFieldSpecified = value;
    }

    /// <remarks />
    public bool AttndntMsgCpbl
    {
        get => this.attndntMsgCpblField;
        set => this.attndntMsgCpblField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool AttndntMsgCpblSpecified
    {
        get => this.attndntMsgCpblFieldSpecified;
        set => this.attndntMsgCpblFieldSpecified = value;
    }

    /// <remarks />
    public string AttndntLang
    {
        get => this.attndntLangField;
        set => this.attndntLangField = value;
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
    public CardFallback1Code FllbckInd
    {
        get => this.fllbckIndField;
        set => this.fllbckIndField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool FllbckIndSpecified
    {
        get => this.fllbckIndFieldSpecified;
        set => this.fllbckIndFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement("SpprtdOptn")]
    public SupportedPaymentOption1Code[] SpprtdOptn
    {
        get => this.spprtdOptnField;
        set => this.spprtdOptnField = value;
    }

    #endregion
}