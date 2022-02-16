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
public partial class CardPaymentToken5
{
    #region Instance Values

    private string tknField;
    private string cardSeqNbField;
    private string tknXpryDtField;
    private string[] tknChrtcField;
    private PaymentTokenIdentifiers1 tknRqstrField;
    private decimal tknAssrncLvlField;
    private bool tknAssrncLvlFieldSpecified;
    private byte[] tknAssrncDataField;
    private string tknAssrncMtdField;
    private bool tknInittdIndField;
    private bool tknInittdIndFieldSpecified;

    /// <remarks />
    public string Tkn
    {
        get => this.tknField;
        set => this.tknField = value;
    }

    /// <remarks />
    public string CardSeqNb
    {
        get => this.cardSeqNbField;
        set => this.cardSeqNbField = value;
    }

    /// <remarks />
    public string TknXpryDt
    {
        get => this.tknXpryDtField;
        set => this.tknXpryDtField = value;
    }

    /// <remarks />
    [XmlElement("TknChrtc")]
    public string[] TknChrtc
    {
        get => this.tknChrtcField;
        set => this.tknChrtcField = value;
    }

    /// <remarks />
    public PaymentTokenIdentifiers1 TknRqstr
    {
        get => this.tknRqstrField;
        set => this.tknRqstrField = value;
    }

    /// <remarks />
    public decimal TknAssrncLvl
    {
        get => this.tknAssrncLvlField;
        set => this.tknAssrncLvlField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool TknAssrncLvlSpecified
    {
        get => this.tknAssrncLvlFieldSpecified;
        set => this.tknAssrncLvlFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] TknAssrncData
    {
        get => this.tknAssrncDataField;
        set => this.tknAssrncDataField = value;
    }

    /// <remarks />
    public string TknAssrncMtd
    {
        get => this.tknAssrncMtdField;
        set => this.tknAssrncMtdField = value;
    }

    /// <remarks />
    public bool TknInittdInd
    {
        get => this.tknInittdIndField;
        set => this.tknInittdIndField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool TknInittdIndSpecified
    {
        get => this.tknInittdIndFieldSpecified;
        set => this.tknInittdIndFieldSpecified = value;
    }

    #endregion
}